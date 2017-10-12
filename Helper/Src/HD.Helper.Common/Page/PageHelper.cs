using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;

namespace HD.Helper.Common
{
    /// <summary>
    /// 网页帮助类
    /// </summary>
    public class PageHelper
    {
        #region 控件状态设置

        /// <summary>
        /// 锁定页面上的一些组件
        /// </summary>
        /// <param name="page"></param>
        /// <param name="obj">不需锁定的控件</param>
        public static void LockPage(Page page, object[] obj)
        {
            Control htmlForm = null;
            foreach (Control ctl in page.Controls)
            {
                if (ctl is HtmlForm)
                {
                    htmlForm = ctl;
                    break;
                }
            }
            //foreach (Control ctl in page.Controls[1].Controls)
            foreach (Control ctl in htmlForm.Controls)
            {
                if (IsContains(obj, ctl) == false)
                {
                    //锁定
                    LockControl(page, ctl);
                }
                else
                {
                    //解除锁定
                    UnLockControl(page, ctl);
                }
            }
        }

        /// <summary>
        /// 解除锁定页面上的一些组件
        /// </summary>
        /// <param name="page"></param>
        /// <param name="obj">继续保持锁定的控件</param>
        public static void UnLockPage(Page page, object[] obj)
        {
            Control htmlForm = null;
            foreach (Control ctl in page.Controls)
            {
                if (ctl is HtmlForm)
                {
                    htmlForm = ctl;
                    break;
                }
            }
            //foreach (Control ctl in page.Controls[1].Controls)
            foreach (Control ctl in htmlForm.Controls)
            {
                if (IsContains(obj, ctl) == false)
                {
                    //解除锁定
                    UnLockControl(page, ctl);
                }
                else
                {
                    //锁定
                    LockControl(page, ctl);
                }
            }
        }

        /// <summary>
        /// 禁用控件
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ctl"></param>
        private static void LockControl(Page page, Control ctl)
        {
            //WebControl
            if (ctl is Button || ctl is CheckBox || ctl is HyperLink || ctl is LinkButton
                || ctl is ListControl || ctl is TextBox)
            {
                ((WebControl)ctl).Enabled = false;

                #region 多行文本框不能禁用，应设为只读，不然滚动条不能使用

                if (ctl is TextBox)
                {
                    if (((TextBox)ctl).TextMode == TextBoxMode.MultiLine)
                    {
                        ((TextBox)ctl).Enabled = true;
                        ((TextBox)ctl).ReadOnly = true;
                    }
                }

                #endregion

                #region 时间控件禁用时不显示图片



                #endregion
            }

            //HtmlControl
            if (ctl is HtmlInputFile)
            {
                ((HtmlInputFile)ctl).Disabled = true;
            }
        }

        /// <summary>
        /// 开放控件
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ctl"></param>
        private static void UnLockControl(Page page, Control ctl)
        {
            //WebControl
            if (ctl is Button || ctl is CheckBox || ctl is HyperLink || ctl is LinkButton
                || ctl is ListControl || ctl is TextBox)
            {
                ((WebControl)ctl).Enabled = true;

                //文本框去掉只读属性
                if (ctl is TextBox)
                {
                    ((TextBox)ctl).ReadOnly = false;
                }

                ////时间输入文本框不禁用时显示按钮
                //if (ctl is WebDateTimeEdit)
                //{
                //    ((WebDateTimeEdit)ctl).SpinButtons.Display = ButtonDisplay.OnRight;
                //}

                ////时间选择文本框不禁用时显示按钮
                //if (ctl is WebDateChooser)
                //{
                //    page.ClientScript.RegisterStartupScript(typeof(string), "Display" + ctl.ClientID + "Image", "<script language=javascript>" +
                //        "document.getElementById('" + ctl.ClientID + "_img" + "').style.display='';</script>");
                //}
            }

            //HtmlControl
            if (ctl is HtmlInputFile)
            {
                ((HtmlInputFile)ctl).Disabled = false;
            }
        }

        /// <summary>
        /// 数组中是否包含当前控件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ctl"></param>
        /// <returns></returns>
        private static bool IsContains(object[] obj, Control ctl)
        {
            foreach (Control c in obj)
            {
                if (c.ID == ctl.ID)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 页面处理其它辅助方法

        /// <summary>
        /// 读取QueryString值
        /// </summary>
        /// <param name="queryStringName">QueryString名称</param>
        /// <returns>QueryString值</returns>
        public static string GetQueryString(string queryStringName)
        {
            if ((HttpContext.Current.Request.QueryString[queryStringName] != null) &&
                (HttpContext.Current.Request.QueryString[queryStringName] != "undefined"))
            {
                return HttpContext.Current.Request.QueryString[queryStringName].Trim();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 页面跳转
        /// </summary>
        /// <param name="url">URL地址</param>
        public void Redirect(string url)
        {
            Page page = GetCurrentPage();
            page.Response.Redirect(url);
        }

        /// <summary>
        /// 获取当前请求页面相对于根目录的层级
        /// </summary>
        /// <returns></returns>
        public static string GetRelativeLevel()
        {
            string ApplicationPath = HttpContext.Current.Request.ApplicationPath;
            if (ApplicationPath.Trim() == "/")
            {
                ApplicationPath = "";
            }

            int i = ApplicationPath == "" ? 1 : 2;
            return "";//Nandasoft.Helper.NDHelperString.Repeat("../", Nandasoft.Helper.NDHelperString.RepeatTime(HttpContext.Current.Request.Path, "/") - i);
        }

        /// <summary>
        /// 返回客户端浏览器版本
        /// 如果是IE类型，返回版本数字
        /// 如果不是IE类型，返回-1
        /// </summary>
        /// <returns>一位数字版本号</returns>
        public static int GetClientBrowserVersion()
        {
            string USER_AGENT = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];

            if (USER_AGENT.IndexOf("MSIE") < 0) return -1;

            string version = USER_AGENT.Substring(USER_AGENT.IndexOf("MSIE") + 5, 1);
            int result;
            if (!int.TryParse(version, out result)) return -1;

            return Convert.ToInt32(version);
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

        /// <summary>
        /// 得到当前页对象实例
        /// </summary>
        /// <returns></returns>
        public static Page GetCurrentPage()
        {
            return (Page)HttpContext.Current.Handler;
        }

        /// <summary>
        /// 从System.Web.HttpRequest的Url中获取所调用的页面名称
        /// </summary>
        /// <returns>页面名称</returns>
        public static string GetPageName()
        {
            int start = 0;
            int end = 0;
            string Url = HttpContext.Current.Request.RawUrl;
            start = Url.LastIndexOf("/") + 1;
            end = Url.IndexOf("?");
            if (end <= 0)
            {
                return Url.Substring(start, Url.Length - start);
            }
            else
            {
                return Url.Substring(start, end - start);
            }
        }
        #endregion
    }
}
