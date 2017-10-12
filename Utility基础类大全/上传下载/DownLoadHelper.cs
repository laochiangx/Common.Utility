using System;
using System.IO;
using System.Web;
using System.Threading;

namespace DotNet.Utilities
{
    /// <summary>
    /// 文件下载帮助类
    /// </summary>
    public class DownLoadHelper
    {
        #region ResponseFile 输出硬盘文件，提供下载 支持大文件、续传、速度限制、资源占用小
        /// <summary>
        ///  输出硬盘文件，提供下载 支持大文件、续传、速度限制、资源占用小
        /// </summary>
        /// <param name="_Request">Page.Request对象</param>
        /// <param name="_Response">Page.Response对象</param>
        /// <param name="_fileName">下载文件名</param>
        /// <param name="_fullPath">带文件名下载路径</param>
        /// <param name="_speed">每秒允许下载的字节数</param>
        /// <returns>返回是否成功</returns>
        public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
        {
            try
            {
                FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    _Response.AddHeader("Accept-Ranges", "bytes");
                    _Response.Buffer = false;
                    long fileLength = myFile.Length;
                    long startBytes = 0;

                    int pack = 10240; //10K bytes
                    //int sleep = 200;   //每秒5次   即5*10K bytes每秒
                    int sleep = (int)Math.Floor((double)(1000 * pack / _speed)) + 1;
                    if (_Request.Headers["Range"] != null)
                    {
                        _Response.StatusCode = 206;
                        string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                    {
                        _Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }
                    _Response.AddHeader("Connection", "Keep-Alive");
                    _Response.ContentType = "application/octet-stream";
                    _Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor((double)((fileLength - startBytes) / pack)) + 1;

                    for (int i = 0; i < maxCount; i++)
                    {
                        if (_Response.IsClientConnected)
                        {
                            _Response.BinaryWrite(br.ReadBytes(pack));
                            Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion
        /// <summary>
        /// 文件下载帮助类
        /// </summary>
        public class DownLoadHelper
        {
            #region ResponseFile 输出硬盘文件，提供下载 支持大文件、续传、速度限制、资源占用小
            /// <summary>
            /// 输出硬盘文件，提供下载 支持大文件、续传、速度限制、资源占用小
            /// </summary>
            /// <param name="_Request">Page.Request对象</param>
            /// <param name="_Response">Page.Response对象</param>
            /// <param name="_fileName">下载文件名</param>
            /// <param name="_fullPath">带文件名下载路径</param>
            /// <param name="_speed">每秒允许下载的字节数</param>
            /// <returns>返回是否成功</returns>
            public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
            {
                try
                {
                    FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    BinaryReader br = new BinaryReader(myFile);
                    try
                    {
                        _Response.AddHeader("Accept-Ranges", "bytes");
                        _Response.Buffer = false;
                        long fileLength = myFile.Length;
                        long startBytes = 0;

                        int pack = 10240; //10K bytes
                        //int sleep = 200; //每秒5次 即5*10K bytes每秒
                        int sleep = (int)Math.Floor((double)(1000 * pack / _speed)) + 1;
                        if (_Request.Headers["Range"] != null)
                        {
                            _Response.StatusCode = 206;
                            string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                            startBytes = Convert.ToInt64(range[1]);
                        }
                        _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                        if (startBytes != 0)
                        {
                            _Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                        }
                        _Response.AddHeader("Connection", "Keep-Alive");
                        _Response.ContentType = "application/octet-stream";
                        _Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));

                        br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                        int maxCount = (int)Math.Floor((double)((fileLength - startBytes) / pack)) + 1;

                        for (int i = 0; i < maxCount; i++)
                        {
                            if (_Response.IsClientConnected)
                            {
                                _Response.BinaryWrite(br.ReadBytes(pack));
                                Thread.Sleep(sleep);
                            }
                            else
                            {
                                i = maxCount;
                            }
                        }
                    }
                    catch
                    {
                        return false;
                    }
                    finally
                    {
                        br.Close();
                        myFile.Close();
                    }
                }
                catch
                {
                    return false;
                }
                return true;
            }
            #endregion
        }



        /// <summary>
        /// 文件下载类
        /// </summary>
        public class FileDown
        {
            public FileDown()
            { }

            /// <summary>
            /// 参数为虚拟路径
            /// </summary>
            public static string FileNameExtension(string FileName)
            {
                return Path.GetExtension(MapPathFile(FileName));
            }

            /// <summary>
            /// 获取物理地址
            /// </summary>
            public static string MapPathFile(string FileName)
            {
                return HttpContext.Current.Server.MapPath(FileName);
            }

            /// <summary>
            /// 普通下载
            /// </summary>
            /// <param name="FileName">文件虚拟路径</param>
            public static void DownLoadold(string FileName)
            {
                string destFileName = MapPathFile(FileName);
                if (File.Exists(destFileName))
                {
                    FileInfo fi = new FileInfo(destFileName);
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.Buffer = false;
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(Path.GetFileName(destFileName), System.Text.Encoding.UTF8));
                    HttpContext.Current.Response.AppendHeader("Content-Length", fi.Length.ToString());
                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    HttpContext.Current.Response.WriteFile(destFileName);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
            }

            /// <summary>
            /// 分块下载
            /// </summary>
            /// <param name="FileName">文件虚拟路径</param>
            public static void DownLoad(string FileName)
            {
                string filePath = MapPathFile(FileName);
                long chunkSize = 204800; //指定块大小 
                byte[] buffer = new byte[chunkSize]; //建立一个200K的缓冲区 
                long dataToRead = 0; //已读的字节数 
                FileStream stream = null;
                try
                {
                    //打开文件 
                    stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    dataToRead = stream.Length;

                    //添加Http头 
                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachement;filename=" + HttpUtility.UrlEncode(Path.GetFileName(filePath)));
                    HttpContext.Current.Response.AddHeader("Content-Length", dataToRead.ToString());

                    while (dataToRead > 0)
                    {
                        if (HttpContext.Current.Response.IsClientConnected)
                        {
                            int length = stream.Read(buffer, 0, Convert.ToInt32(chunkSize));
                            HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.Clear();
                            dataToRead -= length;
                        }
                        else
                        {
                            dataToRead = -1; //防止client失去连接 
                        }
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Response.Write("Error:" + ex.Message);
                }
                finally
                {
                    if (stream != null) stream.Close();
                    HttpContext.Current.Response.Close();
                }
            }

            /// <summary>
            /// 输出硬盘文件，提供下载 支持大文件、续传、速度限制、资源占用小
            /// </summary>
            /// <param name="_Request">Page.Request对象</param>
            /// <param name="_Response">Page.Response对象</param>
            /// <param name="_fileName">下载文件名</param>
            /// <param name="_fullPath">带文件名下载路径</param>
            /// <param name="_speed">每秒允许下载的字节数</param>
            /// <returns>返回是否成功</returns>
            //---------------------------------------------------------------------
            //调用：
            // string FullPath=Server.MapPath("count.txt");
            // ResponseFile(this.Request,this.Response,"count.txt",FullPath,100);
            //---------------------------------------------------------------------
            public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
            {
                try
                {
                    FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    BinaryReader br = new BinaryReader(myFile);
                    try
                    {
                        _Response.AddHeader("Accept-Ranges", "bytes");
                        _Response.Buffer = false;

                        long fileLength = myFile.Length;
                        long startBytes = 0;
                        int pack = 10240; //10K bytes
                        int sleep = (int)Math.Floor((double)(1000 * pack / _speed)) + 1;

                        if (_Request.Headers["Range"] != null)
                        {
                            _Response.StatusCode = 206;
                            string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                            startBytes = Convert.ToInt64(range[1]);
                        }
                        _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                        if (startBytes != 0)
                        {
                            _Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                        }

                        _Response.AddHeader("Connection", "Keep-Alive");
                        _Response.ContentType = "application/octet-stream";
                        _Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));

                        br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                        int maxCount = (int)Math.Floor((double)((fileLength - startBytes) / pack)) + 1;

                        for (int i = 0; i < maxCount; i++)
                        {
                            if (_Response.IsClientConnected)
                            {
                                _Response.BinaryWrite(br.ReadBytes(pack));
                                Thread.Sleep(sleep);
                            }
                            else
                            {
                                i = maxCount;
                            }
                        }
                    }
                    catch
                    {
                        return false;
                    }
                    finally
                    {
                        br.Close();
                        myFile.Close();
                    }
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }







        /// <summary>
        /// 文件上传类
        /// </summary>
        public class FileUp
        {
            public FileUp()
            { }

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

            /// <summary>
            /// 上传文件
            /// </summary>
            /// <param name="PosPhotoUpload">控件</param>
            /// <param name="saveFileName">保存的文件名</param>
            /// <param name="imagePath">保存的文件路径</param>
            public string FileSc(FileUpload PosPhotoUpload, string saveFileName, string imagePath)
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
            public void SaveFile(byte[] binData, string fileName, string fileType)
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
        }









        /// <summary>
        /// UpLoadFiles 的摘要说明
        /// </summary>
        public class UpLoadFiles : System.Web.UI.Page
        {
            public UpLoadFiles()
            {
                //
                // TODO: 在此处添加构造函数逻辑
                //
            }

            public string UploadFile(string filePath, int maxSize, string[] fileType, System.Web.UI.HtmlControls.HtmlInputFile TargetFile)
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
            public string UploadFile(string filePath, int maxSize, string[] fileType, System.Web.UI.HtmlControls.HtmlInputFile TargetFile, out string saveFileName, out int fileSize)
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

            public string UploadFile(string filePath, int maxSize, string[] fileType, string filename, System.Web.UI.HtmlControls.HtmlInputFile TargetFile)
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

            public string GetUploadFileName()
            {
                string Result = "";
                DateTime time = DateTime.Now;
                Result += time.Year.ToString() + FormatNum(time.Month.ToString(), 2) + FormatNum(time.Day.ToString(), 2) + FormatNum(time.Hour.ToString(), 2) + FormatNum(time.Minute.ToString(), 2) + FormatNum(time.Second.ToString(), 2) + FormatNum(time.Millisecond.ToString(), 3);
                return (Result);
            }

            public string FormatNum(string Num, int Bit)
            {
                int L;
                L = Num.Length;
                for (int i = L; i < Bit; i++)
                {
                    Num = "0" + Num;
                }
                return (Num);
            }

        }
    }
}
