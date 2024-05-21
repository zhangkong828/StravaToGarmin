using StravaToGarmin.Client.Common.Extensions;
using StravaToGarmin.Client.Garmin;
using StravaToGarmin.Client.Strava;
using System.Diagnostics;

namespace StravaToGarmin.Client
{
    internal class Program
    {
        static string _fileDic = Path.Combine(Environment.CurrentDirectory, "files");

        static void Main(string[] args)
        {
            if (!Directory.Exists(_fileDic))
            {
                Directory.CreateDirectory(_fileDic);
            }

            var flag = Configure.Instance.Check();
            if (!flag)
            {
                return;
            }

            Start();

            Console.ReadKey();
        }

        private static async Task Start()
        {
            try
            {
                var stravaService = new StravaService();

                var info = await stravaService.Current();
                if (info == null)
                {
                    Console.WriteLine("[Strava重新登录]");
                    var isOK = await stravaService.Login();
                    if (isOK)
                    {
                        info = await stravaService.Current();
                    }
                }

                if (info == null)
                {
                    Console.WriteLine("[Strava登录失败]");
                    return;
                }
                Console.WriteLine($"[Strava登录成功] {info?.firstname}");


                var list = await stravaService.TrainingActivities("", "VirtualRide");//获取虚拟骑行
                Console.WriteLine($"[获取最近活动] {list?.Count}");

                if (list == null || list.Count == 0)
                {
                    Console.WriteLine("[获取活动，无需同步]");
                    return;
                }
                //按照时间排序
                list = list.OrderBy(x => x.start_time).ToList();

                //对比
                var oldId = Configure.Instance.StravaActivityId;
                var oldDatetime = Configure.Instance.StravaActivityDatetime;
                var index = -1;
                if (oldId > 0)
                {
                    var item = list.Where(x => x.id == oldId).FirstOrDefault();
                    if (item != null)
                    {
                        index = list.IndexOf(item);
                    }
                }

                var datas = list.Skip(index + 1).ToList();
                if (datas == null || datas.Count == 0)
                {
                    Console.WriteLine("[对比数据，无需同步]");
                    return;
                }

                //下载
                foreach (var activity in datas)
                {
                    await stravaService.ExportOriginal(activity.id);
                    await Task.Delay(2000);
                }

                var allFiles = Directory.GetFiles(_fileDic, "*.fit", SearchOption.AllDirectories);
                if (allFiles == null || allFiles.Length == 0)
                {
                    Console.WriteLine("[下载文件，无需同步]");
                    return;
                }

                //上传
                var garminService = new GarminService();
                var login = await garminService.Authenticate();
                if (!login)
                {
                    Console.WriteLine("[Garmin登录失败]");
                    return;
                }

                foreach (var file in allFiles)
                {
                    var result = await garminService.Upload(file);
                    await Task.Delay(2000);

                    File.Delete(file);
                }

                //同步最新
                var last = datas.LastOrDefault();
                Configure.Instance.SyncStravaActivity(last.id, last.start_time);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[同步失败] {ex}");
            }


            Console.WriteLine("[运行结束]");
        }
    }
}
