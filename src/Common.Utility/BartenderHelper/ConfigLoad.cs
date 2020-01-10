using System;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;

namespace BartenderHelper
{
    /// <summary>
    /// 获取标签模板的完整路径
    /// </summary>
    public class ConfigLoad
    {
        /// <summary>
        /// 获取标签模板的完整路径
        /// </summary>
        /// <param name="sLabelName">文件名，带后缀，如果不传则默认为"BC_DEFAULT.btw"</param>
        /// <returns>返回标签的完整路径</returns>
        public static string GetLabelNameFull(string sLabelName)
        {
            //获取当前程序集的路径
            string sPathFolder = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            string sLabel = string.Empty;
            if (string.IsNullOrEmpty(sLabelName) && sLabelName.Substring(sLabelName.LastIndexOf(".") + 1) != "btw")
            {
                sLabel = sPathFolder + "\\" + "BC_DEFAULT.btw";
            }
            else
            {
                sLabel = (sPathFolder + "\\" + sLabelName);
            }
            if (string.IsNullOrEmpty(sLabel))
                throw new Exception("LabelName is null");
            if (!File.Exists(sLabel))
                throw new Exception("LabelName not exists : " + sLabel);
            return sLabel;
        }

        /// <summary>
        /// 获取默认打印机名称
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultPrinterName()
        {
            PrintDocument fPrintDoc = new PrintDocument();
            return fPrintDoc.PrinterSettings.PrinterName;
        }

        /// <summary>
        /// 获取本地打印机列表
        /// </summary>
        /// <returns></returns>
        public static string[] GetLocalPrinterList()
        {
            string[] ListPrinter = new string[PrinterSettings.InstalledPrinters.Count];
            //获取当前打印机
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                ListPrinter[i] = (PrinterSettings.InstalledPrinters[i].ToString());
            }
            return ListPrinter;
        }
    }
}
