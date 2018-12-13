using Maticsoft.Common.Mime;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
namespace Utilities
{
    public class MailMessageEx : MailMessage
    {
        public const string EmailRegexPattern = "(['\"]{1,}.+['\"]{1,}\\s+)?<?[\\w\\.\\-]+@[^\\.][\\w\\.\\-]+\\.[a-z]{2,}>?";
        private long _octets;
        private int _messageNumber;
        private static readonly char[] AddressDelimiters = new char[]
        {
            ',',
            ';'
        };
        private List<MailMessageEx> _children;
        public long Octets
        {
            get
            {
                return this._octets;
            }
            set
            {
                this._octets = value;
            }
        }
        public int MessageNumber
        {
            get
            {
                return this._messageNumber;
            }
            internal set
            {
                this._messageNumber = value;
            }
        }
        public List<MailMessageEx> Children
        {
            get
            {
                return this._children;
            }
        }
        public DateTime DeliveryDate
        {
            get
            {
                string text = this.GetHeader("date");
                if (string.IsNullOrEmpty(text))
                {
                    return DateTime.MinValue;
                }
                if (text.IndexOf("(EST)") > 1 && text.IndexOf("-") > 1)
                {
                    text = text.Substring(0, text.IndexOf("-"));
                }
                return Convert.ToDateTime(text);
            }
        }
        public MailAddress ReturnAddress
        {
            get
            {
                string header = this.GetHeader("reply-to");
                if (string.IsNullOrEmpty(header))
                {
                    return null;
                }
                return MailMessageEx.CreateMailAddress(header);
            }
        }
        public string Routing
        {
            get
            {
                return this.GetHeader("received");
            }
        }
        public string MessageId
        {
            get
            {
                return this.GetHeader("message-id");
            }
        }
        public string ReplyToMessageId
        {
            get
            {
                return this.GetHeader("in-reply-to", true);
            }
        }
        public string MimeVersion
        {
            get
            {
                return this.GetHeader("mime-version");
            }
        }
        public string ContentId
        {
            get
            {
                return this.GetHeader("content-id");
            }
        }
        public string ContentDescription
        {
            get
            {
                return this.GetHeader("content-description");
            }
        }
        public ContentDisposition ContentDisposition
        {
            get
            {
                string header = this.GetHeader("content-disposition");
                if (string.IsNullOrEmpty(header))
                {
                    return null;
                }
                return new ContentDisposition(header);
            }
        }
        public ContentType ContentType
        {
            get
            {
                string header = this.GetHeader("content-type");
                if (string.IsNullOrEmpty(header))
                {
                    return null;
                }
                return MimeReader.GetContentType(header);
            }
        }
        public MailMessageEx()
        {
            this._children = new List<MailMessageEx>();
        }
        private string GetHeader(string header)
        {
            return this.GetHeader(header, false);
        }
        private string GetHeader(string header, bool stripBrackets)
        {
            if (stripBrackets)
            {
                return MimeEntity.TrimBrackets(base.Headers[header]);
            }
            return base.Headers[header];
        }
        public static MailMessageEx CreateMailMessageFromEntity(MimeEntity entity)
        {
            MailMessageEx mailMessageEx = new MailMessageEx();
            string[] allKeys = entity.Headers.AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                string text = allKeys[i];
                string text2 = entity.Headers[text];
                if (text2.Equals(string.Empty))
                {
                    text2 = " ";
                }
                mailMessageEx.Headers.Add(text.ToLowerInvariant(), text2);
                string a;
                if ((a = text.ToLowerInvariant()) != null)
                {
                    if (!(a == "bcc"))
                    {
                        if (!(a == "cc"))
                        {
                            if (!(a == "from"))
                            {
                                if (!(a == "reply-to"))
                                {
                                    if (!(a == "subject"))
                                    {
                                        if (a == "to")
                                        {
                                            MailMessageEx.PopulateAddressList(text2, mailMessageEx.To);
                                        }
                                    }
                                    else
                                    {
                                        mailMessageEx.Subject = text2;
                                    }
                                }
                                else
                                {
                                    mailMessageEx.ReplyTo = MailMessageEx.CreateMailAddress(text2);
                                }
                            }
                            else
                            {
                                mailMessageEx.From = MailMessageEx.CreateMailAddress(text2);
                            }
                        }
                        else
                        {
                            MailMessageEx.PopulateAddressList(text2, mailMessageEx.CC);
                        }
                    }
                    else
                    {
                        MailMessageEx.PopulateAddressList(text2, mailMessageEx.Bcc);
                    }
                }
            }
            return mailMessageEx;
        }
        public static MailAddress CreateMailAddress(string address)
        {
            MailAddress result;
            try
            {
                result = new MailAddress(address.Trim(new char[]
                {
                    '\t'
                }));
            }
            catch
            {
                result = new MailAddress(address + "@mail.error");
            }
            return result;
        }
        public static void PopulateAddressList(string addressList, MailAddressCollection recipients)
        {
            foreach (MailAddress current in MailMessageEx.GetMailAddresses(addressList))
            {
                recipients.Add(current);
            }
        }
        private static IEnumerable<MailAddress> GetMailAddresses(string addressList)
        {
            Regex regex = new Regex("(['\"]{1,}.+['\"]{1,}\\s+)?<?[\\w\\.\\-]+@[^\\.][\\w\\.\\-]+\\.[a-z]{2,}>?");
            foreach (Match match in regex.Matches(addressList))
            {
                yield return MailMessageEx.CreateMailAddress(match.Value);
            }
            yield break;
        }
    }
}
