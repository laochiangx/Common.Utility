 
using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utility.Aliyun.OSS
{
    /// <summary>
    /// 阿里云存储
    /// </summary>
    public class Storage
    {
        public string AccessKeyID { get; set; }
        public string AccessKeySecret { get; set; }
        public string BucketName { get; set; }
        public string EndPoint { get; set; }
        public Storage()
        {
            AccessKeyID = System.Configuration.ConfigurationManager.AppSettings["AccessKeyID"].ToString();
            AccessKeySecret = System.Configuration.ConfigurationManager.AppSettings["AccessKeySecret"].ToString();
            BucketName = System.Configuration.ConfigurationManager.AppSettings["BucketName"].ToString();
            EndPoint = System.Configuration.ConfigurationManager.AppSettings["EndPoint"].ToString();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileName">文件名：/images/demo.jpg</param>
        /// <param name="fileStream"></param>
        public void Upload(string fileName, Stream fileStream)
        {

            OssClient ossClient = new OssClient(EndPoint, AccessKeyID, AccessKeySecret);
            ObjectMetadata metadata = new ObjectMetadata();
            //根据文件名设置ContentType
            metadata.ContentType = GetContentType(fileName);
            string key = "MerLogo/" + fileName;
            fileStream.Seek(0, SeekOrigin.Begin);
            PutObjectResult result = ossClient.PutObject(BucketName, key, fileStream, metadata);
        }

     
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileStream"></param>
        /// <param name="dir"></param>
        /// <returns>返回存储后文件路径</returns>
        public string Upload(string fileName, Stream fileStream,string dir)
        {

            OssClient ossClient = new OssClient(EndPoint, AccessKeyID, AccessKeySecret);
            ObjectMetadata metadata = new ObjectMetadata();
            //根据文件名设置ContentType
            metadata.ContentType = GetContentType(fileName);

            string fileext = fileName.Split('.')[1];
            fileName = Guid.NewGuid() + "." + fileext;

            string key = dir + fileName;
            fileStream.Seek(0, SeekOrigin.Begin);
            PutObjectResult result = ossClient.PutObject(BucketName, key, fileStream, metadata);
            return dir+fileName;
        }
      

        private string GetContentType(string fileName)
        {
            string[] fileNameArray = fileName.Split('.');
            string contentType = string.Empty;
            if (fileNameArray.Length >= 2)
            {
                switch (fileNameArray[1])
                {
                    case "gif":
                        contentType = "image/gif";
                        break;
                    case "png":
                        contentType = "image/png";
                        break;
                    case "jpg":
                        contentType = "image/jpg";
                        break;
                    case "rar":
                        contentType = "application/octet-stream";
                        break;
                    case "zip":
                        contentType = "application/zip";
                        break;
                    default:
                        break;
                }
            }
            return contentType;
        }
    }
     
}


//使用例子
//        //获取上传图片                
//        HttpPostedFileBase file = Request.Files["file0"];
//        if (file != null && file.ContentLength > 0)
//        {
//            OSS.Storage sotre = new OSS.Storage();
//            ImageUrl= sotre.Upload(file.FileName, file.InputStream,"Product/");  
//        }