using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace HD.Helper.Common
{
    /// <summary>
    /// 获取MD5值帮助类
    /// </summary>
    public class MD5Encrypt
    {
        /// <summary>
        /// 得到字符串的md5值
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <returns></returns>
        public static string GetStrMD5(string str)
        {
            MD5CryptoServiceProvider md = new MD5CryptoServiceProvider();
            byte[] bt = md.ComputeHash(Encoding.Default.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bt.Length; i++)
            {
                sb.Append(bt[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 得到文件的md5值
        /// </summary>
        /// <param name="str">传入的文件路径</param>
        /// <returns></returns>
        public static string GetFileMD5(string path)
        {
            MD5CryptoServiceProvider md = new MD5CryptoServiceProvider();
            byte[] bt;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                bt = md.ComputeHash(fs);
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bt.Length; i++)
            {
                sb.Append(bt[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
