using System;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Drawing;

namespace DotNet.Utilities
{
    /// <summary>
    /// 文件类型
    /// </summary>
    public enum FileExtension
    {
        JPG = 255216,
        GIF = 7173,
        BMP = 6677,
        PNG = 13780,
        RAR = 8297,
        jpg = 255216,
        exe = 7790,
        xml = 6063,
        html = 6033,
        aspx = 239187,
        cs = 117115,
        js = 119105,
        txt = 210187,
        sql = 255254
    }

    /// <summary>
    /// 图片检测类
    /// </summary>
    public static class FileValidation
    {
        #region 上传图片检测类
        /// <summary>
        /// 是否允许
        /// </summary>
        public static bool IsAllowedExtension(HttpPostedFile oFile, FileExtension[] fileEx)
        {
            int fileLen = oFile.ContentLength;
            byte[] imgArray = new byte[fileLen];
            oFile.InputStream.Read(imgArray, 0, fileLen);
            MemoryStream ms = new MemoryStream(imgArray);
            System.IO.BinaryReader br = new System.IO.BinaryReader(ms);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = br.ReadByte();
                fileclass = buffer.ToString();
                buffer = br.ReadByte();
                fileclass += buffer.ToString();
            }
            catch { }
            br.Close();
            ms.Close();
            foreach (FileExtension fe in fileEx)
            {
                if (Int32.Parse(fileclass) == (int)fe) return true;
            }
            return false;
        }

        /// <summary>
        /// 上传前的图片是否可靠
        /// </summary>
        public static bool IsSecureUploadPhoto(HttpPostedFile oFile)
        {
            bool isPhoto = false;
            string fileExtension = System.IO.Path.GetExtension(oFile.FileName).ToLower();
            string[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg", ".bmp" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    isPhoto = true;
                    break;
                }
            }
            if (!isPhoto)
            {
                return true;
            }
            FileExtension[] fe = { FileExtension.BMP, FileExtension.GIF, FileExtension.JPG, FileExtension.PNG };

            if (IsAllowedExtension(oFile, fe))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 上传后的图片是否安全
        /// </summary>
        /// <param name="photoFile">物理地址</param>
        public static bool IsSecureUpfilePhoto(string photoFile)
        {
            bool isPhoto = false;
            string Img = "Yes";
            string fileExtension = System.IO.Path.GetExtension(photoFile).ToLower();
            string[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg", ".bmp" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    isPhoto = true;
                    break;
                }
            }

            if (!isPhoto)
            {
                return true;
            }
            StreamReader sr = new StreamReader(photoFile, System.Text.Encoding.Default);
            string strContent = sr.ReadToEnd();
            sr.Close();
            string str = "request|<script|.getfolder|.createfolder|.deletefolder|.createdirectory|.deletedirectory|.saveas|wscript.shell|script.encode|server.|.createobject|execute|activexobject|language=";
            foreach (string s in str.Split('|'))
            {
                if (strContent.ToLower().IndexOf(s) != -1)
                {
                    File.Delete(photoFile);
                    Img = "No";
                    break;
                }
            }
            return (Img == "Yes");
        }
        #endregion
    }

    /// <summary>
    /// 图片上传类
    /// </summary>
    //----------------调用-------------------
    //imageUpload iu = new imageUpload();
    //iu.AddText = "";
    //iu.CopyIamgePath = "";
    //iu.DrawString_x = ;
    //iu.DrawString_y = ;
    //iu.DrawStyle = ;
    //iu.Font = "";
    //iu.FontSize = ;
    //iu.FormFile = File1;
    //iu.IsCreateImg =;
    //iu.IsDraw = ;
    //iu.OutFileName = "";
    //iu.OutThumbFileName = "";
    //iu.SavePath = @"~/image/";
    //iu.SaveType = ;
    //iu.sHeight  = ;
    //iu.sWidth   = ;
    //iu.Upload();
    //--------------------------------------
    public class ImageUpload
    {
        #region 私有成员
        private int _Error = 0;//返回上传状态。 
        private int _MaxSize = 1024 * 1024;//最大单个上传文件 (默认)
        private string _FileType = "jpg;gif;bmp;png";//所支持的上传类型用"/"隔开 
        private string _SavePath = System.Web.HttpContext.Current.Server.MapPath(".") + "\\";//保存文件的实际路径 
        private int _SaveType = 0;//上传文件的类型，0代表自动生成文件名 
        private HtmlInputFile _FormFile;//上传控件。 
        private string _InFileName = "";//非自动生成文件名设置。 
        private string _OutFileName = "";//输出文件名。 
        private bool _IsCreateImg = true;//是否生成缩略图。 
        private bool _Iss = false;//是否有缩略图生成.
        private int _Height = 0;//获取上传图片的高度 
        private int _Width = 0;//获取上传图片的宽度 
        private int _sHeight = 120;//设置生成缩略图的高度 
        private int _sWidth = 120;//设置生成缩略图的宽度
        private bool _IsDraw = false;//设置是否加水印
        private int _DrawStyle = 0;//设置加水印的方式０：文字水印模式，１：图片水印模式,2:不加
        private int _DrawString_x = 10;//绘制文本的Ｘ坐标（左上角）
        private int _DrawString_y = 10;//绘制文本的Ｙ坐标（左上角）
        private string _AddText = "GlobalNatureCrafts";//设置水印内容
        private string _Font = "宋体";//设置水印字体
        private int _FontSize = 12;//设置水印字大小
        private int _FileSize = 0;//获取已经上传文件的大小
        private string _CopyIamgePath = System.Web.HttpContext.Current.Server.MapPath(".") + "/images/5dm_new.jpg";//图片水印模式下的覆盖图片的实际地址
        #endregion

        #region 公有属性
        /// <summary>
        /// Error返回值
        /// 1、没有上传的文件
        /// 2、类型不允许
        /// 3、大小超限
        /// 4、未知错误
        /// 0、上传成功。 
        /// </summary>
        public int Error
        {
            get { return _Error; }
        }

        /// <summary>
        /// 最大单个上传文件
        /// </summary>
        public int MaxSize
        {
            set { _MaxSize = value; }
        }

        /// <summary>
        /// 所支持的上传类型用";"隔开 
        /// </summary>
        public string FileType
        {
            set { _FileType = value; }
        }

        /// <summary>
        /// 保存文件的实际路径 
        /// </summary>
        public string SavePath
        {
            set { _SavePath = System.Web.HttpContext.Current.Server.MapPath(value); }
            get { return _SavePath; }
        }

        /// <summary>
        /// 上传文件的类型，0代表自动生成文件名
        /// </summary>
        public int SaveType
        {
            set { _SaveType = value; }
        }

        /// <summary>
        /// 上传控件
        /// </summary>
        public HtmlInputFile FormFile
        {
            set { _FormFile = value; }
        }

        /// <summary>
        /// 非自动生成文件名设置。
        /// </summary>
        public string InFileName
        {
            set { _InFileName = value; }
        }

        /// <summary>
        /// 输出文件名
        /// </summary>
        public string OutFileName
        {
            get { return _OutFileName; }
            set { _OutFileName = value; }
        }

        /// <summary>
        /// 输出的缩略图文件名
        /// </summary>
        public string OutThumbFileName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否有缩略图生成.
        /// </summary>
        public bool Iss
        {
            get { return _Iss; }
        }

        /// <summary>
        /// 获取上传图片的宽度
        /// </summary>
        public int Width
        {
            get { return _Width; }
        }

        /// <summary>
        /// 获取上传图片的高度
        /// </summary>
        public int Height
        {
            get { return _Height; }
        }

        /// <summary>
        /// 设置缩略图的宽度
        /// </summary>
        public int sWidth
        {
            get { return _sWidth; }
            set { _sWidth = value; }
        }

        /// <summary>
        /// 设置缩略图的高度
        /// </summary>
        public int sHeight
        {
            get { return _sHeight; }
            set { _sHeight = value; }
        }

        /// <summary>
        /// 是否生成缩略图
        /// </summary>
        public bool IsCreateImg
        {
            get { return _IsCreateImg; }
            set { _IsCreateImg = value; }
        }

        /// <summary>
        /// 是否加水印
        /// </summary>
        public bool IsDraw
        {
            get { return _IsDraw; }
            set { _IsDraw = value; }
        }

        /// <summary>
        /// 设置加水印的方式
        /// 0:文字水印模式
        /// 1:图片水印模式
        /// 2:不加
        /// </summary>
        public int DrawStyle
        {
            get { return _DrawStyle; }
            set { _DrawStyle = value; }
        }

        /// <summary>
        /// 绘制文本的Ｘ坐标（左上角）
        /// </summary>
        public int DrawString_x
        {
            get { return _DrawString_x; }
            set { _DrawString_x = value; }
        }

        /// <summary>
        /// 绘制文本的Ｙ坐标（左上角）
        /// </summary>
        public int DrawString_y
        {
            get { return _DrawString_y; }
            set { _DrawString_y = value; }
        }

        /// <summary>
        /// 设置文字水印内容
        /// </summary>
        public string AddText
        {
            get { return _AddText; }
            set { _AddText = value; }
        }

        /// <summary>
        /// 设置文字水印字体
        /// </summary>
        public string Font
        {
            get { return _Font; }
            set { _Font = value; }
        }

        /// <summary>
        /// 设置文字水印字的大小
        /// </summary>
        public int FontSize
        {
            get { return _FontSize; }
            set { _FontSize = value; }
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize
        {
            get { return _FileSize; }
            set { _FileSize = value; }
        }

        /// <summary>
        /// 图片水印模式下的覆盖图片的实际地址
        /// </summary>
        public string CopyIamgePath
        {
            set { _CopyIamgePath = System.Web.HttpContext.Current.Server.MapPath(value); }
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 获取文件的后缀名 
        /// </summary>
        private string GetExt(string path)
        {
            return Path.GetExtension(path);
        }

        /// <summary>
        /// 获取输出文件的文件名
        /// </summary>
        private string FileName(string Ext)
        {
            if (_SaveType == 0 || _InFileName.Trim() == "")
                return DateTime.Now.ToString("yyyyMMddHHmmssfff") + Ext;
            else
                return _InFileName;
        }

        /// <summary>
        /// 检查上传的文件的类型，是否允许上传。
        /// </summary>
        private bool IsUpload(string Ext)
        {
            Ext = Ext.Replace(".", "");
            bool b = false;
            string[] arrFileType = _FileType.Split(';');
            foreach (string str in arrFileType)
            {
                if (str.ToLower() == Ext.ToLower())
                {
                    b = true;
                    break;
                }
            }
            return b;
        }
        #endregion

        #region 上传图片
        public void Upload()
        {
            HttpPostedFile hpFile = _FormFile.PostedFile;
            if (hpFile == null || hpFile.FileName.Trim() == "")
            {
                _Error = 1;
                return;
            }
            string Ext = GetExt(hpFile.FileName);
            if (!IsUpload(Ext))
            {
                _Error = 2;
                return;
            }
            int iLen = hpFile.ContentLength;
            if (iLen > _MaxSize)
            {
                _Error = 3;
                return;
            }
            try
            {
                if (!Directory.Exists(_SavePath)) Directory.CreateDirectory(_SavePath);
                byte[] bData = new byte[iLen];
                hpFile.InputStream.Read(bData, 0, iLen);
                string FName;
                FName = FileName(Ext);
                string TempFile = "";
                if (_IsDraw)
                {
                    TempFile = FName.Split('.').GetValue(0).ToString() + "_temp." + FName.Split('.').GetValue(1).ToString();
                }
                else
                {
                    TempFile = FName;
                }
                FileStream newFile = new FileStream(_SavePath + TempFile, FileMode.Create);
                newFile.Write(bData, 0, bData.Length);
                newFile.Flush();
                int _FileSizeTemp = hpFile.ContentLength;

                string ImageFilePath = _SavePath + FName;
                if (_IsDraw)
                {
                    if (_DrawStyle == 0)
                    {
                        System.Drawing.Image Img1 = System.Drawing.Image.FromStream(newFile);
                        Graphics g = Graphics.FromImage(Img1);
                        g.DrawImage(Img1, 100, 100, Img1.Width, Img1.Height);
                        Font f = new Font(_Font, _FontSize);
                        Brush b = new SolidBrush(Color.Red);
                        string addtext = _AddText;
                        g.DrawString(addtext, f, b, _DrawString_x, _DrawString_y);
                        g.Dispose();
                        Img1.Save(ImageFilePath);
                        Img1.Dispose();
                    }
                    else
                    {
                        System.Drawing.Image image = System.Drawing.Image.FromStream(newFile);
                        System.Drawing.Image copyImage = System.Drawing.Image.FromFile(_CopyIamgePath);
                        Graphics g = Graphics.FromImage(image);
                        g.DrawImage(copyImage, new Rectangle(image.Width - copyImage.Width - 5, image.Height - copyImage.Height - 5, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
                        g.Dispose();
                        image.Save(ImageFilePath);
                        image.Dispose();
                    }
                }

                //获取图片的高度和宽度
                System.Drawing.Image Img = System.Drawing.Image.FromStream(newFile);
                _Width = Img.Width;
                _Height = Img.Height;

                //生成缩略图部分 
                if (_IsCreateImg)
                {
                    #region 缩略图大小只设置了最大范围，并不是实际大小
                    float realbili = (float)_Width / (float)_Height;
                    float wishbili = (float)_sWidth / (float)_sHeight;

                    //实际图比缩略图最大尺寸更宽矮，以宽为准
                    if (realbili > wishbili)
                    {
                        _sHeight = (int)((float)_sWidth / realbili);
                    }
                    //实际图比缩略图最大尺寸更高长，以高为准
                    else
                    {
                        _sWidth = (int)((float)_sHeight * realbili);
                    }
                    #endregion

                    this.OutThumbFileName = FName.Split('.').GetValue(0).ToString() + "_s." + FName.Split('.').GetValue(1).ToString();
                    string ImgFilePath = _SavePath + this.OutThumbFileName;

                    System.Drawing.Image newImg = Img.GetThumbnailImage(_sWidth, _sHeight, null, System.IntPtr.Zero);
                    newImg.Save(ImgFilePath);
                    newImg.Dispose();
                    _Iss = true;
                }

                if (_IsDraw)
                {
                    if (File.Exists(_SavePath + FName.Split('.').GetValue(0).ToString() + "_temp." + FName.Split('.').GetValue(1).ToString()))
                    {
                        newFile.Dispose();
                        File.Delete(_SavePath + FName.Split('.').GetValue(0).ToString() + "_temp." + FName.Split('.').GetValue(1).ToString());
                    }
                }
                newFile.Close();
                newFile.Dispose();
                _OutFileName = FName;
                _FileSize = _FileSizeTemp;
                _Error = 0;
                return;
            }
            catch (Exception ex)
            {
                _Error = 4;
                return;
            }
        }
        #endregion
    }
}