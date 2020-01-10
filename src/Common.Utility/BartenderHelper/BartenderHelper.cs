using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using Seagull.BarTender.Print;


namespace BartenderHelper
{
    /// <summary>
    /// bartender打印类,自带日志记录
    ///  * 注意：必须选择x86模式编译
    ///  * 使用此程序步骤如下：
    ///  * 1、新建类，默认构造函数会自动启动打印引擎；
    ///  * 2、指定标签模板和打印机，为空时获取默认值，默认标签=当前路径\BC_DEFAULT.btw，默认打印机=系统默认打印机
    ///  * 3、调用PrintStart。
    ///  * 4、设置标签：指定打印数量\打印内容，总打印次数为iQtyLabel*iQtyLot
    ///         iQtyLabel ： 标签打印时，一个标签连续重复打印的次数
    ///         iQtyQty ： 标签打印时，整个批次重复打印的次数
    ///  *      lstSubStringName ： 标签模板中的变量集合，至少1个
    ///         lstSubStringValue ： 标签中的值，外层List表示不同标签的数量，内层List表示每个标签中各个SubStringName对应的值
    ///  * 5、调用PrintEnd，关闭标签模板。
    ///  * 6、关闭engine
    ///  * 使用建议：需要打印的程序加载时，同时新建该类，以减少engine启动的时间。关闭整个程序时再关闭engine。
    /// </summary>
    public class BartenderHelper
    {
        #region 构造方法
        /// <summary>
        /// 构造函数
        /// </summary>
        public BartenderHelper()
        {

        }
        #endregion

        #region 变量定义
        /// <summary>
        /// 打印引擎
        /// </summary>
        private Engine btEngine = new Engine(true);
        /// <summary>
        /// 标签文档
        /// </summary>
        private LabelFormatDocument btFormatDoc { get; set; }
        /// <summary>
        /// 标签模板路径
        /// </summary>
        public string sLabelNameFull { get; set; }
        /// <summary>
        /// 标签模板中的变量集合
        /// </summary>
        public List<string> lstSubStringName { get; set; }
        /// <summary>
        /// 需要打印的标签中的值，其中子项List<string/>需与lstSubStringName中的名称一一对应，否则数据会错位或报错
        /// </summary>
        public List<List<string>> lstSubStringValue { get; set; }
        /// <summary>
        /// 打印机名称
        /// </summary>
        public string sPrinterName { get; set; }
        /// <summary>
        /// 单个标签内容连续打印次数
        /// </summary>
        public int iQtyLabel { get; set; }
        /// <summary>
        /// 整体重复次数
        /// </summary>
        public int iQtyLot { get; set; }


        #endregion

        #region 打印方法

        /// <summary>
        /// 打印开始，公用程序，启动打印引擎,打开标签模板，指定打印机
        /// </summary>
        /// <param name="sLabel">标签名称，为空时获取默认值=当前路径\BC_DEFAULT.btw</param>
        /// <param name="sPrinter">打印机名称，为空时获取系统默认打印机</param>
        public void PrintStart(string sLabel, string sPrinter)
        {
            //启动打印引擎
            btEngine.Start();
            Log.Info("-- Print Engine Start OK --");
            //获取默认标签完整路径
            sLabelNameFull = ConfigLoad.GetLabelNameFull(sLabel);
            Log.Info("-- Default label path : " + sLabelNameFull + " --");
            //指定打印机名称
            sPrinterName = string.IsNullOrEmpty(sPrinter) ? ConfigLoad.GetDefaultPrinterName() : sPrinter;
            Log.Info("-- Default Printer : " + sPrinterName + " --");
            //加载标签模板，指定打印机
            btFormatDoc = btEngine.Documents.Open(sLabelNameFull, sPrinterName);
            Log.Info("-- FormatDoc opened --");
        }

        /// <summary>
        /// 打印结束，公用程序，关闭标签文档/停止打印引擎
        /// </summary>
        public void PrintEnd()
        {
            //关闭标签模板
            btFormatDoc.Close(SaveOptions.DoNotSaveChanges);
            Log.Info("--FormatDocument closed--");
            //停止打印引擎
            btEngine.Stop();
            Log.Info("--Print Engine Stop--");
        }

        /// <summary>
        /// 打印标签
        /// </summary>
        public void PrintLabel()
        {
            //设置参数
            //更改单个标签打印数量，默认为1
            try
            {
                iQtyLabel = iQtyLabel > 0 ? iQtyLabel : 1;
            }
            catch (Exception)
            {
                iQtyLabel = 1;
            }
            btFormatDoc.PrintSetup.IdenticalCopiesOfLabel = iQtyLabel;
            Log.Info("-- Label repeat Qty = " + iQtyLabel + "; --");

            //更改打印批次数量，默认为1
            try
            {
                iQtyLot = iQtyLot > 0 ? iQtyLot : 1;
            }
            catch (Exception)
            {
                iQtyLot = 1;
            }
            Log.Info("-- Lot repeat qty = " + iQtyLot + "; --");

            for (int iLot = 0; iLot < iQtyLot; iLot++)
            {
                //有标签内容时的打印
                if (lstSubStringName != null && lstSubStringValue != null)
                {
                    //更改标签内容并打印
                    for (int iValue = 0; iValue < lstSubStringValue.Count; iValue++)
                    {
                        string sLabelValue = "[";
                        for (int iName = 0; iName < lstSubStringName.Count; iName++)
                        {
                            btFormatDoc.SubStrings[lstSubStringName[iName]].Value = lstSubStringValue[iValue][iName];
                            sLabelValue += lstSubStringName[iName] + "=" + lstSubStringValue[iValue][iName] + ";";
                        }
                        sLabelValue += "]";
                        if (iLot == 0)
                        {
                            Log.Info("-- Label " + iValue + ":" + sLabelValue + " --");
                            Log.Info("-- PrintJob start : PrintJob" + iValue + " --");
                        }
                        btFormatDoc.Print("PrintJob" + iValue);
                        if (iLot == 0)
                        {
                            Log.Info("-- PrintJob finish : PrintJob" + iValue + " --");
                        }
                    }
                }
                //没有指定标签内容时，直接打印标签模板文件
                else
                {
                    if (iLot == 0)
                    {
                        Log.Info("-- Print default .btw file directly --");
                        Log.Info("-- PrintJob start : PrintJob1 --");
                    }
                    btFormatDoc.Print("PrintJob1");
                    if (iLot == 0) Log.Info("-- PrintJob finish : PrintJob1 --");
                }
            }
        }

        #endregion
    }
}
