using StravaToGarmin.Client.Common.Extensions;
using StravaToGarmin.Client.Garmin;
using StravaToGarmin.Client.Strava;
using System.Diagnostics;

namespace StravaToGarmin.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "files");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
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
                    Console.WriteLine("[重新登录]");
                    var isOK = await stravaService.Login();
                    if (isOK)
                    {
                        info = await stravaService.Current();
                    }
                }

                if (info == null)
                {
                    Console.WriteLine("[登录失败]");
                    return;
                }
                Console.WriteLine($"[登录成功] {info?.firstname}");


                var list = await stravaService.TrainingActivities("", "VirtualRide");//获取虚拟骑行
                Console.WriteLine($"[获取最近活动] {list?.Count}");

                if (list != null && list.Any())
                {
                    await stravaService.ExportOriginal(list.FirstOrDefault().id);

                }

                var filePath = Path.Combine(Environment.CurrentDirectory, "files", $"11424322350.fit");
                var garminService = new GarminService();
                var login = await garminService.Authenticate();
                if (login)
                {
                    await garminService.Upload(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[同步失败] {ex}");
            }

        }
    }
}
