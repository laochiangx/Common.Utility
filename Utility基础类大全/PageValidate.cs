using System;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace DotNet.Utilities
{
    public class PageValidate
    {
        private static Regex RegNumber = new Regex("^[0-9]+$");
        private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
        private static Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");

        public PageValidate()
        {
        }


        #region 数字字符串检查
        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="inputData">源字符串</param>
        /// <param name="formatlevel">0:不做验证| 1:sql语句参数| 2:存储过程参数| 3:EncodeHtml| 4:Encode+sql| 5:Encode+存储过程</param>
        /// <returns>返回格式化后的字符串</returns>
        public static string FormatString(string inputData, int formatlevel)
        {
            return inputData;
        }
        /// <summary>
        /// 检查Request查询字符串的键值，是否是数字，最大长度限制
        /// </summary>
        /// <param name="req">Request</param>
        /// <param name="inputKey">Request的键值</param>
        /// <param name="maxLen">最大长度</param>
        /// <returns>返回Request查询字符串</returns>
        public static string FetchInputDigit(HttpRequest req, string inputKey, int maxLen)
        {
            string retVal = string.Empty;
            if (inputKey != null && inputKey != string.Empty)
            {
                retVal = req.QueryString[inputKey];
                if (null == retVal)
                    retVal = req.Form[inputKey];
                if (null != retVal)
                {
                    retVal = SqlText(retVal, maxLen);
                    if (!IsNumber(retVal))
                        retVal = string.Empty;
                }
            }
            if (retVal == null)
                retVal = string.Empty;
            return retVal;
        }

        public enum CheckType
        { None, Int, SignInt, Float, SignFloat, Chinese, Mail }
        /// <summary>
        /// 检测字符串类型
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <param name="checktype">0:不检测| 1:数字| 2:符号数字| 3: 浮点数| 4:符号浮点| 5: 中文?| 6:邮件?</param>
        /// <returns></returns>
        public static bool checkString(string inputData, int checktype)
        {

            bool _return = false;
            switch (checktype)
            {
                case 0:
                    _return = true;
                    break;
                case 1:
                    _return = IsNumber(inputData);
                    break;
                case 2:
                    _return = IsNumberSign(inputData);
                    break;
                case 3:
                    _return = IsDecimal(inputData);
                    break;
                case 4:
                    _return = IsDecimalSign(inputData);
                    break;
                case 5:
                    _return = IsHasCHZN(inputData);
                    break;
                case 6:
                    _return = IsEmail(inputData);
                    break;
                default:
                    _return = false;
                    break;
            }
            return _return;
        }
        /// <summary>
        /// 是否数字字符串
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string inputData)
        {
            Match m = RegNumber.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumberSign(string inputData)
        {
            Match m = RegNumberSign.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(string inputData)
        {
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimalSign(string inputData)
        {
            Match m = RegDecimalSign.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 中文检测

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsHasCHZN(string inputData)
        {
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }

        #endregion

        public static string GetShortDate(string dt)
        {
            return Convert.ToDateTime(dt).ToShortDateString();
        }

        #region 邮件地址
        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsEmail(string inputData)
        {
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 其他

        /// <summary>
        /// 检查字符串最大长度，返回指定长度的串
        /// </summary>
        /// <param name="sqlInput">输入字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>			
        public static string SqlText(string sqlInput, int maxLength)
        {
            if (sqlInput != null && sqlInput != string.Empty)
            {
                sqlInput = sqlInput.Trim();
                if (sqlInput.Length > maxLength)//按最大长度截取字符串
                    sqlInput = sqlInput.Substring(0, maxLength);
            }
            return sqlInput;
        }


        /// <summary>
        /// 字符串编码
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static string HtmlEncode(string inputData)
        {
            return HttpUtility.HtmlEncode(inputData);
        }
        /// <summary>
        /// 设置Label显示Encode的字符串
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="txtInput"></param>
        public static void SetLabel(Label lbl, string txtInput)
        {
            lbl.Text = HtmlEncode(txtInput);
        }
        public static void SetLabel(Label lbl, object inputObj)
        {
            SetLabel(lbl, inputObj.ToString());
        }

        #endregion

        #region 对于用户权限从数据库中读出的解密过程
        public static string switch_riddle(string s_ch)//解密
        {
            string s_out, s_temp, temp;
            int i_len = s_ch.Length;
            if (i_len == 0 || s_ch == "")
            {
                s_out = "0";
            }
            temp = "";
            s_temp = "";
            s_out = "";
            for (int i = 0; i <= i_len - 1; i++)
            {
                temp = s_ch.Substring(i, 1);
                switch (temp)
                {
                    case "a": s_temp = "1010";
                        break;
                    case "b": s_temp = "1011";
                        break;
                    case "c": s_temp = "1100";
                        break;
                    case "d": s_temp = "1101";
                        break;
                    case "e": s_temp = "1110";
                        break;
                    case "f": s_temp = "1111";
                        break;
                    case "0": s_temp = "0000";
                        break;
                    case "1": s_temp = "0001";
                        break;
                    case "2": s_temp = "0010";
                        break;
                    case "3": s_temp = "0011";
                        break;
                    case "4": s_temp = "0100";
                        break;
                    case "5": s_temp = "0101";
                        break;
                    case "6": s_temp = "0110";
                        break;
                    case "7": s_temp = "0111";
                        break;
                    case "8": s_temp = "1000";
                        break;
                    case "9": s_temp = "1001";
                        break;
                    default: s_temp = "0000";
                        break;
                }
                s_out = s_out + s_temp;
                s_temp = "";
            }
            return s_out;
        }
        #endregion

        #region 用户权限的加密过程
        public static string switch_encrypt(string s_ch)
        {
            string s_out, s_temp, temp;
            int i_len = 64;
            if (i_len == 0 || s_ch == "")
            {
                s_out = "0000";
            }
            temp = "";
            s_temp = "";
            s_out = "";
            for (int i = 0; i <= i_len - 1; i = i + 4)
            {
                temp = s_ch.Substring(i, 4);
                switch (temp)
                {
                    case "1010": s_temp = "a";
                        break;
                    case "1011": s_temp = "b";
                        break;
                    case "1100": s_temp = "c";
                        break;
                    case "1101": s_temp = "d";
                        break;
                    case "1110": s_temp = "e";
                        break;
                    case "1111": s_temp = "f";
                        break;
                    case "0000": s_temp = "0";
                        break;
                    case "0001": s_temp = "1";
                        break;
                    case "0010": s_temp = "2";
                        break;
                    case "0011": s_temp = "3";
                        break;
                    case "0100": s_temp = "4";
                        break;
                    case "0101": s_temp = "5";
                        break;
                    case "0110": s_temp = "6";
                        break;
                    case "0111": s_temp = "7";
                        break;
                    case "1000": s_temp = "8";
                        break;
                    case "1001": s_temp = "9";
                        break;
                    default: s_temp = "0";
                        break;
                }
                s_out = s_out + s_temp;
                s_temp = "";
            }
            return s_out;
        }//加密
        #endregion

        #region   访问权限
        public static bool CheckTrue(string s_admin, int a)
        {
            string s_temp = "";
            s_temp = s_admin.Substring(a - 1, 1);   //s_admin为全局变量
            if (s_temp == "" || s_temp == "1")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region   检测字符串长度
        /// <summary>   
        /// 计算文本长度，区分中英文字符，中文算两个长度，英文算一个长度
        /// </summary>
        /// <param name="Text">需计算长度的字符串</param>
        /// <returns>int</returns>
        public static int Text_Length(string Text)
        {
            int len = 0;

            for (int i = 0; i < Text.Length; i++)
            {
                byte[] byte_len = Encoding.Default.GetBytes(Text.Substring(i, 1));
                if (byte_len.Length > 1)
                    len += 2;  //如果长度大于1，是中文，占两个字节，+2
                else
                    len += 1;  //如果长度等于1，是英文，占一个字节，+1
            }

            return len;
        }
        #endregion

        #region   字符串长度区分中英文截取
        /// <summary>   
        /// 截取文本，区分中英文字符，中文算两个长度，英文算一个长度
        /// </summary>
        /// <param name="str">待截取的字符串</param>
        /// <param name="length">需计算长度的字符串</param>
        /// <returns>string</returns>
        public static string GetSubString(string str, int length)
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

        #region 分页
        //public static string paging(string url, string para, int sumpage, int page)
        //{
        //    string result = string.Empty;
        //    if (sumpage == 1)
        //    {
        //        return result;
        //    }
        //    if (page > sumpage)
        //    {
        //        page = 1;
        //    }
        //    if (sumpage > 0)
        //    {
        //        for (int i = 1; i <= sumpage; i++)
        //        {
        //            if (i == page)
        //            {
        //                result += string.Format("<a class=\"a_page\" href=\"{0}?page={1}{2}\">{3}</a> ", new object[] { url, i.ToString(), para, i.ToString() });
        //            }
        //            else
        //            {
        //                result += string.Format("<a href=\"{0}?page={1}{2}\">{3}</a> ", new object[] { url, i.ToString(), para, i.ToString() });
        //            }
        //        }
        //    }
        //    return result;
        //}

        public static string paging(string url, string para, int sumpage, int page)
        {
            string result = string.Empty;
            if (sumpage == 1)
            {
                return result;
            }
            if (sumpage > 500)
            {
                sumpage = 500;
            }
            if (page > sumpage)
            {
                page = 1;
            }
            StringBuilder sb = new StringBuilder();
            if (sumpage > 0)
            {
                switch (page)
                {
                    case 1:
                        sb.Append(string.Format("<p class=\"next\"><a href=\"{0}?page={1}{2}\">{3}</a> ", new object[] { url, page + 1, para, "下一页" }));
                        break;
                    default:
                        if (sumpage == page)
                        {
                            sb.Append(string.Format("<p class=\"next\"><a href=\"{0}?page={1}{2}\">{3}</a> ", new object[] { url, page - 1, para, "上一页" }));
                        }
                        else
                        {
                            sb.Append(string.Format("<p class=\"next\"><a href=\"{0}?page={1}{2}\">{3}</a> <a href=\"{4}?page={5}{6}\">{7}</a> ",
                                new object[] { url, page + 1, para, "下一页", url, page - 1, para, "上一页" }));
                        }
                        break;
                }
                sb.Append(string.Format("第{0}/{1}页</p>", new object[] { page, sumpage }));
            }
            return sb.ToString();
        }

        public static string paging(string url, string para, int sumpage, int page, System.Web.UI.UserControl myPaging)
        {
            myPaging.Visible = false;
            string result = string.Empty;
            if (sumpage == 1)
            {
                return result;
            }
            if (sumpage > 500)
            {
                sumpage = 500;
            }
            if (page > sumpage)
            {
                page = 1;
            }
            StringBuilder sb = new StringBuilder();
            if (sumpage > 0)
            {
                myPaging.Visible = true;
                switch (page)
                {
                    case 1:
                        sb.Append(string.Format("<a href=\"{0}?page={1}{2}\">{3}</a> ", new object[] { url, page + 1, para, "下一页" }));
                        break;
                    default:
                        if (sumpage == page)
                        {
                            sb.Append(string.Format("<a href=\"{0}?page={1}{2}\">{3}</a> ", new object[] { url, page - 1, para, "上一页" }));
                        }
                        else
                        {
                            sb.Append(string.Format("<a href=\"{0}?page={1}{2}\">{3}</a> <a href=\"{4}?page={5}{6}\">{7}</a> ",
                                new object[] { url, page + 1, para, "下一页", url, page - 1, para, "上一页" }));
                        }
                        break;
                }
                sb.Append(string.Format("第{0}/{1}页", new object[] { page, sumpage }));
            }
            return sb.ToString();
        }

        public static string paging(string para, int sumpage, int page, int count)
        {
            string result = string.Empty;
            if (page > sumpage)
            {
                page = 1;
            }
            StringBuilder sb = new StringBuilder();
            if (sumpage > 0)
            {
                if (sumpage != 1)
                {
                    switch (page)
                    {
                        case 1:
                            sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a> ", new object[] { page + 1, para, "下一页" }));
                            break;
                        default:
                            if (sumpage == page)
                            {
                                sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a> ", new object[] { page - 1, para, "上一页" }));
                            }
                            else
                            {
                                sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a> <a href=\"?page={3}{4}\">{5}</a> ",
                                    new object[] { page - 1, para, "上一页", page + 1, para, "下一页" }));
                            }
                            break;
                    }
                }
                sb.Append(string.Format("第{0}/{1}页 共{2}条", new object[] { page, sumpage, count }));
            }
            return sb.ToString();
        }

        public static void paging(string clinktail, int sumpage, int page, System.Web.UI.WebControls.Label page_view)
        {
            if (sumpage > 0)
            {
                int n = sumpage;    //总页数
                int x = page;   //得到当前页
                int i;
                int endpage;
                string pageview = "", pageviewtop = "";
                if (x > 1)
                {
                    pageview += "&nbsp;&nbsp;<a class='pl' href='?page=1" + clinktail + "'>第1页</a> | ";
                    pageviewtop += "&nbsp;&nbsp;<a class='pl' href='?page=1" + clinktail + "'>第1页</a> | ";
                }
                else
                {
                    pageview += "&nbsp;&nbsp;<font color='#666666'> 第1页 </font> | ";
                    pageviewtop += "&nbsp;&nbsp;<font color='#666666'> 第1页 </font> | ";
                }

                if (x > 1)
                {
                    pageviewtop += " <a class='pl' href='?page=" + (x - 1) + "" + clinktail + "'>上1页</a> ";
                }
                else
                {
                    pageviewtop += " <font color='#666666'>上1页</font> ";
                }

                if (x > ((x - 1) / 10) * 10 && x > 10)
                {
                    pageview += "<a class='pl' href='?page=" + ((x - 1) / 10) * 10 + "" + clinktail + "' onclink='return false;'>上10页</a>";
                }

                //if (((x-1) / 10) * 10 + 10) >= n )
                if (((x - 1) / 10) * 10 + 10 >= n)
                {
                    endpage = n;
                }
                else
                {
                    endpage = ((x - 1) / 10) * 10 + 10;
                }

                for (i = ((x - 1) / 10) * 10 + 1; i <= endpage; ++i)
                {
                    if (i == x)
                    {
                        pageview += " <font color='#FF0000'><b>" + i + "</b></font>";
                    }
                    else
                    {
                        pageview += " <a class='pl' href='?page=" + i + "" + clinktail + "'>" + i + "</a>";
                    }
                }

                if (x < n)
                {
                    pageviewtop += " <a class='pl' href='?page=" + (x + 1) + "" + clinktail + "'>下1页</a> ";
                }
                else
                {
                    pageviewtop += " <font color='#666666'>下1页</font> ";
                }

                if (endpage != n)
                {
                    pageview += " <a class='pl' href='?page=" + (endpage + 1) + "" + clinktail + "' class='pl' onclink='return false;'>下10页</a> | ";
                }
                else
                {
                    pageview += " | ";
                }
                if (x < n)
                {
                    pageview += " <a class='pl' href='?page=" + n + "" + clinktail + "' class='pl'>第" + n + "页</a> ";
                    pageviewtop += " |  <a class='pl' href='?page=" + n + "" + clinktail + "' class='pl'>第" + n + "页</a> ";
                }
                else
                {
                    pageview += "<font color='#666666'> 第" + n + "页 </font>";
                    pageviewtop += " | <font color='#666666'> 第" + n + "页 </font>";
                }
                page_view.Text = pageview.ToString();
            }
            else
            {
                page_view.Text = "";
            }
        }

        //带第一页和最后一页
        public static string paging2(string para, int sumpage, int page, int count)
        {
            string result = string.Empty;
            if (page > sumpage)
            {
                page = 1;
            }
            StringBuilder sb = new StringBuilder();
            if (sumpage > 0)
            {
                if (sumpage != 1)
                {
                    //第一页
                    sb.Append(string.Format("<a href=\"?page={0}{1}\"><img src=\"images/first-icon.gif\" border=\"0\"/></a>&nbsp;&nbsp;", new object[] { 1, para }));
                    switch (page)
                    {
                        case 1:
                            //前一页图片
                            sb.Append(string.Format("<a>{0}</a>", new object[] { "<img src=\"images/left-icon.gif\" border=\"0\"/>" }));
                            sb.Append(string.Format("<a>上一页</a><a href=\"?page={0}{1}\">{2}</a> ", new object[] { page + 1, para, "下一页" }));
                            //后一页图片
                            sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a>", new object[] { page + 1, para, "<img src=\"images/right-icon.gif\" border=\"0\"/>" }));
                            break;
                        default:
                            if (sumpage == page)
                            {
                                //前一页图片
                                sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a>", new object[] { page - 1, para, "<img src=\"images/left-icon.gif\" border=\"0\"/>" }));
                                sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a><a>下一页</a> ", new object[] { page - 1, para, "上一页" }));
                                //后一页图片
                                sb.Append(string.Format("<a>{0}</a>", new object[] { "<img src=\"images/right-icon.gif\" />" }));
                            }
                            else
                            {
                                //前一页图片
                                sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a>", new object[] { page - 1, para, "<img src=\"images/left-icon.gif\" border=\"0\"/>" }));
                                sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a> <a href=\"?page={3}{4}\">{5}</a> ",
                                    new object[] { page - 1, para, "上一页", page + 1, para, "下一页" }));
                                //后一页图片
                                sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a>", new object[] { page + 1, para, "<img src=\"images/right-icon.gif\" border=\"0\"/>" }));
                            }
                            break;
                    }
                    //最后一页图片
                    sb.Append(string.Format("&nbsp;&nbsp;<a href=\"?page={0}{1}\"><img src=\"images/last-icon.gif\" border=\"0\"/></a>&nbsp;&nbsp;", new object[] { sumpage, para }));
                }
                sb.Append(string.Format("第{0}页/共{1}页 共{2}条", new object[] { page, sumpage, count }));
            }
            return sb.ToString();
        }

        public static string paging3(string url, string para, int sumpage, int page, int count)
        {
            string result = string.Empty;
            if (page > sumpage)
            {
                page = 1;
            }
            StringBuilder sb = new StringBuilder();
            if (sumpage > 0)
            {
                if (sumpage != 1)
                {
                    //第一页
                    sb.Append(string.Format("<a href=\"{2}?page={0}{1}\">首页</a>", new object[] { 1, para, url }));
                    switch (page)
                    {
                        case 1:
                            //前一页图片
                            // sb.Append(string.Format("<a>{0}</a>", new object[] { "<img src=\"images/left-icon.gif\" border=\"0\"/>" }));
                            sb.Append(string.Format("<a>上一页</a><a href=\"{3}?page={0}{1}\">{2}</a> ", new object[] { page + 1, para, "下一页", url }));
                            //后一页图片
                            // sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a>", new object[] { page + 1, para, "<img src=\"images/right-icon.gif\" border=\"0\"/>" }));
                            break;
                        default:
                            if (sumpage == page)
                            {
                                //前一页图片
                                //sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a>", new object[] { page - 1, para, "<img src=\"images/left-icon.gif\" border=\"0\"/>" }));
                                sb.Append(string.Format("<a href=\"{3}?page={0}{1}\">{2}</a><a>下一页</a> ", new object[] { page - 1, para, "上一页", url }));
                                //后一页图片
                                //sb.Append(string.Format("<a>{0}</a>", new object[] { "<img src=\"images/right-icon.gif\" />" }));
                            }
                            else
                            {
                                //前一页图片
                                //sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a>", new object[] { page - 1, para, "<img src=\"images/left-icon.gif\" border=\"0\"/>" }));
                                sb.Append(string.Format("<a href=\"{6}?page={0}{1}\">{2}</a> <a href=\"{6}?page={3}{4}\">{5}</a> ",
                                    new object[] { page - 1, para, "上一页", page + 1, para, "下一页", url }));
                                //后一页图片
                                //sb.Append(string.Format("<a href=\"?page={0}{1}\">{2}</a>", new object[] { page + 1, para, "<img src=\"images/right-icon.gif\" border=\"0\"/>" }));
                            }
                            break;
                    }
                    //最后一页图片
                    sb.Append(string.Format("<a href=\"{2}?page={0}{1}\">末页</a>&nbsp;&nbsp;", new object[] { sumpage, para, url }));
                }
                sb.Append(string.Format("第{0}页/共{1}页 共{2}条", new object[] { page, sumpage, count }));
            }
            return sb.ToString();
        }
        #endregion

        #region 日期格式判断
        /// <summary>
        /// 日期格式字符串判断
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(string str)
        {
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    DateTime.Parse(str);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 是否由特定字符组成
        public static bool isContainSameChar(string strInput)
        {
            string charInput = string.Empty;
            if (!string.IsNullOrEmpty(strInput))
            {
                charInput = strInput.Substring(0, 1);
            }
            return isContainSameChar(strInput, charInput, strInput.Length);
        }

        public static bool isContainSameChar(string strInput, string charInput, int lenInput)
        {
            if (string.IsNullOrEmpty(charInput))
            {
                return false;
            }
            else
            {
                Regex RegNumber = new Regex(string.Format("^([{0}])+$", charInput));
                //Regex RegNumber = new Regex(string.Format("^([{0}]{{1}})+$", charInput,lenInput));
                Match m = RegNumber.Match(strInput);
                return m.Success;
            }
        }
        #endregion

        #region 检查输入的参数是不是某些定义好的特殊字符：这个方法目前用于密码输入的安全检查
        /// <summary>
        /// 检查输入的参数是不是某些定义好的特殊字符：这个方法目前用于密码输入的安全检查
        /// </summary>
        public static bool isContainSpecChar(string strInput)
        {
            string[] list = new string[] { "123456", "654321" };
            bool result = new bool();
            for (int i = 0; i < list.Length; i++)
            {
                if (strInput == list[i])
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        #endregion
    }
}