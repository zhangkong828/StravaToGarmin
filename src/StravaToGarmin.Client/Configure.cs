using StravaToGarmin.Client.Common;
using StravaToGarmin.Client.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StravaToGarmin.Client
{
    public class Configure
    {
        static string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.config");
        private static Configure _instance = new Configure();

        public static Configure Instance
        {
            get
            {
                return _instance;
            }
        }

        private void LoadConfigure()
        {
            try
            {
                StravaEmail = GetConfig("StravaEmail");
                StravaPassword = GetConfig("StravaPassword");

                GarminUsername = GetConfig("GarminUsername");
                GarminPassword = GetConfig("GarminPassword");

                StravaActivityId = long.TryParse(GetConfig("StravaActivityId"), out long stravaActivityId) ? stravaActivityId : 0;
                StravaActivityDatetime = DateTime.TryParse(GetConfig("StravaActivityDatetime"), out DateTime stravaActivityDatetime) ? stravaActivityDatetime : DateTime.MinValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        string GetConfig(string key)
        {
            return FileHelper.ReadKeyValue(configPath, key);
        }

        public bool Check()
        {
            LoadConfigure();

            if (StravaEmail.IsNullOrWhiteSpace() || StravaPassword.IsNullOrWhiteSpace())
            {
                Console.WriteLine("[Strava配置错误]");
                return false;
            }

            if (GarminUsername.IsNullOrWhiteSpace() || GarminPassword.IsNullOrWhiteSpace())
            {
                Console.WriteLine("[Garmin配置错误]");
                return false;
            }

            return true;
        }


        public void SyncStravaActivity(long activityId, DateTime dateTime)
        {
            StravaActivityId = activityId;
            StravaActivityDatetime = dateTime;
            FileHelper.SaveKeyValue(configPath, "StravaActivityId", activityId.ToString());
            FileHelper.SaveKeyValue(configPath, "StravaActivityDatetime", dateTime.ToString());
        }


        public string StravaEmail { get; set; }

        public string StravaPassword { get; set; }

        public string GarminUsername { get; set; }

        public string GarminPassword { get; set; }


        public long StravaActivityId { get; set; }

        public DateTime StravaActivityDatetime { get; set; }
    }
}
