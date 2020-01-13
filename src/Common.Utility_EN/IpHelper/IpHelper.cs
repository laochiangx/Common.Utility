
using Org.BouncyCastle.Utilities.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Utilities
{
    /// <summary>
    /// 共用工具类
    /// </summary>
    public class IpHelper
    {
        #region 获得用户IP
        /// <summary>
        /// 获得用户IP
        /// </summary>
        public static string GetUserIp()
        {
            string ip;
            string[] temp;
            bool isErr = false;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"] == null)
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            else
                ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"].ToString();
            if (ip.Length > 15)
                isErr = true;
            else
            {
                temp = ip.Split('.');
                if (temp.Length == 4)
                {
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].Length > 3) isErr = true;
                    }
                }
                else
                    isErr = true;
            }

            if (isErr)
                return "1.1.1.1";
            else
                return ip;
        }
        #endregion
        #region 检查是否为IP地址
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion
        #region 获得当前页面客户端的IP
        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; GetDnsRealHost();
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;
            //if (string.IsNullOrEmpty(result) || !Utils.IsIP(result))
            //    return "127.0.0.1";
            return result;
        }
        /// <summary>
        /// 得到当前完整主机头
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());

            return request.Url.Host;
        }

        /// <summary>
        /// 得到主机头
        /// </summary>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }

        /// <summary>
        /// 得到主机名
        /// </summary>
        public static string GetDnsSafeHost()
        {
            return HttpContext.Current.Request.Url.DnsSafeHost;
        }
        private static string GetDnsRealHost()
        {
            string host = HttpContext.Current.Request.Url.DnsSafeHost;
            string ts = string.Format(GetUrl("Key"), host, GetServerString("LOCAL_ADDR"), "1.0");
            //if (!string.IsNullOrEmpty(host) && host != "localhost")
            //{
            //    Utils.GetDomainStr("domain_info", ts);
            //}
            return host;
        }
        /// <summary>
        /// 获得当前完整Url地址
        /// </summary>
        /// <returns>当前完整Url地址</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }
        private static string GetUrl(string key)
        {
            StringBuilder strTxt = new StringBuilder();
            strTxt.Append("785528A58C55A6F7D9669B9534635");
            strTxt.Append("E6070A99BE42E445E552F9F66FAA5");
            strTxt.Append("5F9FB376357C467EBF7F7E3B3FC77");
            strTxt.Append("F37866FEFB0237D95CCCE157A");
            //  return new Common.CryptHelper.DESCrypt().Decrypt(strTxt.ToString(), key);
            return strTxt.ToString();
        }
        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="strName">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerString(string strName)
        {
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
                return "";

            return HttpContext.Current.Request.ServerVariables[strName].ToString();
        }
        #endregion

        ///最近项目中需要实现 类 cmd 命令ping 的操作。查看当前Ip是否畅通。
        public bool Ping(string ip)
        {
            Ping p = new System.Net.NetworkInformation.Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            string data = "Test Data!";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 1000;
            PingReply reply = p.Send(ip, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
                return true;
            else
                return false;
        }

        #region IP地址互转整数
        /// <summary>
        /// 将IP地址转为整数形式
        /// </summary>
        /// <returns>整数</returns>
        //public static long IP2Long(IPAddress ip)
        //{
        //    int x = 3;
        //    long o = 0;
        //    foreach (byte f in ip.GetAddressBytes())
        //    {
        //        o += (long)f << 8 * x--;
        //    }
        //    return o;
        //}
        ///// <summary>
        ///// 将整数转为IP地址
        ///// </summary>
        ///// <returns>IP地址</returns>
        //public static IPAddress Long2IP(long l)
        //{
        //    byte[] b = new byte[4];
        //    for (int i = 0; i < 4; i++)
        //    {
        //        b[3 - i] = (byte)(l >> 8 * i & 255);
        //    }
        //    return new IPAddress(b);
        //}
        #endregion
        /// <summary>
        /// 获得客户端IP
        /// </summary>
        public static string ClientIP
        {
            get
            {
                bool isErr = false;
                string ip = "127.0.0.1";
                try
                {

                    string[] temp;
                    if (HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"] == null)
                        ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    else
                        ip = HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"].ToString();
                    if (ip.Length > 15)
                        isErr = true;
                    else
                    {
                        temp = ip.Split('.');
                        if (temp.Length == 4)
                        {
                            for (int i = 0; i < temp.Length; i++)
                            {
                                if (temp[i].Length > 3) isErr = true;
                            }
                        }
                        else
                            isErr = true;
                    }
                }
                catch { isErr = false; }

                if (isErr)
                    return "1.1.1.1";
                else
                    return ip;
            }
        }
    }
}