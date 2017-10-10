using System;
using System.Data;
using System.Text;

namespace DotNet.Utilities
{
    /// <summary>
    /// 利用OWC11进行作统计图的封装类。
    /// 李天平 2005-8-31
    /// </summary>
    public class OWCChart11
    {

        #region 属性
        private string _phaysicalimagepath;
        private string _title;
        private string _seriesname;
        private int _picwidth;
        private int _pichight;
        private DataTable _datasource;
        private string strCategory;
        private string strValue;

        public string PhaysicalImagePath
        {
            set { _phaysicalimagepath = value; }
            get { return _phaysicalimagepath; }
        }
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        public string SeriesName
        {
            set { _seriesname = value; }
            get { return _seriesname; }
        }

        public int PicWidth
        {
            set { _picwidth = value; }
            get { return _picwidth; }
        }

        public int PicHight
        {
            set { _pichight = value; }
            get { return _pichight; }
        }
        public DataTable DataSource
        {
            set
            {
                _datasource = value;
                strCategory = GetColumnsStr(_datasource);
                strValue = GetValueStr(_datasource);
            }
            get { return _datasource; }
        }

        private string GetColumnsStr(System.Data.DataTable dt)
        {
            StringBuilder strList = new StringBuilder();
            foreach (DataRow r in dt.Rows)
            {
                strList.Append(r[0].ToString() + '\t');
            }
            return strList.ToString();
        }
        private string GetValueStr(DataTable dt)
        {
            StringBuilder strList = new StringBuilder();
            foreach (DataRow r in dt.Rows)
            {
                strList.Append(r[1].ToString() + '\t');
            }
            return strList.ToString();
        }

        #endregion


        public OWCChart11()
        {
        }
        public OWCChart11(string PhaysicalImagePath, string Title, string SeriesName)
        {
            _phaysicalimagepath = PhaysicalImagePath;
            _title = Title;
            _seriesname = SeriesName;
        }


        /// <summary>
        /// 柱形图
        /// </summary>
        /// <returns></returns>
        public string CreateColumn()
        {
            Microsoft.Office.Interop.Owc11.ChartSpace objCSpace = new Microsoft.Office.Interop.Owc11.ChartSpaceClass();//创建ChartSpace对象来放置图表			
            Microsoft.Office.Interop.Owc11.ChChart objChart = objCSpace.Charts.Add(0);//在ChartSpace对象中添加图表，Add方法返回chart对象

            //指定图表的类型。类型由OWC.ChartChartTypeEnum枚举值得到//Microsoft.Office.Interop.OWC.ChartChartTypeEnum
            objChart.Type = Microsoft.Office.Interop.Owc11.ChartChartTypeEnum.chChartTypeColumnClustered;

            //指定图表是否需要图例
            objChart.HasLegend = true;

            //标题
            objChart.HasTitle = true;
            objChart.Title.Caption = _title;
            //			objChart.Title.Font.Bold=true;
            //			objChart.Title.Font.Color="blue";


            #region 样式设置

            //			//旋转
            //			objChart.Rotation  = 360;//表示指定三维图表的旋转角度
            //			objChart.Inclination = 10;//表示指定三维图表的视图斜率。有效范围为 -90 到 90

            //背景颜色
            //			objChart.PlotArea.Interior.Color = "red";

            //底座颜色
            //			objChart.PlotArea.Floor.Interior.Color = "green";
            // 
            //			objChart.Overlap = 50;//单个类别中标志之间的重叠量

            #endregion

            //x,y轴的图示说明
            objChart.Axes[0].HasTitle = true;
            objChart.Axes[0].Title.Caption = "X ： 类别";
            objChart.Axes[1].HasTitle = true;
            objChart.Axes[1].Title.Caption = "Y ： 数量";


            //添加一个series
            Microsoft.Office.Interop.Owc11.ChSeries ThisChSeries = objChart.SeriesCollection.Add(0);


            //给定series的名字
            ThisChSeries.SetData(Microsoft.Office.Interop.Owc11.ChartDimensionsEnum.chDimSeriesNames,
                Microsoft.Office.Interop.Owc11.ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(), SeriesName);
            //给定分类
            ThisChSeries.SetData(Microsoft.Office.Interop.Owc11.ChartDimensionsEnum.chDimCategories,
                Microsoft.Office.Interop.Owc11.ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(), strCategory);
            //给定值
            ThisChSeries.SetData(Microsoft.Office.Interop.Owc11.ChartDimensionsEnum.chDimValues,
                Microsoft.Office.Interop.Owc11.ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(), strValue);

            Microsoft.Office.Interop.Owc11.ChDataLabels dl = objChart.SeriesCollection[0].DataLabelsCollection.Add();
            dl.HasValue = true;
            //			dl.Position=Microsoft.Office.Interop.Owc11.ChartDataLabelPositionEnum.chLabelPositionOutsideEnd;


            string filename = DateTime.Now.ToString("yyyyMMddHHmmssff") + ".gif";
            string strAbsolutePath = _phaysicalimagepath + "\\" + filename;
            objCSpace.ExportPicture(strAbsolutePath, "GIF", _picwidth, _pichight);//输出成GIF文件.

            return filename;

        }


        /// <summary>
        /// 饼图
        /// </summary>
        /// <returns></returns>
        public string CreatePie()
        {
            Microsoft.Office.Interop.Owc11.ChartSpace objCSpace = new Microsoft.Office.Interop.Owc11.ChartSpaceClass();//创建ChartSpace对象来放置图表			
            Microsoft.Office.Interop.Owc11.ChChart objChart = objCSpace.Charts.Add(0);//在ChartSpace对象中添加图表，Add方法返回chart对象


            //指定图表的类型
            objChart.Type = Microsoft.Office.Interop.Owc11.ChartChartTypeEnum.chChartTypePie;

            //指定图表是否需要图例
            objChart.HasLegend = true;

            //标题
            objChart.HasTitle = true;
            objChart.Title.Caption = _title;


            //添加一个series
            Microsoft.Office.Interop.Owc11.ChSeries ThisChSeries = objChart.SeriesCollection.Add(0);

            //给定series的名字
            ThisChSeries.SetData(Microsoft.Office.Interop.Owc11.ChartDimensionsEnum.chDimSeriesNames,
                Microsoft.Office.Interop.Owc11.ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(), SeriesName);
            //给定分类
            ThisChSeries.SetData(Microsoft.Office.Interop.Owc11.ChartDimensionsEnum.chDimCategories,
                Microsoft.Office.Interop.Owc11.ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(), strCategory);
            //给定值
            ThisChSeries.SetData(Microsoft.Office.Interop.Owc11.ChartDimensionsEnum.chDimValues,
                Microsoft.Office.Interop.Owc11.ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(), strValue);


            //表示系列或趋势线上的单个数据标志
            Microsoft.Office.Interop.Owc11.ChDataLabels dl = objChart.SeriesCollection[0].DataLabelsCollection.Add();
            dl.HasValue = true;
            dl.HasPercentage = true;
            //图表绘图区的图例放置在右侧。
            //			dl.Position=Microsoft.Office.Interop.Owc11.ChartDataLabelPositionEnum.chLabelPositionRight;

            string filename = DateTime.Now.Ticks.ToString() + ".gif";
            string strAbsolutePath = _phaysicalimagepath + "\\" + filename;
            objCSpace.ExportPicture(strAbsolutePath, "GIF", _picwidth, _pichight);//输出成GIF文件.

            return filename;
        }

        /// <summary>
        /// 条形图
        /// </summary>
        /// <returns></returns>
        public string CreateBar()
        {
            Microsoft.Office.Interop.Owc11.ChartSpace objCSpace = new Microsoft.Office.Interop.Owc11.ChartSpaceClass();//创建ChartSpace对象来放置图表			
            Microsoft.Office.Interop.Owc11.ChChart objChart = objCSpace.Charts.Add(0);//在ChartSpace对象中添加图表，Add方法返回chart对象

            //指定图表的类型。类型由OWC.ChartChartTypeEnum枚举值得到//Microsoft.Office.Interop.OWC.ChartChartTypeEnum
            objChart.Type = Microsoft.Office.Interop.Owc11.ChartChartTypeEnum.chChartTypeBarClustered;

            //指定图表是否需要图例
            objChart.HasLegend = true;

            //标题
            objChart.HasTitle = true;
            objChart.Title.Caption = _title;
            //			objChart.Title.Font.Bold=true;
            //			objChart.Title.Font.Color="blue";


            #region 样式设置

            //			//旋转
            //			objChart.Rotation  = 360;//表示指定三维图表的旋转角度
            //			objChart.Inclination = 10;//表示指定三维图表的视图斜率。有效范围为 -90 到 90

            //背景颜色
            //			objChart.PlotArea.Interior.Color = "red";

            //底座颜色
            //			objChart.PlotArea.Floor.Interior.Color = "green";
            // 
            //			objChart.Overlap = 50;//单个类别中标志之间的重叠量

            #endregion

            //x,y轴的图示说明
            objChart.Axes[0].HasTitle = true;
            objChart.Axes[0].Title.Caption = "X ： 类别";
            objChart.Axes[1].HasTitle = true;
            objChart.Axes[1].Title.Caption = "Y ： 数量";


            //添加一个series
            Microsoft.Office.Interop.Owc11.ChSeries ThisChSeries = objChart.SeriesCollection.Add(0);


            //给定series的名字
            ThisChSeries.SetData(Microsoft.Office.Interop.Owc11.ChartDimensionsEnum.chDimSeriesNames,
                Microsoft.Office.Interop.Owc11.ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(), SeriesName);
            //给定分类
            ThisChSeries.SetData(Microsoft.Office.Interop.Owc11.ChartDimensionsEnum.chDimCategories,
                Microsoft.Office.Interop.Owc11.ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(), strCategory);
            //给定值
            ThisChSeries.SetData(Microsoft.Office.Interop.Owc11.ChartDimensionsEnum.chDimValues,
                Microsoft.Office.Interop.Owc11.ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(), strValue);

            Microsoft.Office.Interop.Owc11.ChDataLabels dl = objChart.SeriesCollection[0].DataLabelsCollection.Add();
            dl.HasValue = true;
            //			dl.Position=Microsoft.Office.Interop.Owc11.ChartDataLabelPositionEnum.chLabelPositionOutsideEnd;


            string filename = DateTime.Now.ToString("yyyyMMddHHmmssff") + ".gif";
            string strAbsolutePath = _phaysicalimagepath + "\\" + filename;
            objCSpace.ExportPicture(strAbsolutePath, "GIF", _picwidth, _pichight);//输出成GIF文件.

            return filename;

        }

    }
}
