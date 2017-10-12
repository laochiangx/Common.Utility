using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HD.Helper.Common
{
    /// <summary>
    /// 字符串操作类
    /// </summary>
    public class StringHelper
    {
        #region 列表与字符串互转
        /// <summary>
        /// 把字符串按照分隔符转换成 List
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符</param>
        /// <param name="toLower">是否转换为小写</param>
        /// <returns></returns>
        public static List<string> GetStrList(string str, char speater, bool toLower)
        {
            List<string> list = new List<string>();
            string[] ss = str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    string strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }
            }
            return list;
        }

        /// <summary>
        /// 把字符串按照分隔符转换成 List
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符</param>
        /// <param name="toLower">是否转换为小写</param>
        /// <returns></returns>
        public static List<string> GetStrList(string str, char speater)
        {
            return GetStrList(str, speater, false);
        }

        /// <summary>
        /// 把字符串转 按照, 分割 换为数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new Char[] { ',' });
        }

        /// <summary>
        /// 分割字符串，按指定的字符分割
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splitstr"></param>
        /// <returns></returns>
        public static string[] GetStrArray(string str, char splitstr)
        {
            string[] strArray = null;
            if ((str != null) && (str != ""))
            {
                strArray = new Regex(splitstr.ToString()).Split(str);
            }
            return strArray;
        }

        /// <summary>
        /// 把 List<string> 按照分隔符组装成 string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="speater"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 得到数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<int> list)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i].ToString());
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 得到数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayValueStr(Dictionary<int, int> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<int, int> kvp in list)
            {
                sb.Append(kvp.Value + ",");
            }
            if (list.Count > 0)
            {
                return DelLastComma(sb.ToString());
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 把字符串按照指定分隔符装成 List 去除重复
        /// </summary>
        /// <param name="o_str"></param>
        /// <param name="sepeater"></param>
        /// <returns></returns>
        public static List<string> GetUniqueStrArray(string o_str, char sepeater)
        {
            List<string> list = new List<string>();
            string[] ss = o_str.Split(sepeater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != sepeater.ToString())
                {
                    list.Add(s);
                }
            }
            return list;
        }
        #endregion

        #region 删除最后一个字符之后的字符

        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }

        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }

        #endregion

        #region 全角\半角转换
        /// <summary>
        /// 全角转半角的函数(DBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        /// <summary>
        /// 半角转全角的函数(SBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>        
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }
        #endregion

        #region 将字符串样式转换为纯字符串
        /// <summary>
        ///  将字符串样式转换为纯字符串
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="SplitString"></param>
        /// <returns></returns>
        public static string GetCleanStyle(string StrList, string SplitString)
        {
            string RetrunValue = "";
            //如果为空，返回空值
            if (StrList == null)
            {
                RetrunValue = "";
            }
            else
            {
                //返回去掉分隔符
                string NewString = "";
                NewString = StrList.Replace(SplitString, "");
                RetrunValue = NewString;
            }
            return RetrunValue;
        }
        #endregion

        #region 将字符串转换为新样式
        /// <summary>
        /// 将字符串转换为新样式
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="NewStyle"></param>
        /// <param name="SplitString"></param>
        /// <param name="Error"></param>
        /// <returns></returns>
        public static string GetNewStyle(string StrList, string NewStyle, string SplitString, out string Error)
        {
            string ReturnValue = "";
            //如果输入空值，返回空，并给出错误提示
            if (StrList == null)
            {
                ReturnValue = "";
                Error = "请输入需要划分格式的字符串";
            }
            else
            {
                //检查传入的字符串长度和样式是否匹配,如果不匹配，则说明使用错误。给出错误信息并返回空值
                int strListLength = StrList.Length;
                int NewStyleLength = GetCleanStyle(NewStyle, SplitString).Length;
                if (strListLength != NewStyleLength)
                {
                    ReturnValue = "";
                    Error = "样式格式的长度与输入的字符长度不符，请重新输入";
                }
                else
                {
                    //检查新样式中分隔符的位置
                    string Lengstr = "";
                    for (int i = 0; i < NewStyle.Length; i++)
                    {
                        if (NewStyle.Substring(i, 1) == SplitString)
                        {
                            Lengstr = Lengstr + "," + i;
                        }
                    }
                    if (Lengstr != "")
                    {
                        Lengstr = Lengstr.Substring(1);
                    }
                    //将分隔符放在新样式中的位置
                    string[] str = Lengstr.Split(',');
                    foreach (string bb in str)
                    {
                        StrList = StrList.Insert(int.Parse(bb), SplitString);
                    }
                    //给出最后的结果
                    ReturnValue = StrList;
                    //因为是正常的输出，没有错误
                    Error = "";
                }
            }
            return ReturnValue;
        }
        #endregion

        #region 获取正确的Id，如果不是正整数，返回0
        /// <summary>
        /// 获取正确的Id，如果不是正整数，返回0
        /// </summary>
        /// <param name="_value"></param>
        /// <returns>返回正确的整数ID，失败返回0</returns>
        public static int StrToId(string _value)
        {
            if (IsNumberId(_value))
                return int.Parse(_value);
            else
                return 0;
        }
        #endregion

        #region 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。
        /// <summary>
        /// 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。(0除外)
        /// </summary>
        /// <param name="_value">需验证的字符串。。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsNumberId(string _value)
        {
            return QuickValidate("^[1-9]*[0-9]*$", _value);
        }
        #endregion

        #region 快速验证一个字符串是否符合指定的正则表达式。
        /// <summary>
        /// 快速验证一个字符串是否符合指定的正则表达式。
        /// </summary>
        /// <param name="_express">正则表达式的内容。</param>
        /// <param name="_value">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool QuickValidate(string _express, string _value)
        {
            if (_value == null || _value.Length == 0)
            { 
                return false;
            }
            Regex myRegex = new Regex(_express);
            return myRegex.IsMatch(_value);
        }
        #endregion

        #region 根据配置对指定字符串进行MD5加密
        /// <summary>
        /// 根据配置对指定字符串进行 MD5 加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetMD5(string s)
        {
            //md5加密
            s = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(s, "md5").ToString();

            return s.ToLower().Substring(8, 16);
        }
        #endregion

        #region 得到字符串长度，一个汉字长度为2
        /// <summary>
        /// 得到字符串长度，一个汉字长度为2
        /// </summary>
        /// <param name="inputString">参数字符串</param>
        /// <returns></returns>
        public static int GetStrLength(string inputString)
        {
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
            }
            return tempLen;
        }
        #endregion

        #region 截取指定长度字符串
        /// <summary>
        /// 截取指定长度字符串
        /// </summary>
        /// <param name="inputString">要处理的字符串</param>
        /// <param name="len">指定长度</param>
        /// <returns>返回处理后的字符串</returns>
        public static string ClipString(string inputString, int len)
        {
            bool isShowFix = false;
            if (len % 2 == 1)
            {
                isShowFix = true;
                len--;
            }
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }

            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (isShowFix && mybyte.Length > len)
                tempString += "…";
            return tempString;
        }

        /// <summary>
        /// 检查字符串最大长度，返回指定长度的串
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>			
        public static string GetSubStr(string input, int maxLength)
        {
            if (input != null && input != string.Empty)
            {
                input = input.Trim();
                if (input.Length > maxLength)//按最大长度截取字符串
                    input = input.Substring(0, maxLength);
            }
            return input;
        }

        /// <summary>   
        /// 截取文本，区分中英文字符，中文算两个长度，英文算一个长度，用于数据库存储
        /// </summary>
        /// <param name="str">待截取的字符串</param>
        /// <param name="length">需计算长度的字符串</param>
        /// <returns>string</returns>
        public static string GetSqlText(string str, int length)
        {
            string temp = str;
            int j = 0;
            int k = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (Regex.IsMatch(temp.Substring(i, 1), @"[\u4e00-\u9fa5]+"))
                {
                    j += 2;
                }
                else
                {
                    j += 1;
                }
                if (j <= length)
                {
                    k += 1;
                }
                if (j > length)
                {
                    return temp.Substring(0, k) + "..";
                }
            }
            return temp;
        }

        /// <summary>
        /// 从字符串左边起给定的位置截取字符串
        /// </summary>
        /// <param name="str">给定要截取的字符串内容</param>
        /// <param name="len">截取的起始位置</param>
        /// <returns>截取后的字符串 string</returns>
        public static string GetLeftStr(string str, int len)
        {
            if (str.Length > len && len > 0)
            {
                return str.Substring(0, len - 1) + "…";
            }
            else
            {
                return str;
            }
        }

        #endregion

        #region HTML转行成TEXT
        /// <summary>
        /// HTML转行成TEXT
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string HtmlToTxt(string strHtml)
        {
            string[] aryReg ={
            @"<script[^>]*?>.*?</script>",
            @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
            @"([\r\n])[\s]+",
            @"&(quot|#34);",
            @"&(amp|#38);",
            @"&(lt|#60);",
            @"&(gt|#62);", 
            @"&(nbsp|#160);", 
            @"&(iexcl|#161);",
            @"&(cent|#162);",
            @"&(pound|#163);",
            @"&(copy|#169);",
            @"&#(\d+);",
            @"-->",
            @"<!--.*\n"
            };

            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, string.Empty);
            }

            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");


            return strOutput;
        }
        #endregion

        #region 字符串转换\数字转换 转换为安全格式
        /// <summary>
        /// 返回安全整数类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>int16 整形数字</returns>
        public static string safeInt(string str)
        {
            string constr = "0";
            try
            {
                constr = (Convert.ToInt16(str)).ToString();
                return constr;
            }
            catch
            {
                return constr;
            }
        }

        /// <summary>
        /// 转化为安全字符串 去除其中的英文单引号
        /// </summary>
        /// <param name="str">字符串内容</param>
        /// <returns>string 处理后的</returns>
        public static string safeStr(string str)
        {
            try
            {
                str = str.Replace("'", "'");
                str = str.Replace("\"", "&quot;");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                return str;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 把字符串转化为安全的text内容
        /// </summary>
        /// <param name="str">传入的字符串内容</param>
        /// <returns>转化后结果 string</returns>
        public static string safeText(string str)
        {
            try
            {
                str = str.Replace("'", "'");
                str = str.Replace("\"", "&quot;");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                str = str.Replace("\n", "<br />");
                return str;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 把'还原成单引号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string safeStrToSource(string str)
        {
            try
            {
                str = str.Replace("&lt;", "<");
                str = str.Replace("&gt;", ">");
                str = str.Replace("&quot;", "\"");
                str = str.Replace("'", "'");
                str = str.Replace("&nbsp;", " ");
                return str;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 把经过安全的text内容转化为源字符串
        /// </summary>
        /// <param name="str">传入的字符串内容</param>
        /// <returns></returns>
        public static string safeTextToSource(string str)
        {
            try
            {
                str = str.Replace("<br />", "\n");
                str = str.Replace("&lt;", "<");
                str = str.Replace("&gt;", ">");
                str = str.Replace("&quot;", "\"");
                str = str.Replace("'", "'");
                str = str.Replace("&nbsp;", " ");
                return str;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 是否安全
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsSafeSql(string str)
        {
            string sql = str.ToLower();
            if (sql.Contains("delete"))
            {
                return false;
            }
            else if (sql.Contains("select"))
            {
                return false;
            }
            else if (sql.Contains("update"))
            {
                return false;
            }
            else if (sql.Contains("drop"))
            {
                return false;
            }
            else if (sql.Contains("execute"))
            {
                return false;
            }
            else if (sql.Contains("exec"))
            {
                return false;
            }
            else if (sql.Contains("'"))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 替换不安全sql
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SafeSql(string str)
        {
            System.Text.StringBuilder sb = new StringBuilder(str.ToLower());
            sb.Replace("delete", "");
            sb.Replace("select", "");
            sb.Replace("update", "");
            sb.Replace("drop", "");
            sb.Replace("execute", "");
            sb.Replace("exec", "");
            sb.Replace("'", "");
            return sb.ToString();
        }
        #endregion

        #region 从字符串里随机得到，规定个数的字符串.

        /// <summary>
        /// 从字符串里随机得到，规定个数的字符串.
        /// </summary>
        /// <param name="allChar"></param>
        /// <param name="CodeCount"></param>
        /// <returns></returns>
        private string GetRandomCode(string allChar, int CodeCount)
        {
            //string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z"; 
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(allCharArray.Length - 1);

                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }

                temp = t;
                RandomCode += allCharArray[t];
            }
            return RandomCode;
        }

        #endregion

        #region String与Html互转
        /// <summary>
        /// 转换成 HTML code
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string StrToHtml(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }
        /// <summary>
        ///解析html成 普通文本
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string HtmlToStr(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }
        #endregion

        #region 页面HTML格式化
        public static string GetHtml(string sDetail)
        {
            Regex r;
            Match m;
            #region 处理空格
            sDetail = sDetail.Replace(" ", "&nbsp;");
            #endregion
            #region 处理单引号
            sDetail = sDetail.Replace("'", "’");
            #endregion
            #region 处理双引号
            sDetail = sDetail.Replace("\"", "&quot;");
            #endregion
            #region html标记符
            sDetail = sDetail.Replace("<", "&lt;");
            sDetail = sDetail.Replace(">", "&gt;");

            #endregion
            #region 处理换行
            //处理换行，在每个新行的前面添加两个全角空格
            r = new Regex(@"(\r\n((&nbsp;)|　)+)(?<正文>\S+)", RegexOptions.IgnoreCase);
            for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
            {
                sDetail = sDetail.Replace(m.Groups[0].ToString(), "<BR>　　" + m.Groups["正文"].ToString());
            }
            //处理换行，在每个新行的前面添加两个全角空格
            sDetail = sDetail.Replace("\r\n", "<BR>");
            #endregion

            return sDetail;
        }
        #endregion

        #region 清除字符串中的危险字符
        /// <summary>
        /// 清除字符串中的危险字符
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string ClearInputText(string inputString, int maxLength)
        {
            StringBuilder retVal = new StringBuilder();

            // 检查是否为空
            if ((inputString != null) && (inputString != String.Empty))
            {
                inputString = inputString.Trim();

                //检查长度
                if (inputString.Length > maxLength)
                    inputString = inputString.Substring(0, maxLength);

                //替换危险字符
                for (int i = 0; i < inputString.Length; i++)
                {
                    switch (inputString[i])
                    {
                        case '"':
                            retVal.Append("&quot;");
                            break;
                        case '<':
                            retVal.Append("&lt;");
                            break;
                        case '>':
                            retVal.Append("&gt;");
                            break;
                        default:
                            retVal.Append(inputString[i]);
                            break;
                    }
                }
                retVal.Replace("'", " ");// 替换单引号
            }
            return retVal.ToString();

        }
        #endregion
    }
}
