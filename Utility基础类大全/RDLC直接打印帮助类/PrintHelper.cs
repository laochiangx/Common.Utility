using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Text;
using System.Data;
/// <summary>
/// 打印帮助类
/// </summary>
public class PrintHelper
{
    private int m_currentPageIndex;
    private IList<Stream> m_streams;

    /// <summary>
    /// 报表直接打印
    /// </summary>
    /// <param name="reportPath">报表文件路径</param>
    /// <param name="printerName">打印机名称</param>
    /// <param name="dt">DataTable</param>
    /// <param name="sourceName">rdlc的数据集名称</param>
    /// <param name="paraList">参数列表</param>
    public void Run(string reportPath, string printerName, DataTable dt, string sourceName, List<ReportParameter> paraList)
    {
        LocalReport report = new LocalReport();
        report.ReportPath = reportPath;
        report.DataSources.Add(new ReportDataSource(sourceName, dt));
        report.EnableExternalImages = true;
        report.SetParameters(paraList);
        Export(report);
        m_currentPageIndex = 0;
        Print(printerName);
    }

    private void Export(LocalReport report)
    {
        string deviceInfo =
            "<DeviceInfo>" +
            " <OutputFormat>EMF</OutputFormat>" +
            "</DeviceInfo>";
        Warning[] warnings;
        m_streams = new List<Stream>();
        try
        {
            report.Render("Image", deviceInfo, CreateStream, out warnings);
        }
        catch (Exception ex)
        {
            Exception innerEx = ex.InnerException;
            while (innerEx != null)
            {
                string errmessage = innerEx.Message;
                innerEx = innerEx.InnerException;

            }
        }

        foreach (Stream stream in m_streams)
        {
            stream.Position = 0;
        }
    }

    private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
    {
        Stream stream = new FileStream(name + DateTime.Now.Millisecond + "." + fileNameExtension, FileMode.Create);
        m_streams.Add(stream);
        return stream;
    }
    private void Print(string printerName)
    {
        if (m_streams == null || m_streams.Count == 0) return;
        PrintDocument printDoc = new PrintDocument();
        if (printerName.Length > 0)
        {
            printDoc.PrinterSettings.PrinterName = printerName;
        }
        foreach (PaperSize ps in printDoc.PrinterSettings.PaperSizes)
        {
            if (ps.PaperName == "A4")
            {
                printDoc.PrinterSettings.DefaultPageSettings.PaperSize = ps;
                printDoc.DefaultPageSettings.PaperSize = ps;
            }
        }
        if (!printDoc.PrinterSettings.IsValid)
        {
            string msg = string.Format("找不到打印机：{0}", printerName);
            LogUtil.Log(msg);
            return;
        }
        printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
        printDoc.Print();
    }

    private void PrintPage(object sender, PrintPageEventArgs ev)
    {
        Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);
        ev.Graphics.DrawImage(pageImage, 0, 0, 827, 1169);//像素
        m_currentPageIndex++;
        ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
    }
    //初始化报表信息
    private void SetReportInfo(string reportPath, string sourceName, DataTable dataSource, bool isFengPi)
    {
        if (!File.Exists(reportPath))
        {
            MessageBox.Show("报表文件:" + reportPath + " 不存在!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (dataSource == null || dataSource.Rows.Count == 0)
        {
            MessageBox.Show("没有找到案卷号为:" + txtArchiveNum.Text.Trim() + "的相关目录信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        pos = 1;
        LocalReport report1 = new LocalReport();
        //设置需要打印的报表的文件名称。 
        report1.ReportPath = reportPath;
        if (isFengPi)
        {
            //设置参数
            string archveTypeName = GetArchiveTypeName();
            ReportParameter archiveType = new ReportParameter("ArchiveType", archveTypeName);
            report1.SetParameters(archiveType);
        }
        //创建要打印的数据源
        ReportDataSource source = new ReportDataSource(sourceName, dataSource);
        report1.DataSources.Add(source);
        //刷新报表中的需要呈现的数据  
        report1.Refresh();
        pos = 2;
        m_streams = new List<Stream>();
        string deviceInfo = "<DeviceInfo>" +
            "  <OutputFormat>EMF</OutputFormat>" +
            "  <PageWidth>21cm</PageWidth>" +
            "  <PageHeight>29.7cm</PageHeight>" +
            "  <MarginTop>2.0066cm</MarginTop>" +
            "  <MarginLeft>2.0066cm</MarginLeft>" +
            "  <MarginRight>2.0066cm</MarginRight>" +
            "  <MarginBottom>2.0066cm</MarginBottom>" +
            "</DeviceInfo>";
        Warning[] warnings;
        //将报表的内容按照deviceInfo指定的格式输出到CreateStream函数提供的Stream中。
        report1.Render("Image", deviceInfo, CreateStream, out warnings);
    }

    //声明一个Stream对象的列表用来保存报表的输出数据 
    //LocalReport对象的Render方法会将报表按页输出为多个Stream对象。
 //   private List<Stream> m_streams;
    //用来提供Stream对象的函数，用于LocalReport对象的Render方法的第三个参数。
    private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)

    {
        pos = 3;
        //如果需要将报表输出的数据保存为文件，请使用FileStream对象。
        Stream stream = new MemoryStream();
        m_streams.Add(stream);
        return stream;
    }

    //用来记录当前打印到第几页了 
    private int m_currentPageIndex;

    #region 打印报表
    private void Print()
    {
        pos = 4;
        m_currentPageIndex = 0;
        if (m_streams == null || m_streams.Count == 0)
            return;
        //声明PrintDocument对象用于数据的打印 
        PrintDocument printDoc = new PrintDocument();
        //指定需要使用的打印机的名称，使用空字符串""来指定默认打印机  
        // printDoc.PrinterSettings.PrinterName = ""; 
        //判断指定的打印机是否可用 
        if (!printDoc.PrinterSettings.IsValid)
        {
            MessageBox.Show("没有找到打印机!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        pos = 5;
        printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
        //执行打印操作，Print方法将触发PrintPage事件。
        printDoc.Print();

        //释放资源
        foreach (Stream stream in m_streams)
        {
            stream.Dispose();
            stream.Close();
        }
        m_streams = null;
    }

    private void PrintPage(object sender, PrintPageEventArgs ev)
    {
        pos = 6;
        //Metafile对象用来保存EMF或WMF格式的图形，
        //我们在前面将报表的内容输出为EMF图形格式的数据流。
        m_streams[m_currentPageIndex].Position = 0;
        Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);
        //指定是否横向打印
        ev.PageSettings.Landscape = false;
        //这里的Graphics对象实际指向了打印机
        ev.Graphics.DrawImage(pageImage, ev.PageBounds);
        m_streams[m_currentPageIndex].Close();
        m_currentPageIndex++;
        //设置是否需要继续打印
        ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
    }
    #endregion

    //打印封皮
    private void btPrint_Click(object sender, EventArgs e)
    {
        string reportPath = Application.StartupPath + "\\Files\\ReportEnvelop.rdlc";
        SetReportInfo(reportPath, "DataSet1", GetDataSource(true), true);
        Print();

    }
}
