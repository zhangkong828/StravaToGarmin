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


        public string StravaEmail { get; set; }

        public string StravaPassword { get; private set; }

        public string GarminUsername { get; private set; }

        public string GarminPassword { get; private set; }



    }
}
