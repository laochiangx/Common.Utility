 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace Common.Utility
{
    /// <summary>
    /// 防止跨站脚本攻击的代码
    /// </summary>
    public class WebSafe
    {
        private const string StrRegex = @"^\+/v(8|9)|\b(and|or)\b.{1,6}?(=|>|<|\bin\b|\blike\b)|/\*.+?\*/|<\s*script\b|<\s*img\b|\bEXEC\b|UNION.+?SELECT|UPDATE.+?SET|INSERT\s+INTO.+?VALUES|(SELECT|DELETE).+?FROM|(CREATE|ALTER|DROP|TRUNCATE)\s+(TABLE|DATABASE)";

        public static bool PostData()
        {
            bool result = false;
            for (int i = 0; i < HttpContext.Current.Request.Form.Count; i++)
            {
                result = CheckData(HttpContext.Current.Request.Form[i].ToString());
                if (result)
                {
                    break;
                }
            }
            return result;
        }


        public static bool GetData()
        {
            bool result = false;

            for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
            {
                result = CheckData(HttpContext.Current.Request.QueryString[i].ToString());
                if (result)
                {
                    break;
                }
            }
            return result;
        }


        public static bool CookieData()
        {
            bool result = false;
            for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
            {
                result = CheckData(HttpContext.Current.Request.Cookies[i].Value.ToLower());
                if (result)
                {
                    break;
                }
            }
            return result;

        }
        public static bool referer()
        {
            bool result = false;
            return result = CheckData(HttpContext.Current.Request.UrlReferrer.ToString());
        }

        public static bool CheckData(string inputData)
        {
            if (Regex.IsMatch(inputData, StrRegex))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
