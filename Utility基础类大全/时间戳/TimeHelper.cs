 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utility.时间戳
{
   public class TimeHelper
    {
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name=”timeStamp”></param>
        /// <returns></returns>
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime); 
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name=”time”></param>
        /// <returns></returns>
        public static string ConvertDateTime(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return Convert.ToString((int)(time - startTime).TotalSeconds);
        }

        /// <summary>
		/// 获取以0点0分0秒开始的日期
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public static DateTime GetStartDateTime(DateTime d)
		{
			if (d.Hour != 0)
			{
				var year = d.Year;
				var month = d.Month;
				var day = d.Day;
				var hour = "0";
				var minute = "0";
				var second = "0";
				d = Convert.ToDateTime(string.Format("{0}-{1}-{2} {3}:{4}:{5}", year, month, day, hour, minute, second));
			}
			return d;
		}

		/// <summary>
		/// 获取以23点59分59秒结束的日期
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public static DateTime GetEndDateTime(DateTime d)
		{
			if (d.Hour != 23)
			{
				var year = d.Year;
				var month = d.Month;
				var day = d.Day;
				var hour = "23";
				var minute = "59";
				var second = "59";
				d = Convert.ToDateTime(string.Format("{0}-{1}-{2} {3}:{4}:{5}", year, month, day, hour, minute, second));
			}
			return d;
		}
    }
}
