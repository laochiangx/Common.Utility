using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.Collections;
using System.Text.RegularExpressions;

namespace ActiveDirectoryDome
{
    class Program
    {
        static void Main(string[] args)
        {
            if (TryAuthenticate("prepat.net", "administrator", "yclb2009@"))
            {
                Console.WriteLine("登陆成功！");
            }
            else
            {
                Console.WriteLine("登陆失败！");
            }
            Console.WriteLine("*******************************************************");
            List<string> group = new List<string>();
            group = GetADGroups("administrator", "prepat.net", "administrator", "yclb2009@");
            foreach (string temp in group)
            {
                Console.WriteLine("组名称:"+temp.ToString());
            }

            Console.ReadLine();
        }

        /// <summary>
        /// 验证AD用户是否登陆成功
        /// </summary>
        /// <param name="domain">域名称</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>返回登陆状态</returns>
        public static bool TryAuthenticate(string domain, string username, string password)
        {
            bool isLogin = false;
            try
            {
                DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}", domain), username, password);
                entry.RefreshCache();
                isLogin = true;
            }
            catch
            {
                isLogin = false;
            }
            return isLogin;
        }

        /// <summary>
        /// 取用户所对应的用户组
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <param name="domain">域</param>
        /// <param name="adusername">登陆用户</param>
        /// <param name="adpassword">登陆密码</param>
        /// <returns></returns>
        public static List<string> GetADGroups(string userName,string domain,string adusername,string adpassword)
        {
            List<string> groups = new List<string>();
            try
            {
                var entry = new DirectoryEntry(string.Format("LDAP://{0}", domain), adusername, adpassword);
                entry.RefreshCache();

                DirectorySearcher search = new DirectorySearcher(entry);
                search.PropertiesToLoad.Add("memberof");
                search.Filter = string.Format("sAMAccountName={0}", userName);
                SearchResult result = search.FindOne();

                if (result != null)
                {
                    ResultPropertyValueCollection c = result.Properties["memberof"];
                    foreach (var a in c)
                    {
                        string temp = a.ToString();
                        Match match = Regex.Match(temp, @"CN=\s*(?<g>\w*)\s*.");
                        groups.Add(match.Groups["g"].Value);
                    }
                }
            }
            catch
            {
 
            }
            return groups;
        }
    }
}
