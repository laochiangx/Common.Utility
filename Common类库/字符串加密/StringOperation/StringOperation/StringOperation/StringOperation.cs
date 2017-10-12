namespace StringOperation
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Web.Security;

    public class StringOperation
    {
        private static string De(string strCookie, int type)
        {
            string str;
            if ((type % 2) == 0)
            {
                str = DeTransform1(strCookie);
            }
            else
            {
                str = DeTransform3(strCookie);
            }
            return Transform2(strCookie);
        }

        //转换字符(Reverse)
        public static string Decode(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }

        //解密
        public static string Decrypt(string Passowrd)
        {
            return FormsAuthentication.Decrypt(Passowrd).Name.ToString();
        }

        public static string DecryptCookie(string strCookie, int type)
        {
            int i;
            StringBuilder sb = new StringBuilder();
            string[] strarr = new string[0xff];
            int count = strCookie.Length / 4;
            for (i = 0; i < count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    sb.Append(strCookie.Substring((i * 4) + j, 1));
                }
                strarr[i] = sb.ToString();
                sb.Remove(0, sb.Length);
            }
            for (i = 0; i < count; i++)
            {
                char ch = (char) uint.Parse(uint.Parse(strarr[i], NumberStyles.AllowHexSpecifier).ToString("D"));
                sb.Append(ch);
            }
            return De(sb.ToString(), type);
        }

        //解密方式一
        public static string DeTransform1(string str)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();
            foreach (char a in str)
            {
                switch ((i % 6))
                {
                    case 0:
                        sb.Append((char) (a - '\x0001'));
                        break;

                    case 1:
                        sb.Append((char) (a - '\x0005'));
                        break;

                    case 2:
                        sb.Append((char) (a - '\a'));
                        break;

                    case 3:
                        sb.Append((char) (a - '\x0002'));
                        break;

                    case 4:
                        sb.Append((char) (a - '\x0004'));
                        break;

                    case 5:
                        sb.Append((char) (a - '\t'));
                        break;
                }
                i++;
            }
            return sb.ToString();
        }

        //加密方式三
        public static string DeTransform3(string str)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();
            foreach (char a in str)
            {
                switch ((i % 6))
                {
                    case 0:
                        sb.Append((char) (a - '\x0003'));
                        break;

                    case 1:
                        sb.Append((char) (a - '\x0006'));
                        break;

                    case 2:
                        sb.Append((char) (a - '\b'));
                        break;

                    case 3:
                        sb.Append((char) (a - '\a'));
                        break;

                    case 4:
                        sb.Append((char) (a - '\x0005'));
                        break;

                    case 5:
                        sb.Append((char) (a - '\x0002'));
                        break;
                }
                i++;
            }
            return sb.ToString();
        }

        private static string En(string strCookie, int type)
        {
            string str;
            if ((type % 2) == 0)
            {
                str = Transform1(strCookie);
            }
            else
            {
                str = Transform3(strCookie);
            }
            return Transform2(strCookie);
        }

        //转换字符
        public static string Encode(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }

        //加密
        public static string Encrypt(string Password)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(Password, true, 2);
            return FormsAuthentication.Encrypt(ticket).ToString();
        }

        //SHA1加密,MD5加密
        public static string Encrypt(string Password, int Format)
        {
            switch (Format)
            {
                case 0:
                    return FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "SHA1");

                case 1:
                    return FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "MD5");
            }
            return "";
        }

        public static string EncryptCookie(string strCookie, int type)
        {
            string str = En(strCookie, type);
            StringBuilder sb = new StringBuilder();
            foreach (char a in str)
            {
                sb.Append(Convert.ToString((int) a, 0x10).PadLeft(4, '0'));
            }
            return sb.ToString();
        }

        //反转字符
        public static string Reverse(string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = str.Length - 1; i >= 0; i--)
            {
                sb.Append(str[i]);
            }
            return sb.ToString();
        }

        //解密方式一
        public static string Transform1(string str)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();
            foreach (char a in str)
            {
                switch ((i % 6))
                {
                    case 0:
                        sb.Append((char) (a + '\x0001'));
                        break;

                    case 1:
                        sb.Append((char) (a + '\x0005'));
                        break;

                    case 2:
                        sb.Append((char) (a + '\a'));
                        break;

                    case 3:
                        sb.Append((char) (a + '\x0002'));
                        break;

                    case 4:
                        sb.Append((char) (a + '\x0004'));
                        break;

                    case 5:
                        sb.Append((char) (a + '\t'));
                        break;
                }
                i++;
            }
            return sb.ToString();
        }

        public static string Transform2(string str)
        {
            uint j = 0;
            StringBuilder sb = new StringBuilder();
            str = Reverse(str);
            foreach (char a in str)
            {
                j = a;
                if (j > 0xff)
                {
                    j = (uint) ((a >> 8) + ((a & 0xff) << 8));
                }
                else
                {
                    j = (uint) ((a >> 4) + ((a & 15) << 4));
                }
                sb.Append((char) j);
            }
            return sb.ToString();
        }

        //解密方式三
        public static string Transform3(string str)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();
            foreach (char a in str)
            {
                switch ((i % 6))
                {
                    case 0:
                        sb.Append((char) (a + '\x0003'));
                        break;

                    case 1:
                        sb.Append((char) (a + '\x0006'));
                        break;

                    case 2:
                        sb.Append((char) (a + '\b'));
                        break;

                    case 3:
                        sb.Append((char) (a + '\a'));
                        break;

                    case 4:
                        sb.Append((char) (a + '\x0005'));
                        break;

                    case 5:
                        sb.Append((char) (a + '\x0002'));
                        break;
                }
                i++;
            }
            return sb.ToString();
        }
    }
}

