using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Collections;

namespace DotNet.Utilities_Xofly
{
    public enum MailFormat { Text, HTML };
    public enum MailPriority { Low = 1, Normal = 3, High = 5 };

    /// <summary>
    /// 添加附件
    /// </summary>
    public class MailAttachments
    {
        #region 构造函数
        public MailAttachments()
        {
            _Attachments = new ArrayList();
        }
        #endregion

        #region 私有字段
        private IList _Attachments;
        private const int MaxAttachmentNum = 10;
        #endregion

        #region 索引器
        public string this[int index]
        {
            get { return (string)_Attachments[index]; }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 添加邮件附件
        /// </summary>
        /// <param name="FilePath">附件的绝对路径</param>
        public void Add(params string[] filePath)
        {
            if (filePath == null)
            {
                throw (new ArgumentNullException("非法的附件"));
            }
            else
            {
                for (int i = 0; i < filePath.Length; i++)
                {
                    Add(filePath[i]);
                }
            }
        }

        /// <summary>
        /// 添加一个附件,当指定的附件不存在时，忽略该附件，不产生异常。
        /// </summary>
        /// <param name="filePath">附件的绝对路径</param>
        public void Add(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                if (_Attachments.Count < MaxAttachmentNum)
                {
                    _Attachments.Add(filePath);
                }
            }
        }

        /// <summary>
        /// 清除所有附件
        /// </summary>
        public void Clear()
        {
            _Attachments.Clear();
        }

        /// <summary>
        /// 获取附件个数
        /// </summary>
        public int Count
        {
            get { return _Attachments.Count; }
        }
        #endregion
    }

    /// <summary>
    /// 邮件信息
    /// </summary>
    public class MailMessage
    {
        #region 构造函数
        public MailMessage()
        {
            _Recipients = new ArrayList();        //收件人列表
            _Attachments = new MailAttachments(); //附件
            _BodyFormat = MailFormat.HTML;        //缺省的邮件格式为HTML
            _Priority = MailPriority.Normal;
            _Charset = "GB2312";
        }
        #endregion

        #region 私有字段
        private int _MaxRecipientNum = 30;
        private string _From;      //发件人地址
        private string _FromName;  //发件人姓名
        private IList _Recipients; //收件人
        private MailAttachments _Attachments;//附件
        private string _Body;      //内容
        private string _Subject;   //主题
        private MailFormat _BodyFormat;     //邮件格式
        private string _Charset = "GB2312"; //字符编码格式
        private MailPriority _Priority;     //邮件优先级
        #endregion

        #region 公有属性
        /// <summary>
        /// 设定语言代码，默认设定为GB2312，如不需要可设置为""
        /// </summary>
        public string Charset
        {
            get { return _Charset; }
            set { _Charset = value; }
        }

        /// <summary>
        /// 最大收件人
        /// </summary>
        public int MaxRecipientNum
        {
            get { return _MaxRecipientNum; }
            set { _MaxRecipientNum = value; }
        }

        /// <summary>
        /// 发件人地址
        /// </summary>
        public string From
        {
            get { return _From; }
            set { _From = value; }
        }

        /// <summary>
        /// 发件人姓名
        /// </summary>
        public string FromName
        {
            get { return _FromName; }
            set { _FromName = value; }
        }

        /// <summary>
        /// 内容
        /// </summary>
        public string Body
        {
            get { return _Body; }
            set { _Body = value; }
        }

        /// <summary>
        /// 主题
        /// </summary>
        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }

        /// <summary>
        /// 附件
        /// </summary>
        public MailAttachments Attachments
        {
            get { return _Attachments; }
            set { _Attachments = value; }
        }

        /// <summary>
        /// 优先权
        /// </summary>
        public MailPriority Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }

        /// <summary>
        /// 收件人
        /// </summary>
        public IList Recipients
        {
            get { return _Recipients; }
        }

        /// <summary>
        /// 邮件格式
        /// </summary>
        public MailFormat BodyFormat
        {
            set { _BodyFormat = value; }
            get { return _BodyFormat; }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 增加一个收件人地址
        /// </summary>
        /// <param name="recipient">收件人的Email地址</param>
        public void AddRecipients(string recipient)
        {
            if (_Recipients.Count < MaxRecipientNum)
            {
                _Recipients.Add(recipient);
            }
        }

        /// <summary>
        /// 增加多个收件人地址
        /// </summary>
        /// <param name="recipient">收件人的Email地址集合</param>
        public void AddRecipients(params string[] recipient)
        {
            if (recipient == null)
            {
                throw (new ArgumentException("收件人不能为空."));
            }
            else
            {
                for (int i = 0; i < recipient.Length; i++)
                {
                    AddRecipients(recipient[i]);
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// 邮件操作
    /// </summary>
    public class SmtpServerHelper
    {
        #region 构造函数、析构函数
        public SmtpServerHelper()
        {
            SMTPCodeAdd();
        }

        ~SmtpServerHelper()
        {
            networkStream.Close();
            tcpClient.Close();
        }
        #endregion

        #region 私有字段
        /// <summary>
        /// 回车换行
        /// </summary>
        private string CRLF = "\r\n";

        /// <summary>
        /// 错误消息反馈
        /// </summary>
        private string errmsg;

        /// <summary>
        /// TcpClient对象，用于连接服务器
        /// </summary> 
        private TcpClient tcpClient;

        /// <summary>
        /// NetworkStream对象
        /// </summary> 
        private NetworkStream networkStream;

        /// <summary>
        /// 服务器交互记录
        /// </summary>
        private string logs = "";

        /// <summary>
        /// SMTP错误代码哈希表
        /// </summary>
        private Hashtable ErrCodeHT = new Hashtable();

        /// <summary>
        /// SMTP正确代码哈希表
        /// </summary>
        private Hashtable RightCodeHT = new Hashtable();
        #endregion

        #region 公有属性
        /// <summary>
        /// 错误消息反馈
        /// </summary>
        public string ErrMsg
        {
            set { errmsg = value; }
            get { return errmsg; }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 将字符串编码为Base64字符串
        /// </summary>
        /// <param name="str">要编码的字符串</param>
        private string Base64Encode(string str)
        {
            byte[] barray;
            barray = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(barray);
        }

        /// <summary>
        /// 将Base64字符串解码为普通字符串
        /// </summary>
        /// <param name="str">要解码的字符串</param>
        private string Base64Decode(string str)
        {
            byte[] barray;
            barray = Convert.FromBase64String(str);
            return Encoding.Default.GetString(barray);
        }

        /// <summary>
        /// 得到上传附件的文件流
        /// </summary>
        /// <param name="FilePath">附件的绝对路径</param>
        private string GetStream(string FilePath)
        {
            System.IO.FileStream FileStr = new System.IO.FileStream(FilePath, System.IO.FileMode.Open);
            byte[] by = new byte[System.Convert.ToInt32(FileStr.Length)];
            FileStr.Read(by, 0, by.Length);
            FileStr.Close();
            return (System.Convert.ToBase64String(by));
        }

        /// <summary>
        /// SMTP回应代码哈希表
        /// </summary>
        private void SMTPCodeAdd()
        {
            ErrCodeHT.Add("421", "服务未就绪，关闭传输信道");
            ErrCodeHT.Add("432", "需要一个密码转换");
            ErrCodeHT.Add("450", "要求的邮件操作未完成，邮箱不可用（例如，邮箱忙）");
            ErrCodeHT.Add("451", "放弃要求的操作；处理过程中出错");
            ErrCodeHT.Add("452", "系统存储不足，要求的操作未执行");
            ErrCodeHT.Add("454", "临时认证失败");
            ErrCodeHT.Add("500", "邮箱地址错误");
            ErrCodeHT.Add("501", "参数格式错误");
            ErrCodeHT.Add("502", "命令不可实现");
            ErrCodeHT.Add("503", "服务器需要SMTP验证");
            ErrCodeHT.Add("504", "命令参数不可实现");
            ErrCodeHT.Add("530", "需要认证");
            ErrCodeHT.Add("534", "认证机制过于简单");
            ErrCodeHT.Add("538", "当前请求的认证机制需要加密");
            ErrCodeHT.Add("550", "要求的邮件操作未完成，邮箱不可用（例如，邮箱未找到，或不可访问）");
            ErrCodeHT.Add("551", "用户非本地，请尝试<forward-path>");
            ErrCodeHT.Add("552", "过量的存储分配，要求的操作未执行");
            ErrCodeHT.Add("553", "邮箱名不可用，要求的操作未执行（例如邮箱格式错误）");
            ErrCodeHT.Add("554", "传输失败");

            RightCodeHT.Add("220", "服务就绪");
            RightCodeHT.Add("221", "服务关闭传输信道");
            RightCodeHT.Add("235", "验证成功");
            RightCodeHT.Add("250", "要求的邮件操作完成");
            RightCodeHT.Add("251", "非本地用户，将转发向<forward-path>");
            RightCodeHT.Add("334", "服务器响应验证Base64字符串");
            RightCodeHT.Add("354", "开始邮件输入，以<CRLF>.<CRLF>结束");
        }

        /// <summary>
        /// 发送SMTP命令
        /// </summary> 
        private bool SendCommand(string str)
        {
            byte[] WriteBuffer;
            if (str == null || str.Trim() == String.Empty)
            {
                return true;
            }
            logs += str;
            WriteBuffer = Encoding.Default.GetBytes(str);
            try
            {
                networkStream.Write(WriteBuffer, 0, WriteBuffer.Length);
            }
            catch
            {
                errmsg = "网络连接错误";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 接收SMTP服务器回应
        /// </summary>
        private string RecvResponse()
        {
            int StreamSize;
            string Returnvalue = String.Empty;
            byte[] ReadBuffer = new byte[1024];
            try
            {
                StreamSize = networkStream.Read(ReadBuffer, 0, ReadBuffer.Length);
            }
            catch
            {
                errmsg = "网络连接错误";
                return "false";
            }

            if (StreamSize == 0)
            {
                return Returnvalue;
            }
            else
            {
                Returnvalue = Encoding.Default.GetString(ReadBuffer).Substring(0, StreamSize);
                logs += Returnvalue + this.CRLF;
                return Returnvalue;
            }
        }

        /// <summary>
        /// 与服务器交互，发送一条命令并接收回应。
        /// </summary>
        /// <param name="str">一个要发送的命令</param>
        /// <param name="errstr">如果错误，要反馈的信息</param>
        private bool Dialog(string str, string errstr)
        {
            if (str == null || str.Trim() == string.Empty)
            {
                return true;
            }
            if (SendCommand(str))
            {
                string RR = RecvResponse();
                if (RR == "false")
                {
                    return false;
                }

                //检查返回的代码，根据[RFC 821]返回代码为3位数字代码如220
                string RRCode = RR.Substring(0, 3);
                if (RightCodeHT[RRCode] != null)
                {
                    return true;
                }
                else
                {
                    if (ErrCodeHT[RRCode] != null)
                    {
                        errmsg += (RRCode + ErrCodeHT[RRCode].ToString());
                        errmsg += CRLF;
                    }
                    else
                    {
                        errmsg += RR;
                    }
                    errmsg += errstr;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 与服务器交互，发送一组命令并接收回应。
        /// </summary>
        private bool Dialog(string[] str, string errstr)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!Dialog(str[i], ""))
                {
                    errmsg += CRLF;
                    errmsg += errstr;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        private bool Connect(string smtpServer, int port)
        {
            try
            {
                tcpClient = new TcpClient(smtpServer, port);
            }
            catch (Exception e)
            {
                errmsg = e.ToString();
                return false;
            }
            networkStream = tcpClient.GetStream();

            if (RightCodeHT[RecvResponse().Substring(0, 3)] == null)
            {
                errmsg = "网络连接失败";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取优先级
        /// </summary>
        /// <param name="mailPriority">优先级</param>
        private string GetPriorityString(MailPriority mailPriority)
        {
            string priority = "Normal";
            if (mailPriority == MailPriority.Low)
            {
                priority = "Low";
            }
            else if (mailPriority == MailPriority.High)
            {
                priority = "High";
            }
            return priority;
        }

        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="smtpServer">发信SMTP服务器</param>
        /// <param name="port">端口，默认为25</param>
        /// <param name="username">发信人邮箱地址</param>
        /// <param name="password">发信人邮箱密码</param>
        /// <param name="mailMessage">邮件内容</param>
        private bool SendEmail(string smtpServer, int port, bool ESmtp, string username, string password, MailMessage mailMessage)
        {
            if (Connect(smtpServer, port) == false) return false;

            string priority = GetPriorityString(mailMessage.Priority);

            bool Html = (mailMessage.BodyFormat == MailFormat.HTML);

            string[] SendBuffer;
            string SendBufferstr;

            //进行SMTP验证
            if (ESmtp)
            {
                SendBuffer = new String[4];
                SendBuffer[0] = "EHLO " + smtpServer + CRLF;
                SendBuffer[1] = "AUTH LOGIN" + CRLF;
                SendBuffer[2] = Base64Encode(username) + CRLF;
                SendBuffer[3] = Base64Encode(password) + CRLF;
                if (!Dialog(SendBuffer, "SMTP服务器验证失败，请核对用户名和密码。")) return false;
            }
            else
            {
                SendBufferstr = "HELO " + smtpServer + CRLF;
                if (!Dialog(SendBufferstr, "")) return false;
            }

            //发件人地址
            SendBufferstr = "MAIL FROM:<" + username + ">" + CRLF;
            if (!Dialog(SendBufferstr, "发件人地址错误，或不能为空")) return false;

            //收件人地址
            SendBuffer = new string[mailMessage.Recipients.Count];
            for (int i = 0; i < mailMessage.Recipients.Count; i++)
            {
                SendBuffer[i] = "RCPT TO:<" + (string)mailMessage.Recipients[i] + ">" + CRLF;
            }
            if (!Dialog(SendBuffer, "收件人地址有误")) return false;

            SendBufferstr = "DATA" + CRLF;
            if (!Dialog(SendBufferstr, "")) return false;

            //发件人姓名
            SendBufferstr = "From:" + mailMessage.FromName + "<" + mailMessage.From + ">" + CRLF;

            if (mailMessage.Recipients.Count == 0)
            {
                return false;
            }
            else
            {
                SendBufferstr += "To:=?" + mailMessage.Charset.ToUpper() + "?B?" + Base64Encode((string)mailMessage.Recipients[0]) + "?=" + "<" + (string)mailMessage.Recipients[0] + ">" + CRLF;
            }
            SendBufferstr += ((mailMessage.Subject == String.Empty || mailMessage.Subject == null) ? "Subject:" : ((mailMessage.Charset == "") ? ("Subject:" + mailMessage.Subject) : ("Subject:" + "=?" + mailMessage.Charset.ToUpper() + "?B?" + Base64Encode(mailMessage.Subject) + "?="))) + CRLF;
            SendBufferstr += "X-Priority:" + priority + CRLF;
            SendBufferstr += "X-MSMail-Priority:" + priority + CRLF;
            SendBufferstr += "Importance:" + priority + CRLF;
            SendBufferstr += "X-Mailer: Lion.Web.Mail.SmtpMail Pubclass [cn]" + CRLF;
            SendBufferstr += "MIME-Version: 1.0" + CRLF;
            if (mailMessage.Attachments.Count != 0)
            {
                SendBufferstr += "Content-Type: multipart/mixed;" + CRLF;
                SendBufferstr += " boundary=\"=====" + (Html ? "001_Dragon520636771063_" : "001_Dragon303406132050_") + "=====\"" + CRLF + CRLF;
            }
            if (Html)
            {
                if (mailMessage.Attachments.Count == 0)
                {
                    SendBufferstr += "Content-Type: multipart/alternative;" + CRLF; //内容格式和分隔符
                    SendBufferstr += " boundary=\"=====003_Dragon520636771063_=====\"" + CRLF + CRLF;
                    SendBufferstr += "This is a multi-part message in MIME format." + CRLF + CRLF;
                }
                else
                {
                    SendBufferstr += "This is a multi-part message in MIME format." + CRLF + CRLF;
                    SendBufferstr += "--=====001_Dragon520636771063_=====" + CRLF;
                    SendBufferstr += "Content-Type: multipart/alternative;" + CRLF; //内容格式和分隔符
                    SendBufferstr += " boundary=\"=====003_Dragon520636771063_=====\"" + CRLF + CRLF;
                }
                SendBufferstr += "--=====003_Dragon520636771063_=====" + CRLF;
                SendBufferstr += "Content-Type: text/plain;" + CRLF;
                SendBufferstr += ((mailMessage.Charset == "") ? (" charset=\"iso-8859-1\"") : (" charset=\"" + mailMessage.Charset.ToLower() + "\"")) + CRLF;
                SendBufferstr += "Content-Transfer-Encoding: base64" + CRLF + CRLF;
                SendBufferstr += Base64Encode("邮件内容为HTML格式，请选择HTML方式查看") + CRLF + CRLF;

                SendBufferstr += "--=====003_Dragon520636771063_=====" + CRLF;

                SendBufferstr += "Content-Type: text/html;" + CRLF;
                SendBufferstr += ((mailMessage.Charset == "") ? (" charset=\"iso-8859-1\"") : (" charset=\"" + mailMessage.Charset.ToLower() + "\"")) + CRLF;
                SendBufferstr += "Content-Transfer-Encoding: base64" + CRLF + CRLF;
                SendBufferstr += Base64Encode(mailMessage.Body) + CRLF + CRLF;
                SendBufferstr += "--=====003_Dragon520636771063_=====--" + CRLF;
            }
            else
            {
                if (mailMessage.Attachments.Count != 0)
                {
                    SendBufferstr += "--=====001_Dragon303406132050_=====" + CRLF;
                }
                SendBufferstr += "Content-Type: text/plain;" + CRLF;
                SendBufferstr += ((mailMessage.Charset == "") ? (" charset=\"iso-8859-1\"") : (" charset=\"" + mailMessage.Charset.ToLower() + "\"")) + CRLF;
                SendBufferstr += "Content-Transfer-Encoding: base64" + CRLF + CRLF;
                SendBufferstr += Base64Encode(mailMessage.Body) + CRLF;
            }
            if (mailMessage.Attachments.Count != 0)
            {
                for (int i = 0; i < mailMessage.Attachments.Count; i++)
                {
                    string filepath = (string)mailMessage.Attachments[i];
                    SendBufferstr += "--=====" + (Html ? "001_Dragon520636771063_" : "001_Dragon303406132050_") + "=====" + CRLF;
                    SendBufferstr += "Content-Type: text/plain;" + CRLF;
                    SendBufferstr += " name=\"=?" + mailMessage.Charset.ToUpper() + "?B?" + Base64Encode(filepath.Substring(filepath.LastIndexOf("\\") + 1)) + "?=\"" + CRLF;
                    SendBufferstr += "Content-Transfer-Encoding: base64" + CRLF;
                    SendBufferstr += "Content-Disposition: attachment;" + CRLF;
                    SendBufferstr += " filename=\"=?" + mailMessage.Charset.ToUpper() + "?B?" + Base64Encode(filepath.Substring(filepath.LastIndexOf("\\") + 1)) + "?=\"" + CRLF + CRLF;
                    SendBufferstr += GetStream(filepath) + CRLF + CRLF;
                }
                SendBufferstr += "--=====" + (Html ? "001_Dragon520636771063_" : "001_Dragon303406132050_") + "=====--" + CRLF + CRLF;
            }
            SendBufferstr += CRLF + "." + CRLF;
            if (!Dialog(SendBufferstr, "错误信件信息")) return false;

            SendBufferstr = "QUIT" + CRLF;
            if (!Dialog(SendBufferstr, "断开连接时错误")) return false;

            networkStream.Close();
            tcpClient.Close();
            return true;
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 发送电子邮件,SMTP服务器不需要身份验证
        /// </summary>
        /// <param name="smtpServer">发信SMTP服务器</param>
        /// <param name="port">端口，默认为25</param>
        /// <param name="mailMessage">邮件内容</param>
        public bool SendEmail(string smtpServer, int port, MailMessage mailMessage)
        {
            return SendEmail(smtpServer, port, false, "", "", mailMessage);
        }

        /// <summary>
        /// 发送电子邮件,SMTP服务器需要身份验证
        /// </summary>
        /// <param name="smtpServer">发信SMTP服务器</param>
        /// <param name="port">端口，默认为25</param>
        /// <param name="username">发信人邮箱地址</param>
        /// <param name="password">发信人邮箱密码</param>
        /// <param name="mailMessage">邮件内容</param>
        public bool SendEmail(string smtpServer, int port, string username, string password, MailMessage mailMessage)
        {
            return SendEmail(smtpServer, port, true, username, password, mailMessage);
        }
        #endregion
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    //--------------------调用-----------------------
    //MailAttachments ma=new MailAttachments();
    //ma.Add(@"附件地址");
    //MailMessage mail = new MailMessage();
    //mail.Attachments=ma;
    //mail.Body="你好";
    //mail.AddRecipients("zjy99684268@163.com");
    //mail.From="zjy99684268@163.com";
    //mail.FromName="zjy";
    //mail.Subject="Hello";
    //SmtpClient sp = new SmtpClient();
    //sp.SmtpServer = "smtp.163.com";
    //sp.Send(mail, "zjy99684268@163.com", "123456");
    //------------------------------------------------
    public class SmtpClient
    {
        #region 构造函数
        public SmtpClient()
        { }

        public SmtpClient(string _smtpServer)
        {
            _SmtpServer = _smtpServer;
        }
        #endregion

        #region 私有字段
        private string errmsg;
        private string _SmtpServer;
        #endregion

        #region 公有属性
        /// <summary>
        /// 错误消息反馈
        /// </summary>
        public string ErrMsg
        {
            get { return errmsg; }
        }

        /// <summary>
        /// 邮件服务器
        /// </summary>
        public string SmtpServer
        {
            set { _SmtpServer = value; }
            get { return _SmtpServer; }
        }
        #endregion

        public bool Send(MailMessage mailMessage, string username, string password)
        {
            SmtpServerHelper helper = new SmtpServerHelper();
            if (helper.SendEmail(_SmtpServer, 25, username, password, mailMessage))
                return true;
            else
            {
                errmsg = helper.ErrMsg;
                return false;
            }
        }
    }

    /// <summary>
    /// 操作服务器上邮件
    /// </summary>
    public class SmtpMail
    {
        public SmtpMail()
        { }

        #region 字段
        private StreamReader sr;
        private StreamWriter sw;
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        #endregion

        #region 私有方法
        /// <summary>
        /// 向服务器发送信息
        /// </summary>
        private bool SendDataToServer(string str)
        {
            try
            {
                sw.WriteLine(str);
                sw.Flush();
                return true;
            }
            catch (Exception err)
            {
                return false;
            }
        }

        /// <summary>
        /// 从网络流中读取服务器回送的信息
        /// </summary>
        private string ReadDataFromServer()
        {
            string str = null;
            try
            {
                str = sr.ReadLine();
                if (str[0] == '-')
                {
                    str = null;
                }
            }
            catch (Exception err)
            {
                str = err.Message;
            }
            return str;
        }
        #endregion

        #region 获取邮件信息
        /// <summary>
        /// 获取邮件信息
        /// </summary>
        /// <param name="uid">邮箱账号</param>
        /// <param name="pwd">邮箱密码</param>
        /// <returns>邮件信息</returns>
        public ArrayList ReceiveMail(string uid, string pwd)
        {
            ArrayList EmailMes = new ArrayList();
            string str;
            int index = uid.IndexOf('@');
            string pop3Server = "pop3." + uid.Substring(index + 1);
            tcpClient = new TcpClient(pop3Server, 110);
            networkStream = tcpClient.GetStream();
            sr = new StreamReader(networkStream);
            sw = new StreamWriter(networkStream);

            if (ReadDataFromServer() == null) return EmailMes;
            if (SendDataToServer("USER " + uid) == false) return EmailMes;
            if (ReadDataFromServer() == null) return EmailMes;
            if (SendDataToServer("PASS " + pwd) == false) return EmailMes;
            if (ReadDataFromServer() == null) return EmailMes;
            if (SendDataToServer("LIST") == false) return EmailMes;
            if ((str = ReadDataFromServer()) == null) return EmailMes;

            string[] splitString = str.Split(' ');
            int count = int.Parse(splitString[1]);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    if ((str = ReadDataFromServer()) == null) return EmailMes;
                    splitString = str.Split(' ');
                    EmailMes.Add(string.Format("第{0}封邮件，{1}字节", splitString[0], splitString[1]));
                }
                return EmailMes;
            }
            else
            {
                return EmailMes;
            }
        }
        #endregion

        #region 读取邮件内容
        /// <summary>
        /// 读取邮件内容
        /// </summary>
        /// <param name="mailMessage">第几封</param>
        /// <returns>内容</returns>
        public string ReadEmail(string str)
        {
            string state = "";
            if (SendDataToServer("RETR " + str) == false)
                state = "Error";
            else
            {
                state = sr.ReadToEnd();
            }
            return state;
        }
        #endregion

        #region 删除邮件
        /// <summary>
        /// 删除邮件
        /// </summary>
        /// <param name="str">第几封</param>
        /// <returns>操作信息</returns>
        public string DeleteEmail(string str)
        {
            string state = "";
            if (SendDataToServer("DELE " + str) == true)
            {
                state = "成功删除";
            }
            else
            {
                state = "Error";
            }
            return state;
        }
        #endregion

        #region 关闭服务器连接
        /// <summary>
        /// 关闭服务器连接
        /// </summary>
        public void CloseConnection()
        {
            SendDataToServer("QUIT");
            sr.Close();
            sw.Close();
            networkStream.Close();
            tcpClient.Close();
        }
        #endregion
    }

}

