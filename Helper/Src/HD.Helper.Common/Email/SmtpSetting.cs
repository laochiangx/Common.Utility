using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HD.Helper.Common
{
    /// <summary>
    /// smtp设置类
    /// </summary>
    public class SmtpSetting
    {
        private string _server;
        public string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        private bool _authentication;
        public bool Authentication
        {
            get { return _authentication; }
            set { _authentication = value; }
        }

        private string _user;
        public string User
        {
            get { return _user; }
            set { _user = value; }
        }

        private string _sender;
        public string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }
}
