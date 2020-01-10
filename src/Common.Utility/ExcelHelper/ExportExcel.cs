 
using System;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;

namespace Common.Utility
{
    public class ExportExcel
    {

        protected void ExportData(string strContent, string FileName)
        {

            FileName = FileName + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "gb2312";
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //this.Page.EnableViewState = false; 
            // 添加头信息，为"文件下载/另存为"对话框指定默认文件名 
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ".xls");
            // 把文件流发送到客户端 
            HttpContext.Current.Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            HttpContext.Current.Response.Write(strContent);
            HttpContext.Current.Response.Write("</body></html>");
            // 停止页面的执行 
            //Response.End();
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="obj"></param>
        public void ExportData(GridView obj)
        {
            try
            {
                string style = "";
                if (obj.Rows.Count > 0)
                {
                    style = @"<style> .text { mso-number-format:\@; } </style> ";
                }
                else
                {
                    style = "no data.";
                }

                HttpContext.Current.Response.ClearContent();
                DateTime dt = DateTime.Now;
                string filename = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString();
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=ExportData" + filename + ".xls");
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                HttpContext.Current.Response.Charset = "GB2312";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                obj.RenderControl(htw);
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
            catch
            {
            }
        }

    }
}



/*实际应用的例子
 if (list.Count > 0)
                {
 *                  //1.实例化
                    xftwl.Infrastructure.ExportExcel ex = new Infrastructure.ExportExcel();

 *                  //2.指定数据源
                    GridView gv = new GridView();
                    gv.DataSource = list;
                    gv.DataBind();

                    //3.设置导出的excel某列格式
                    if(gv.Rows.Count>0)                     
                    {
                        for (int j = 0; j < gv.Rows.Count; j++)
                        {
                            //把每行第二列格式设为文本格式
                            gv.Rows[j].Cells[1].Attributes.Add("style", "mso-number-format:'\\@'");
                            //每行第三列格式：两位小数
                            gv.Rows[j].Cells[2].Attributes.Add("style", "mso-number-format:'0\\.00'");
                        }
                    }                   

                    ex.ExportData(gv);
                }
 
 
 */