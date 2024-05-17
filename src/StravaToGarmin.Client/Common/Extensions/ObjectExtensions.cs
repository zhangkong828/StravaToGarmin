using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StravaToGarmin.Client.Common.Extensions
{
    public static class ObjectExtension
    {
        /// <summary>
        /// Json序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JsonSerializer(this object obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj);
        }
    }
}
