using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StravaToGarmin.Client.Common.Extensions
{
    public class IsoDateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string _dateFormat;
        public IsoDateTimeConverter(string dateFormat = "yyyy-MM-dd'T'HH:mm:ss.fffK")
        {
            _dateFormat = dateFormat;
        }


        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string dateString = reader.GetString();
            DateTime dateTime;
            try
            {
                // 尝试解析ISO 8601格式
                dateTime = DateTime.ParseExact(dateString, "yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            }
            catch (FormatException)
            {
                // 如果ISO 8601格式解析失败，尝试其他格式
                dateTime = DateTime.Parse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            }
            return dateTime;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_dateFormat, CultureInfo.InvariantCulture));
        }
    }
}
