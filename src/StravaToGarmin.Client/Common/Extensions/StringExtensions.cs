using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace StravaToGarmin.Client.Common.Extensions
{
    public static class StringExtension
    {
        public static bool IsMatch(this string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }

        public static string Match(this string input, string pattern)
        {
            var match = Regex.Match(input, pattern);
            if (match.Success)
            {
                var value = match.Groups[1].Value;
                if (!string.IsNullOrWhiteSpace(value))
                    return value.Trim();
            }
            return null;
        }

        /// <summary>
        /// 格式化json字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatJsonString(this string str)
        {
            try
            {
                using JsonDocument document = JsonDocument.Parse(str);
                string formattedJson = JsonSerializer.Serialize(document.RootElement, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                return formattedJson;
            }
            catch (Exception)
            {
                return str;
            }
        }

        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonstr"></param>
        /// <returns></returns>
        public static T JsonDeserializer<T>(this string jsonstr)
        {
            if (string.IsNullOrEmpty(jsonstr))
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(jsonstr);
        }

        public static object JsonDeserializer(this string jsonstr, Type type)
        {
            if (string.IsNullOrEmpty(jsonstr))
            {
                return null;
            }
            return JsonSerializer.Deserialize(jsonstr, type);
        }

        /// <summary>
        /// 指示指定的字符串是 null、空或者仅由空白字符组成。
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 格式化String中的DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string FormatDateTimeString(this string value, string format = "yyyy-MM-dd HH:mm:ss")
        {
            if (DateTime.TryParse(value, out DateTime datetime))
            {
                return datetime.ToString(format);
            }
            return value;
        }

        /// <summary>从当前字符串开头移除另一字符串，不区分大小写，循环多次匹配前缀</summary>
        /// <param name="str">当前字符串</param>
        /// <param name="starts">另一字符串</param>
        /// <returns></returns>
        public static string TrimStart(this string str, params string[] starts)
        {
            if (string.IsNullOrEmpty(str)) return str;
            if (starts == null || starts.Length < 1 || string.IsNullOrEmpty(starts[0])) return str;

            for (var i = 0; i < starts.Length; i++)
            {
                if (str.StartsWith(starts[i], StringComparison.OrdinalIgnoreCase))
                {
                    str = str.Substring(starts[i].Length);
                    if (string.IsNullOrEmpty(str)) break;

                    // 从头开始
                    i = -1;
                }
            }
            return str;
        }

        /// <summary>从当前字符串结尾移除另一字符串，不区分大小写，循环多次匹配后缀</summary>
        /// <param name="str">当前字符串</param>
        /// <param name="ends">另一字符串</param>
        /// <returns></returns>
        public static string TrimEnd(this string str, params string[] ends)
        {
            if (string.IsNullOrEmpty(str)) return str;
            if (ends == null || ends.Length < 1 || string.IsNullOrEmpty(ends[0])) return str;

            for (var i = 0; i < ends.Length; i++)
            {
                if (str.EndsWith(ends[i], StringComparison.OrdinalIgnoreCase))
                {
                    str = str.Substring(0, str.Length - ends[i].Length);
                    if (string.IsNullOrEmpty(str)) break;

                    // 从头开始
                    i = -1;
                }
            }
            return str;
        }

        public static string UrlEncode(this string input)
        {
            if (input.IsNullOrWhiteSpace())
                return input;

            return HttpUtility.UrlEncode(input);
        }

        public static string UrlDecode(this string input)
        {
            if (input.IsNullOrWhiteSpace())
                return input;

            return HttpUtility.UrlDecode(input);
        }
    }
}
