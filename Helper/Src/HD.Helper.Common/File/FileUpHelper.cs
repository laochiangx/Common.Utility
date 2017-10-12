using System;
using System.IO;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI.HtmlControls;

namespace HD.Helper.Common
{
    /// <summary>
    /// 文件上传帮助类
    /// </summary>
    public class FileUpHelper
    {
        public FileUpHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 上传文件
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath">保存文件地址</param>
        /// <param name="maxSize">文件最大大小</param>
        /// <param name="fileType">文件后缀类型</param>
        /// <param name="TargetFile">控件名</param>
        /// <returns></returns>
        public string UploadFile(
            string filePath,
            int maxSize,
            string[] fileType,
            HtmlInputFile TargetFile)
        {
            string Result = "UnDefine";
            bool typeFlag = false;
            string FilePath = filePath;
            int MaxSize = maxSize;
            string strFileName, strNewName, strFilePath;
            if (TargetFile.PostedFile.FileName == "")
            {
                return "FILE_ERR";
            }
            strFileName = TargetFile.PostedFile.FileName;
            TargetFile.Accept = "*/*";
            strFilePath = FilePath;
            if (Directory.Exists(strFilePath) == false)
            {
                Directory.CreateDirectory(strFilePath);
            }
            FileInfo myInfo = new FileInfo(strFileName);
            string strOldName = myInfo.Name;
            strNewName = strOldName.Substring(strOldName.LastIndexOf("."));
            strNewName = strNewName.ToLower();
            if (TargetFile.PostedFile.ContentLength <= MaxSize)
            {
                for (int i = 0; i <= fileType.GetUpperBound(0); i++)
                {
                    if (strNewName.ToLower() == fileType[i].ToString()) { typeFlag = true; break; }
                }
                if (typeFlag)
                {
                    string strFileNameTemp = GetUploadFileName();
                    string strFilePathTemp = strFilePath;
                    float strFileSize = TargetFile.PostedFile.ContentLength;
                    strOldName = strFileNameTemp + strNewName;
                    strFilePath = strFilePath + "\\" + strOldName;
                    TargetFile.PostedFile.SaveAs(strFilePath);
                    Result = strOldName + "|" + strFileSize;
                    TargetFile.Dispose();
                }
                else
                {
                    return "TYPE_ERR";
                }
            }
            else
            {
                return "SIZE_ERR";
            }
            return (Result);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath">保存文件地址</param>
        /// <param name="maxSize">文件最大大小</param>
        /// <param name="fileType">文件后缀类型</param>
        /// <param name="TargetFile">控件名</param>
        /// <param name="saveFileName">保存后的文件名和地址</param>
        /// <param name="fileSize">文件大小</param>
        /// <returns></returns>
        public string UploadFile(
            string filePath,
            int maxSize,
            string[] fileType,
            HtmlInputFile TargetFile,
            out string saveFileName,
            out int fileSize)
        {
            saveFileName = "";
            fileSize = 0;

            string Result = "";
            bool typeFlag = false;
            string FilePath = filePath;
            int MaxSize = maxSize;
            string strFileName, strNewName, strFilePath;
            if (TargetFile.PostedFile.FileName == "")
            {
                return "请选择上传的文件";
            }
            strFileName = TargetFile.PostedFile.FileName;
            TargetFile.Accept = "*/*";
            strFilePath = FilePath;
            if (Directory.Exists(strFilePath) == false)
            {
                Directory.CreateDirectory(strFilePath);
            }
            FileInfo myInfo = new FileInfo(strFileName);
            string strOldName = myInfo.Name;
            strNewName = strOldName.Substring(strOldName.LastIndexOf("."));
            strNewName = strNewName.ToLower();
            if (TargetFile.PostedFile.ContentLength <= MaxSize)
            {
                string strFileNameTemp = GetUploadFileName();
                string strFilePathTemp = strFilePath;
                strOldName = strFileNameTemp + strNewName;
                strFilePath = strFilePath + "\\" + strOldName;

                fileSize = TargetFile.PostedFile.ContentLength / 1024;
                saveFileName = strFilePath.Substring(strFilePath.IndexOf("FileUpload\\"));
                TargetFile.PostedFile.SaveAs(strFilePath);
                TargetFile.Dispose();
            }
            else
            {
                return "上传文件超出指定的大小";
            }
            return (Result);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath">保存文件地址</param>
        /// <param name="maxSize">文件最大大小</param>
        /// <param name="fileType">文件后缀类型</param>
        /// <param name="filename">指定保存后的文件名</param>
        /// <param name="TargetFile">控件名</param>
        /// <returns></returns>
        public string UploadFile(
            string filePath,
            int maxSize,
            string[] fileType,
            string filename,
            HtmlInputFile TargetFile)
        {
            string Result = "UnDefine";
            bool typeFlag = false;
            string FilePath = filePath;
            int MaxSize = maxSize;
            string strFileName, strNewName, strFilePath;
            if (TargetFile.PostedFile.FileName == "")
            {
                return "FILE_ERR";
            }
            strFileName = TargetFile.PostedFile.FileName;
            TargetFile.Accept = "*/*";
            strFilePath = FilePath;
            if (Directory.Exists(strFilePath) == false)
            {
                Directory.CreateDirectory(strFilePath);
            }
            FileInfo myInfo = new FileInfo(strFileName);
            string strOldName = myInfo.Name;
            strNewName = strOldName.Substring(strOldName.Length - 3, 3);
            strNewName = strNewName.ToLower();
            if (TargetFile.PostedFile.ContentLength <= MaxSize)
            {
                for (int i = 0; i <= fileType.GetUpperBound(0); i++)
                {
                    if (strNewName.ToLower() == fileType[i].ToString()) { typeFlag = true; break; }
                }
                if (typeFlag)
                {
                    string strFileNameTemp = filename;
                    string strFilePathTemp = strFilePath;
                    strOldName = strFileNameTemp + "." + strNewName;
                    strFilePath = strFilePath + "\\" + strOldName;
                    TargetFile.PostedFile.SaveAs(strFilePath);
                    Result = strOldName;
                    TargetFile.Dispose();
                }
                else
                {
                    return "TYPE_ERR";
                }
            }
            else
            {
                return "SIZE_ERR";
            }
            return (Result);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="binData">字节数组</param>
        /// <param name="fileName">文件名</param>
        /// <param name="fileType">文件类型</param>
        //-------------------调用----------------------
        //byte[] by = GetBinaryFile("E:\\Hello.txt");
        //this.SaveFile(by,"Hello",".txt");
        //---------------------------------------------
        public void UploadFile(byte[] binData, string fileName, string fileType)
        {
            FileStream fileStream = null;
            MemoryStream m = new MemoryStream(binData);
            try
            {
                string savePath = HttpContext.Current.Server.MapPath("~/File/");
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                string File = savePath + fileName + fileType;
                fileStream = new FileStream(File, FileMode.Create);
                m.WriteTo(fileStream);
            }
            finally
            {
                m.Close();
                fileStream.Close();
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="PosPhotoUpload">控件</param>
        /// <param name="saveFileName">保存的文件名</param>
        /// <param name="imagePath">保存的文件路径</param>
        public string UploadFile(FileUpload PosPhotoUpload, string saveFileName, string imagePath)
        {
            string state = "";
            if (PosPhotoUpload.HasFile)
            {
                if (PosPhotoUpload.PostedFile.ContentLength / 1024 < 10240)
                {
                    string MimeType = PosPhotoUpload.PostedFile.ContentType;
                    if (String.Equals(MimeType, "image/gif") || String.Equals(MimeType, "image/pjpeg"))
                    {
                        string extFileString = System.IO.Path.GetExtension(PosPhotoUpload.PostedFile.FileName);
                        PosPhotoUpload.PostedFile.SaveAs(HttpContext.Current.Server.MapPath(imagePath));
                    }
                    else
                    {
                        state = "上传文件类型不正确";
                    }
                }
                else
                {
                    state = "上传文件不能大于10M";
                }
            }
            else
            {
                state = "没有上传文件";
            }
            return state;
        }
        #endregion

        #region 生成上传文件名
        /// <summary>
        /// 生成上传文件名
        /// </summary>
        /// <returns></returns>
        public string GetUploadFileName()
        {
            string Result = "";
            DateTime time = DateTime.Now;
            Result += time.Year.ToString() + FormatNum(time.Month.ToString(), 2) + FormatNum(time.Day.ToString(), 2) + FormatNum(time.Hour.ToString(), 2) + FormatNum(time.Minute.ToString(), 2) + FormatNum(time.Second.ToString(), 2) + FormatNum(time.Millisecond.ToString(), 3);
            return (Result);
        } 
        #endregion

        #region 字符串前补0
        /// <summary>
        /// 字符串前补0
        /// </summary>
        /// <param name="Num">字符串</param>
        /// <param name="Bit">字符串长度</param>
        /// <returns></returns>
        public string FormatNum(string Num, int Bit)
        {
            for (int i = Num.Length; i < Bit; i++)
            {
                Num = "0" + Num;
            }
            return Num;
        } 
        #endregion

        #region 转换为字节数组
        /// <summary>
        /// 转换为字节数组
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>字节数组</returns>
        public byte[] GetBinaryFile(string filename)
        {
            if (File.Exists(filename))
            {
                FileStream Fsm = null;
                try
                {
                    Fsm = File.OpenRead(filename);
                    return this.ConvertStreamToByteBuffer(Fsm);
                }
                catch
                {
                    return new byte[0];
                }
                finally
                {
                    Fsm.Close();
                }
            }
            else
            {
                return new byte[0];
            }
        } 
        #endregion

        #region 流转化为字节数组
        /// <summary>
        /// 流转化为字节数组
        /// </summary>
        /// <param name="theStream">流</param>
        /// <returns>字节数组</returns>
        public byte[] ConvertStreamToByteBuffer(System.IO.Stream theStream)
        {
            int bi;
            MemoryStream tempStream = new System.IO.MemoryStream();
            try
            {
                while ((bi = theStream.ReadByte()) != -1)
                {
                    tempStream.WriteByte(((byte)bi));
                }
                return tempStream.ToArray();
            }
            catch
            {
                return new byte[0];
            }
            finally
            {
                tempStream.Close();
            }
        } 
        #endregion

    }
}
