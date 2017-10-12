using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.International.Converters.PinYinConverter;

namespace HD.Helper.Common
{
    /// <summary>
    /// 中文转换拼音帮助类
    /// </summary>
    public class PinYinHelper
    {
        /// <summary>
        /// 获取字符的拼音，没有音标
        /// </summary>
        /// <param name="ch">传入的字符</param>
        /// <returns>返回的字符的拼音</returns>
        public static string GetPinYinChar(char ch)
        {
            string res = "";
            if (ChineseChar.IsValidChar(ch))
            {
                ChineseChar cc = new ChineseChar(ch);
                res = cc.Pinyins[0].ToString().Substring(0, cc.Pinyins[0].Length - 1);
            }
            else
            {
                res = ch.ToString();
            }
            return res;
        }

        /// <summary>
        /// 获取字符串的拼音
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <returns>返回字符串的声音</returns>
        public static string GetPinYinStr(string str)
        {
            char[] ch = str.ToCharArray();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ch.Length; i++)
            {
                sb.Append(GetPinYinChar(ch[i]));
            }
            return sb.ToString();

        }
    }
}
