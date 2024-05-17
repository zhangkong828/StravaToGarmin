using StravaToGarmin.Client.Common.Extensions;
using StravaToGarmin.Client.Strava;

namespace StravaToGarmin.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Start();

            Console.ReadKey();
        }

        private static async Task Start()
        {
            var stravaService = new StravaService();

            var isOK = await stravaService.Login();
            if (isOK)
            {
                var info = await stravaService.Current();
                Console.WriteLine(info.JsonSerializer());

                var list = await stravaService.TrainingActivities("", "VirtualRide");
                Console.WriteLine(list?.Count);
            }
        }
    }
}
