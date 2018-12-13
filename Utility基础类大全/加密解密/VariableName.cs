using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
    /// <summary>
    /// VariableName 的摘要说明。
    /// </summary>
    public class VariableName
    {
        /// <summary>
        /// 用于对WebService进行会话状态进行判断的键值
        /// </summary>
        public static readonly string WebServiceAuthKey = "RFSYS_WEBSVC_KEY";

        /// <summary>
        /// 用于随机认证码键值
        /// </summary>
        public static readonly string AuthKey = "AUTH_KEY";

        public static readonly string SessionIntervalKey = "SESSION_INTERVAL";

        #region 日期格式
        /// <summary>
        /// 日期格式化字符串"yyyyMMdd"
        /// 如20031206
        /// </summary>
        public static readonly string YYYYMMDD = "yyyyMMdd";

        /// <summary>
        /// 日期格式化字符串"yyyy-MM-dd"
        /// 如2003-12-06
        /// </summary>
        public static readonly string YYYY_MM_DD = "yyyy-MM-dd";

        /// <summary>
        /// 12小时制日期格式化字符串"yyyy-MM-dd hh:mm:ss"
        /// 如2003-12-06 08:29:32(可能表示是上午8时或下午20时)
        /// </summary>
        public static readonly string YYYY_MM_DD_hh_mm_ss = "yyyy-MM-dd hh:mm:ss";

        /// <summary>
        /// 24小时制日期格式化字符串"yyyy-MM-dd HH:mm:ss"
        /// 如2003-12-06 15:29:32
        /// </summary>
        public static readonly string YYYY_MM_DD_HH_mm_ss = "yyyy-MM-dd HH:mm:ss";
        #endregion

        #region 加密解密
        /// <summary>
        /// 可逆加密算法默认的密钥
        /// </summary>
        public static readonly string DefaultEncryptKey = "TianChi Info Tech 2003-12-23";
        #endregion

        /// <summary>
        /// 服务器配置文件中关于限制日期的键值
        /// </summary>
        public static readonly string LimitDate = "LimitDate";

        #region 数据库
        /// <summary>
        /// 服务器配置文件中关于Access数据库路径
        /// </summary>
        public static readonly string AccessDataBasePathKey = "AccessDataBasePath";
        /// <summary>
        /// 服务器配置文件中关于数据库服务类型的键值
        /// </summary>
        public static readonly string DBServerTypeKey = "DBType";
        /// <summary>
        /// 服务器配置文件中关于数据库服务地址的键值
        /// </summary>
        public static readonly string DBServerAddressKey = "DBAddress";

        /// <summary>
        /// 服务器配置文件中关于数据库服务用户名的键值
        /// </summary>
        public static readonly string DBServerUserKey = "DBUserName";

        /// <summary>
        /// 服务器配置文件中关于数据库服务密码的键值
        /// </summary>
        public static readonly string DBServerPasswordKey = "DBPassword";
        /// <summary>
        /// 服务器配置文件中关于数据库名称的键值
        /// </summary>
        public static readonly string DBCataloglKey = "DBCatalog";
        #endregion

        #region 日志
        /// <summary>
        /// 服务器配置文件中关于日志目录的键值
        /// </summary>
        public static readonly string ServerLogPathKey = "ServerLogPath";

        /// <summary>
        /// 服务器配置文件中关于日志级别的键值
        /// </summary>
        public static readonly string ServerLogLevelKey = "ServerLogLevel";

        /// <summary>
        /// 项目名称
        /// </summary>
        public static readonly string ProjectName = "ProjectName";
        #endregion

        #region 文件上传
        /// <summary>
        /// 文件上传目录
        /// </summary>
        public static readonly string FileUploadDirectory = "FileUploadDirectory";
        #endregion

        #region 邮件
        /// <summary>
        /// SMTP邮件服务器地址
        /// </summary>
        public static readonly string SMTPServer = "SMTPServer";
        /// <summary>
        /// SMTP邮件服务器端口
        /// </summary>
        public static readonly string SMTPServerPort = "SMTPServerPort";
        /// <summary>
        /// SMTPEnableSsl
        /// 如果使用GMail，则需要设置为true 
        /// </summary>
        public static readonly string SMTPEnableSsl = "SMTPEnableSsl";
        /// <summary>
        /// SMTP邮件服务器用户名
        /// </summary>
        public static readonly string SMTPUserID = "SMTPUserID";
        /// <summary>
        /// SMTP邮件服务器用户密码
        /// </summary>
        public static readonly string SMTPPassword = "SMTPPassword";
        /// <summary>
        /// 错误报告邮箱
        /// </summary>
        public static readonly string ErrorEmail = "ErrorEmail";
        /// <summary>
        /// 支持邮箱
        /// </summary>
        public static readonly string SupportEmail = "SupportEmail";
        #endregion

        #region WCF访问
        /// <summary>
        /// WCFUserName
        /// </summary>
        public static readonly string WCFUserName = "WCFUserName";
        /// <summary>
        /// WCFPassWord
        /// </summary>
        public static readonly string WCFPassWord = "WCFPassWord";
        #endregion
        /// <summary>
        /// WCF服务器地址
        /// </summary>
        public static readonly string ServiceHost = "ServiceHost";
        /// <summary>
        /// 是否加密
        /// </summary>
        public static readonly string IsEncrypt = "IsEncrypt";

        #region IGotMessage
        /// <summary>
        /// 访问视频文件网址路径
        /// </summary>
        public static readonly string IAdd = "iAdd";
        public static readonly string IUrl = "iUrl";
        public static readonly string FfmpegExecFile = "ffmpegExecFile";
        public static readonly string ImageDir = "imageDir";
        public static readonly string VideoDir = "videoDir";
        #endregion
    }
}
