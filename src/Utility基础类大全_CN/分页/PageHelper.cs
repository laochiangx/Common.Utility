using System;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace Utilities
{
    public class PageValidate
    {
        #region 分页
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
    }
}