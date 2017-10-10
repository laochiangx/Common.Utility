using System;
using System.Text;
using System.Data;
using OWCChart;

namespace DotNet.Utilities
{
    /// <summary>
    /// Assistant 的摘要说明。
    /// </summary>
    public sealed class Assistant
    {


        #region 创建显示图像的标签

        /// <summary>
        /// 创建显示图像的标签(flash加点击)
        /// </summary>
        public static string CreateTag(string ADID, string filename, string desc, string FileType, string LinkURL, int Width, int High)
        {
            StringBuilder TagStr = new StringBuilder();
            switch (FileType)
            {
                case "image/gif":
                case "image/bmp":
                case "image/pjpeg":
                    {
                        if ((LinkURL.Trim() != "") && (LinkURL.Trim() != "http://"))//非空
                        {
                            TagStr.Append("<a href=\"");
                            TagStr.Append(ConfigHelper.GetConfigString("URL") + "/FormAdHit.aspx?ADID=" + ADID);
                            TagStr.Append("&LinkURL=" + LinkURL.Replace("&", "$$$"));
                            TagStr.Append("\"");
                            TagStr.Append(" target=\"_blank\">");
                        }
                        TagStr.Append(" <IMG alt=\"" + desc + "\"");
                        TagStr.Append(" src=\"" + filename + "\"");
                        TagStr.Append(" width=\"" + Width + "\" height=\"" + High + "\" ");
                        TagStr.Append(" border=\"0\">");
                        if ((LinkURL.Trim() != "") && (LinkURL.Trim() != "http://"))
                        {
                            TagStr.Append("</a>");
                        }
                        break;
                    }

                case "application/x-shockwave-flash":
                    {
                        //					TagStr.Append("<object ");
                        ////					TagStr.Append(" width="+Width+" height="+High+" ");
                        //					TagStr.Append("  classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\"  ");
                        //					TagStr.Append(" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0\"> ");
                        ////					TagStr.Append(" <param name=\"movie\" value=\""+filename+"?clickthru=");
                        ////					TagStr.Append("FormAdHit.aspx?ADID="+ADID);
                        ////					TagStr.Append("_LinkURL="+LinkURL);
                        ////					TagStr.Append("\"> ");					
                        //					TagStr.Append(" <param name=\"wmode\" value=\"opaque\"> ");
                        //					TagStr.Append(" <param name=\"quality\" value=\"autohigh\"> ");
                        //					
                        //					TagStr.Append(" <embed  ");
                        //					TagStr.Append(" width="+Width+" height="+High+"  ");
                        //					TagStr.Append(" src=\""+filename+"?clickthru=");
                        //					TagStr.Append("FormAdHit.aspx?ADID="+ADID);
                        //					if((LinkURL.Trim()!="")&&(LinkURL.Trim()!="http://"))
                        //					{
                        //						TagStr.Append("_LinkURL="+LinkURL);
                        //					}
                        //					TagStr.Append("\"  ");	
                        //					TagStr.Append(" quality=\"high\" wmode=\"opaque\" type=\"application/x-shockwave-flash\"  ");
                        //					TagStr.Append(" plugspace=\"http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash\"> ");
                        //					TagStr.Append(" </embed></object> ");

                        TagStr.Append(" <embed ");
                        TagStr.Append(" src=\"" + filename + "\" ");
                        //					TagStr.Append(" src=\""+filename+"?clickthru=");
                        //					TagStr.Append("FormAdHit.aspx?ADID="+ADID);
                        //					if((LinkURL.Trim()!="")&&(LinkURL.Trim()!="http://"))
                        //					{
                        //						TagStr.Append("_LinkURL="+LinkURL);
                        //					}
                        //					TagStr.Append("\"  ");	
                        TagStr.Append(" width=" + Width + " height=" + High + "  ");
                        TagStr.Append(" quality=\"high\" ");
                        TagStr.Append(" ></embed>");

                    }

                    break;

                case "video/x-ms-wmv":
                case "video/mpeg":
                case "video/x-ms-asf":
                case "video/avi":
                case "audio/mpeg":
                case "audio/mid":
                case "audio/wav":
                case "audio/x-ms-wma":
                    TagStr.Append("<embed");
                    TagStr.Append(" src=\"" + filename + "\" border=\"0\" ");
                    TagStr.Append(" width=\"" + Width + "\" height=\"" + High + "\"");
                    TagStr.Append(" autoStart=\"1\" playCount=\"0\" enableContextMenu=\"0\"");
                    TagStr.Append(" type=\"application/x-mplayer2\"></embed>");
                    break;

                default:
                    //TagStr.Append("不允许该格式文件显示！");
                    break;
            }

            return TagStr.ToString();

        }


        /// <summary>
        /// 创建显示图像的标签(flash无点击)
        /// </summary>		
        public static string CreateTag2(string ADID, string filename, string desc, string FileType, string LinkURL, int Width, int High)
        {
            StringBuilder TagStr = new StringBuilder();
            switch (FileType)
            {
                case "image/gif":
                case "image/bmp":
                case "image/pjpeg":
                    {
                        TagStr.Append("<a href=\"");
                        TagStr.Append(ConfigHelper.GetConfigString("URL") + "\\FormAdHit.aspx?ADID=" + ADID);
                        TagStr.Append("&LinkURL=" + LinkURL);
                        TagStr.Append("\"");
                        TagStr.Append(" target=\"_blank\">");
                        TagStr.Append(" <IMG alt=\"" + desc + "\"");
                        TagStr.Append(" src=\"" + filename + "\"");
                        TagStr.Append(" width=\"" + Width + "\" height=\"" + High + "\" ");
                        TagStr.Append(" border=\"0\">");
                        TagStr.Append("</a>");
                        break;
                    }

                case "application/x-shockwave-flash":
                    {
                        //					TagStr.Append("<a href=\"");
                        //					TagStr.Append(LinkURL);
                        //					TagStr.Append("FormAdHit.aspx?ADID="+ADID);
                        //					TagStr.Append("&LinkURL="+LinkURL);
                        //					TagStr.Append("\"");
                        //					TagStr.Append(" target=\"_blank\">");

                        TagStr.Append(" <embed src=\"" + filename + "\" ");
                        TagStr.Append(" quality=\"high\" bgcolor=\"#f5f5f5\" ");
                        TagStr.Append(" ></embed>");

                        //					TagStr.Append("</a>");
                    }

                    break;

                case "video/x-ms-wmv":
                case "video/mpeg":
                case "video/x-ms-asf":
                case "video/avi":
                case "audio/mpeg":
                case "audio/mid":
                case "audio/wav":
                case "audio/x-ms-wma":

                    //					TagStr.Append("<a href=\"");
                    //					TagStr.Append(LinkURL);
                    //					TagStr.Append("FormAdHit.aspx?ADID="+ADID);
                    //					TagStr.Append("&LinkURL="+LinkURL);
                    //					TagStr.Append("\"");
                    //					TagStr.Append(" target=\"_blank\">");
                    TagStr.Append("<embed");
                    TagStr.Append(" src=\"" + filename + "\" border=\"0\" ");
                    TagStr.Append(" width=\"" + Width + "\" height=\"" + High + "\"");
                    TagStr.Append(" autoStart=\"1\" playCount=\"0\" enableContextMenu=\"0\"");
                    TagStr.Append(" type=\"application/x-mplayer2\"></embed>");
                    //					TagStr.Append("</a>");


                    break;

                default:
                    //					TagStr.Append("不允许该格式文件显示！");
                    break;
            }

            return TagStr.ToString();

        }


        /// <summary>
        /// 创建显示图像的标签(重载)，无宽高限制，(flash加点击)
        /// </summary>
        public static string CreateTag(string ADID, string filename, string desc, string FileType, string LinkURL)
        {
            StringBuilder TagStr = new StringBuilder();
            switch (FileType)
            {
                case "image/gif":
                case "image/bmp":
                case "image/pjpeg":
                    {
                        TagStr.Append("<a href=\"");
                        TagStr.Append(ConfigHelper.GetConfigString("URL") + "\\FormAdHit.aspx?ADID=" + ADID);
                        TagStr.Append("&LinkURL=" + LinkURL);
                        TagStr.Append("\"");
                        TagStr.Append(" target=\"_blank\">");
                        TagStr.Append(" <IMG alt=\"" + desc + "\"");
                        TagStr.Append(" src=\"" + filename + "\"");
                        //					TagStr.Append(" width=\""+Width+"\" height=\""+High+"\" ");
                        TagStr.Append(" border=\"0\">");
                        TagStr.Append("</a>");
                        break;
                    }

                case "application/x-shockwave-flash":
                    {
                        TagStr.Append("<object ");
                        //					TagStr.Append(" width="+Width+" height="+High+" ");
                        TagStr.Append("  classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\"  ");
                        TagStr.Append(" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0\"> ");
                        //					TagStr.Append(" <param name=\"movie\" value=\""+filename+"?clickthru=");
                        //					TagStr.Append("FormAdHit.aspx?ADID="+ADID);
                        //					TagStr.Append("_LinkURL="+LinkURL);
                        //					TagStr.Append("\"> ");					
                        TagStr.Append(" <param name=\"wmode\" value=\"opaque\"> ");
                        TagStr.Append(" <param name=\"quality\" value=\"autohigh\"> ");
                        TagStr.Append(" <embed  ");
                        //					TagStr.Append(" width="+Width+" height="+High+"  ");
                        TagStr.Append(" src=\"" + filename + "?clickthru=");
                        TagStr.Append(ConfigHelper.GetConfigString("URL") + "\\FormAdHit.aspx?ADID=" + ADID);
                        TagStr.Append("_LinkURL=" + LinkURL);
                        TagStr.Append("\"  ");
                        TagStr.Append(" quality=\"autohigh\" wmode=\"opaque\" type=\"application/x-shockwave-flash\"  ");
                        TagStr.Append(" plugspace=\"http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash\"> ");
                        TagStr.Append(" </embed></object> ");
                    }

                    break;

                case "video/x-ms-wmv":
                case "video/mpeg":
                case "video/x-ms-asf":
                case "video/avi":
                case "audio/mpeg":
                case "audio/mid":
                case "audio/wav":
                case "audio/x-ms-wma":
                    TagStr.Append("<embed");
                    TagStr.Append(" src=\"" + filename + "\" border=\"0\" ");
                    //					TagStr.Append(" width=\""+Width+"\" height=\""+High+"\"");	
                    TagStr.Append(" autoStart=\"1\" playCount=\"0\" enableContextMenu=\"0\"");
                    TagStr.Append(" type=\"application/x-mplayer2\"></embed>");


                    break;

                default:
                    break;
            }

            return TagStr.ToString();

        }


        /// <summary>
        /// 创建显示图像的标签(重载)，无宽高限制，(flash无点击)
        /// </summary>
        public static string CreateTag2(string ADID, string filename, string desc, string FileType, string LinkURL)
        {
            StringBuilder TagStr = new StringBuilder();
            switch (FileType)
            {
                case "image/gif":
                case "image/bmp":
                case "image/pjpeg":
                    {
                        TagStr.Append("<a href=\"");
                        TagStr.Append("FormAdHit.aspx?ADID=" + ADID);
                        TagStr.Append("&LinkURL=" + LinkURL);
                        TagStr.Append("\"");
                        TagStr.Append(" target=\"_blank\">");
                        TagStr.Append(" <IMG alt=\"" + desc + "\"");
                        TagStr.Append(" src=\"" + filename + "\"");
                        //					TagStr.Append(" width=\""+Width+"\" height=\""+High+"\" ");
                        TagStr.Append(" border=\"0\">");
                        TagStr.Append("</a>");
                        break;
                    }

                case "application/x-shockwave-flash":
                    {
                        TagStr.Append(" <embed src=\"" + filename + "\" ");
                        TagStr.Append(" quality=\"high\" bgcolor=\"#f5f5f5\" ");
                        TagStr.Append(" ></embed>");
                    }

                    break;

                case "video/x-ms-wmv":
                case "video/mpeg":
                case "video/x-ms-asf":
                case "video/avi":
                case "audio/mpeg":
                case "audio/mid":
                case "audio/wav":
                case "audio/x-ms-wma":
                    TagStr.Append("<embed");
                    TagStr.Append(" src=\"" + filename + "\" border=\"0\" ");
                    //					TagStr.Append(" width=\""+Width+"\" height=\""+High+"\"");	
                    TagStr.Append(" autoStart=\"1\" playCount=\"0\" enableContextMenu=\"0\"");
                    TagStr.Append(" type=\"application/x-mplayer2\"></embed>");


                    break;

                default:
                    break;
            }

            return TagStr.ToString();

        }


        #region
        /// <summary>
        /// 创建显示图像的标签
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="desc"></param>
        /// <param name="FileType"></param>
        /// <param name="LinkURL"></param>
        /// <param name="Width"></param>
        /// <param name="High"></param>
        /// <returns></returns>
        public static string CreateTagOld(string filename, string desc, string FileType, string LinkURL, int Width, int High)
        {
            StringBuilder TagStr = new StringBuilder();
            switch (FileType)
            {
                case "image/gif":
                case "image/bmp":
                case "image/pjpeg":
                    {
                        TagStr.Append("<a href=\"");
                        TagStr.Append(LinkURL);
                        TagStr.Append("\"");
                        TagStr.Append(" target=\"_blank\">");
                        TagStr.Append(" <IMG alt=\"" + desc + "\"");
                        TagStr.Append(" src=\"" + filename + "\"");
                        TagStr.Append(" width=\"" + Width + "\" height=\"" + High + "\" border=\"0\">");
                        TagStr.Append("</a>");
                        break;
                    }

                case "application/x-shockwave-flash":
                    {
                        TagStr.Append("<a href=\"");
                        TagStr.Append(LinkURL);
                        TagStr.Append("\"");
                        TagStr.Append(" target=\"_blank\">");
                        TagStr.Append(" <embed src=\"" + filename + "\" ");
                        TagStr.Append(" quality=\"high\" bgcolor=\"#f5f5f5\"");
                        TagStr.Append(" ></embed>");

                        //					TagStr.Append(" <embed src=\""+filename+"\" ");		
                        //					TagStr.Append("pluginspage=\"http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash\"");					
                        //					TagStr.Append(" type=\"application/x-shockwave-flash\"");
                        //					TagStr.Append(" width=\""+Width+"\" height=\""+High+"\"");
                        //					TagStr.Append(" play=\"true\" loop=\"true\" quality=\"high\" scale=\"showall\" ");					
                        //					TagStr.Append(" ></embed>");

                        TagStr.Append("</a>");
                    }

                    break;

                case "video/x-ms-wmv":
                case "video/mpeg":
                case "video/x-ms-asf":
                case "video/avi":
                case "audio/mpeg":
                case "audio/mid":
                case "audio/wav":
                case "audio/x-ms-wma":
                    //					TagStr.Append("<a href=\"");
                    //					TagStr.Append(LinkURL);
                    //					TagStr.Append("\"");
                    //					TagStr.Append(" target=\"_blank\">");
                    //					TagStr.Append("<OBJECT  classid=\"clsid:6BF52A52-394A-11D3-B153-00C04F79FAA6\" VIEWASTEXT>");
                    //					TagStr.Append("<PARAM NAME=\"URL\" VALUE=\""+filename+"\">");
                    //					TagStr.Append("<PARAM NAME=\"autoStart\" VALUE=\"1\">");
                    //					TagStr.Append("<PARAM NAME=\"enableContextMenu\" VALUE=\"0\" ></OBJECT>");	
                    //					TagStr.Append("</a>");

                    TagStr.Append("<a href=\"");
                    TagStr.Append(LinkURL);
                    TagStr.Append("\"");
                    TagStr.Append(" target=\"_blank\">");
                    TagStr.Append("<embed");
                    TagStr.Append(" src=\"" + filename + "\" border=\"0\" width=\"" + Width + "\" height=\"" + High + "\"");
                    TagStr.Append(" autoStart=\"1\" playCount=\"0\" enableContextMenu=\"0\"");
                    TagStr.Append(" type=\"application/x-mplayer2\"></embed>");
                    TagStr.Append("</a>");


                    break;

                default://其他类型作为附件链接下载
                    TagStr.Append("不允许该格式文件显示！");
                    break;
            }

            return TagStr.ToString();

        }

        #endregion

        #endregion

        #region	 创建数据图形文件

        /// <summary>
        /// 创建数据图形文件
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="CharType">图类型 Column,Pie</param>
        /// <param name="ImagePath">图像存放目录</param>
        /// <param name="Title">图形标题</param>
        /// <returns></returns>
        public static string CreateChart(DataTable dt, string CharType, string ImagePath, string Title)
        {
            String PhaysicalImagePath = ImagePath;
            OWCChart.OWCChartFactory mychart = new OWCChartFactory(Title, PhaysicalImagePath, 530, 300, new OWCChartFontStyle());
            OWCChart.OWCSeriesClass MyItem = new OWCSeriesClass();

            MyItem.SeriesName = "次数";
            MyItem.SetDataSource(dt, "Item", "Value");
            switch (CharType)
            {
                case "Column":
                    mychart.CreateOneColumn("时间", "次", MyItem);
                    break;
                case "Pie":
                    mychart.CreateSinglePie(MyItem);
                    break;

            }
            String imageName = mychart.ExportPictuire();
            return imageName;
            //			Image1.ImageUrl = ".\\"+m_imagePath+imageName;

        }
        public static string CreateMultiColumns(DataTable[] dts, string ImagePath, string Title)
        {
            String PhaysicalImagePath = ImagePath;
            OWCChart.OWCChartFactory mychart = new OWCChartFactory(Title, PhaysicalImagePath, 530, 300, new OWCChartFontStyle());
            OWCChart.OWCSeriesClass[] MyItems = new OWCSeriesClass[dts.Length];

            MyItems[0] = new OWCSeriesClass();
            MyItems[0].SeriesName = "显示次数";
            MyItems[0].SetDataSource(dts[0], "Item", "Value");

            MyItems[1] = new OWCSeriesClass();
            MyItems[1].SeriesName = "点击次数";
            MyItems[1].SetDataSource(dts[1], "Item", "Value");


            mychart.CreateMultiColumns("时间", "次", MyItems);


            String imageName = mychart.ExportPictuire();
            return imageName;

        }

        public static string CreateSingleBar(DataTable dt, string CharType, string ImagePath, string Title)
        {
            String PhaysicalImagePath = ImagePath;
            OWCChart.OWCChartFactory mychart = new OWCChartFactory(Title, PhaysicalImagePath, 500, 600, new OWCChartFontStyle());
            OWCChart.OWCSeriesClass MyItem = new OWCSeriesClass();

            MyItem.SeriesName = "次数";
            MyItem.SetDataSource(dt, "Item", "Value");
            mychart.CreateSingleBar(" ", "", MyItem);
            String imageName = mychart.ExportPictuire();
            return imageName;
        }
        public static string CreateMultiBar(DataTable[] dts, string ImagePath, string Title)
        {
            String PhaysicalImagePath = ImagePath;
            OWCChart.OWCChartFactory mychart = new OWCChartFactory(Title, PhaysicalImagePath, 500, 600, new OWCChartFontStyle());
            OWCChart.OWCSeriesClass[] MyItems = new OWCSeriesClass[dts.Length];

            MyItems[0] = new OWCSeriesClass();
            MyItems[0].SeriesName = "显示次数";
            MyItems[0].SetDataSource(dts[0], "Item", "Value");

            MyItems[1] = new OWCSeriesClass();
            MyItems[1].SeriesName = "点击次数";
            MyItems[1].SetDataSource(dts[1], "Item", "Value");


            mychart.CreateMultiBar(" ", "", MyItems);


            String imageName = mychart.ExportPictuire();
            return imageName;

        }

        #endregion

        #region

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


    }
}
