using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StravaToGarmin.Client.Strava.Models
{
    public class TrainingActivitiesResponse
    {
        public List<Activity> models { get; set; }
        public int page { get; set; }
        public int perPage { get; set; }
        public int total { get; set; }
    }

    public class Activity
    {
        /// <summary>
        /// 活动id
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 运动类型  VirtualRide
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 运动类型 模拟骑行
        /// </summary>
        public string display_type { get; set; }
        /// <summary>
        /// 活动类型 模拟骑行
        /// </summary>
        public string activity_type_display_name { get; set; }
        public bool _private { get; set; }
        public object bike_id { get; set; }
        public object athlete_gear_id { get; set; }
        /// <summary>
        /// 开始时间 2024/5/16周四
        /// </summary>
        public string start_date { get; set; }
        /// <summary>
        /// 开始时间 时间戳 秒  1715758941  
        /// ISO 8601格式
        /// </summary>
        public int start_date_local_raw { get; set; }
        /// <summary>
        /// 开始时间  utc时间
        /// </summary>
        public DateTime start_time { get; set; }
        /// <summary>
        /// 星期 Thu
        /// </summary>
        public string start_day { get; set; }
        /// <summary>
        /// 距离 2位数
        /// </summary>
        public string distance { get; set; }
        /// <summary>
        /// 距离 精确数
        /// </summary>
        public float distance_raw { get; set; }
        /// <summary>
        /// 单位  千米
        /// </summary>
        public string long_unit { get; set; }
        /// <summary>
        /// 单位 km
        /// </summary>
        public string short_unit { get; set; }
        /// <summary>
        /// 时间 时分秒  1:00:35
        /// </summary>
        public string moving_time { get; set; }
        /// <summary>
        /// 时间 毫秒  3635
        /// </summary>
        public int moving_time_raw { get; set; }
        /// <summary>
        /// 时间 时分秒  1:00:35
        /// </summary>
        public string elapsed_time { get; set; }
        /// <summary>
        /// 时间 毫秒  3635
        /// </summary>
        public int elapsed_time_raw { get; set; }
        public bool trainer { get; set; }
        public string static_map { get; set; }
        public bool has_latlng { get; set; }
        public bool commute { get; set; }
        /// <summary>
        /// 海拔 
        /// </summary>
        public string elevation_gain { get; set; }
        /// <summary>
        /// 海拔单位 m
        /// </summary>
        public string elevation_unit { get; set; }
        public float elevation_gain_raw { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 活动地址
        /// </summary>
        public string activity_url { get; set; }
        public string activity_url_for_twitter { get; set; }
        public string twitter_msg { get; set; }
        public bool is_new { get; set; }
        public bool is_changing_type { get; set; }
        /// <summary>
        /// 相对负荷度
        /// </summary>
        public float suffer_score { get; set; }
        public int? workout_type { get; set; }
        public bool flagged { get; set; }
        public bool hide_power { get; set; }
        public bool hide_heartrate { get; set; }
        public bool leaderboard_opt_out { get; set; }
        /// <summary>
        /// 访问权限 everyone
        /// </summary>
        public string visibility { get; set; }
    }

}
