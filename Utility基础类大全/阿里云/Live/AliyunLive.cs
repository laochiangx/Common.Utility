using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DotNet.Utilities.Aliyun.Live
{
    /*阿里云中直播 分为：推流，拉流，录制
     * 为防止推流，拉流url非法盗用，使用了A类鉴权 对url进行鉴权
     * A类鉴权：在阿里云平台创建一个私钥，在双方服务器上用相同的方式md5sum加密
     */
    /// <summary>
    /// 阿里云直播
    /// </summary>
    public class AliyunLive
    {
        /// <summary>
        /// 创建授权url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
       public string CreatAuthUrl(string url, string domain)
        {
            string privateKey = ConfigurationManager.AppSettings["aliyunLiveKey"].ToString();
            string timeStamp = ConvertDateTime(DateTime.Now);
            string URI = url.Replace(domain, "");
            //sstring = "URI-Timestamp-rand-uid-PrivateKey"
            if (URI.Contains("?"))
            {
                URI = URI.Split('?')[0];
            }

            string s = string.Format("{0}-{1}-{2}-{3}-{4}", URI, timeStamp, 0, 0, privateKey);
            var authKey = Md5Sum(s);

            string result = string.Format("{0}-{1}-{2}-{3}", timeStamp, 0, 0, authKey);
            return url + "&auth_key=" + result;

        }
        string Md5Sum(string strToEncrypt)
        {
            byte[] bs = UTF8Encoding.UTF8.GetBytes(strToEncrypt);
            return Md5Sum(bs);
        }
        string Md5Sum(byte[] bs)
        {
            // 创建md5 对象  
            System.Security.Cryptography.MD5 md5;
            md5 = System.Security.Cryptography.MD5CryptoServiceProvider.Create();

            // 生成16位的二进制校验码  
            byte[] hashBytes = md5.ComputeHash(bs);

            // 转为32位字符串  
            string hashString = "";
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0');
        }


        string ConvertDateTime(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return Convert.ToString((int)(time - startTime).TotalSeconds);
        }

        /*   生成推流，与拉流的例子

        [HttpPost]
        public object CreateLive(CreateLiveModel model)
        {
            //原始推流地址
            //rtmp://video-center.alivecdn.com/APPName/StreamName?vhost=live.tutaojin.com
            //原始直播地址
            //m3u8格式 http://live.tutaojin.com/AppName/StreamName.m3u8

            //使用阿里云的A鉴权方式：http://DomainName/Filename?auth_key=timestamp-rand-uid-md5hash
            var userinfo = base.CurrentUser;
            if (userinfo == null) return null;
            //创建新的直播信息
            LiveInfoModel info = new LiveInfoModel
            {
                Cover = model.Cover,
                Title = model.Title,
                UserId = userinfo.Id
            };
            var target = liveService.Create(info);
            if (target == null)
            {
                return new { success = false, msg = "无法创建直播" };
            }
            var liveId = target.Id;
            string url = string.Format("rtmp://video-center.alivecdn.com/{0}/{1}?vhost={2}", "tutaojin", "stream-" + liveId, "live.tutaojin.com");

            return new { success = true, url = CreatAuthUrl(url, "rtmp://video-center.alivecdn.com"), liveId = liveId };
        }

        /// <summary>
        /// 获取直播地址
        /// </summary>
        /// <param name="LiveId"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetLive(long LiveId)
        {
            var userinfo = base.CurrentUser;
            if (userinfo == null) return null;
            //原拉流地址http: // live.aliyun.com / {AppName} / {StreamName} .m3u8
            string url = "http://live.tutaojin.com/tutaojin/stream-" + LiveId + ".m3u8";
            return new { sucess = true, url = CreatAuthUrl(url, "http://live.tutaojin.com") };
        }
        */
    }
}
