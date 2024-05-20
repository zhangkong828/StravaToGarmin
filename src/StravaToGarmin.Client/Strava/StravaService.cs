using StravaToGarmin.Client.Common.Extensions;
using StravaToGarmin.Client.Strava.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace StravaToGarmin.Client.Strava
{
    public class StravaService
    {
        private readonly HttpClient _httpClient;
        private readonly CookieContainer _cookieContainer;
        string _configPath = Environment.CurrentDirectory + @"\strava.ini";

        public StravaService()
        {
            _cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = _cookieContainer
            };

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://www.strava.com"),
                Timeout = TimeSpan.FromSeconds(30),

            };
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36 Edg/124.0.0.0");

            if (File.Exists(_configPath))
            {
                var cookieString = File.ReadAllText(_configPath);
                _httpClient.DefaultRequestHeaders.Add("Cookie", cookieString);
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Login()
        {
            //访问login页面  获取form参数
            var response = await _httpClient.GetAsync("/login");
            response.EnsureSuccessStatusCode().WriteRequestToConsole();
            var loginHtml = await response.Content.ReadAsStringAsync();

            //var utf8 = loginHtml.Match("input name=\"utf8\" type=\"hidden\" value=\"(.+?)\"");
            var utf8 = "%E2%9C%93";
            var authenticity_token = loginHtml.Match("name=\"csrf-token\" content=\"(.+?)\"");

            if (utf8.IsNullOrWhiteSpace() || authenticity_token.IsNullOrWhiteSpace())
            {
                Console.WriteLine("[访问Strava登录页面失败]");
                return false;
            }

            //登录
            _httpClient.DefaultRequestHeaders.Referrer = new Uri("https://www.strava.com/login");
            var postData = new StringContent($"utf8={utf8}&authenticity_token={authenticity_token.UrlEncode()}&plan=&email={Configure.Instance.StravaEmail.UrlEncode()}&password={Configure.Instance.StravaPassword}", new MediaTypeHeaderValue("application/x-www-form-urlencoded"));
            response = await _httpClient.PostAsync("/session", postData);
            response.EnsureSuccessStatusCode().WriteRequestToConsole();

            var html = await response.Content.ReadAsStringAsync();

            if (html.IsMatch("<title>Dashboard \\| Strava</title>"))
            {
                var cookieString = _cookieContainer.GetCookieHeader(new Uri("https://strava.com"));
                File.WriteAllText(_configPath, cookieString);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<CurrentAthlete> Current()
        {
            var response = await _httpClient.GetAsync("/frontend/athletes/current");
            response.EnsureSuccessStatusCode().WriteRequestToConsole();
            var result = await response.Content.ReadAsStringAsync();
            var current = result.JsonDeserializer<FrontendAthletesCurrentResponse>();
            return current?.currentAthlete;
        }

        /// <summary>
        /// 获取活动
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="activity_type">运动类型</param>
        /// <returns></returns>
        public async Task<List<Activity>> TrainingActivities(string keywords, string activity_type)
        {
            /*
            <option value="Ride">骑行</option>           
            <option value="VirtualRide">模拟骑行</option>

            必须参数
            X-CSRF-Token: ENuXXG3hmC5SWztYNzErOOifP7OTSx5F+OPwq6dj10yGq7kQvXgP2aiEoNi+WzLb/uJF+1Mso32oga51oW7Eng==
            X-Requested-With: XMLHttpRequest

             */


            //获取token
            var response = await _httpClient.GetAsync("/athlete/training");
            response.EnsureSuccessStatusCode().WriteRequestToConsole();
            var trainingHtml = await response.Content.ReadAsStringAsync();
            var token = trainingHtml.Match("name=\"csrf-token\" content=\"(.+?)\"");
            if (token.IsNullOrWhiteSpace())
            {
                Console.WriteLine("访问Strava活动页面失败");
                return null;
            }


            var url = $"/athlete/training_activities?keywords={keywords ?? string.Empty}&activity_type={activity_type ?? "Ride"}&workout_type=&commute=&private_activities=&trainer=&gear=&search_session_id=&new_activity_only=false&order=&page=1";

            _httpClient.DefaultRequestHeaders.Add("X-CSRF-Token", token);
            _httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

            response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode().WriteRequestToConsole();
            var result = await response.Content.ReadAsStringAsync();
            var activities = result.JsonDeserializer<TrainingActivitiesResponse>(true);
            if (activities != null && activities.models != null)
                return activities.models;

            return null;
        }

        /// <summary>
        /// 下载fit文件
        /// </summary>
        /// <param name="activity_id"></param>
        /// <returns></returns>
        public async Task<bool> ExportOriginal(long activity_id)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "files", $"{activity_id}.fit");
            var response = await _httpClient.GetAsync($"/activities/{activity_id}/export_original");
            response.EnsureSuccessStatusCode().WriteRequestToConsole();
            using (Stream responseStream = await response.Content.ReadAsStreamAsync())
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await responseStream.CopyToAsync(fileStream);
                }
            }
            Console.WriteLine($"[下载成功] {activity_id}.fit");
            return true;
        }
    }
}
