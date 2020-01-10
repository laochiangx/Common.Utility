using Maticsoft.Common.Mail;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Utilities;

namespace Maticsoft.Common.Mime
{
    public class MimeEntity
    {
        private StringBuilder _encodedMessage;
        private List<MimeEntity> _children;
        private ContentType _contentType;
        private string _mediaSubType;
        private string _mediaMainType;
        private NameValueCollection _headers;
        private string _mimeVersion;
        private string _contentId;
        private string _contentDescription;
        private ContentDisposition _contentDisposition;
        private string _transferEncoding;
        private TransferEncoding _contentTransferEncoding;
        private string _startBoundary;
        private MimeEntity _parent;
        private MemoryStream _content;
        public StringBuilder EncodedMessage
        {
            get
            {
                return this._encodedMessage;
            }
        }
        public List<MimeEntity> Children
        {
            get
            {
                return this._children;
            }
        }
        public ContentType ContentType
        {
            get
            {
                return this._contentType;
            }
        }
        public string MediaSubType
        {
            get
            {
                return this._mediaSubType;
            }
        }
        public string MediaMainType
        {
            get
            {
                return this._mediaMainType;
            }
        }
        public NameValueCollection Headers
        {
            get
            {
                return this._headers;
            }
        }
        public string MimeVersion
        {
            get
            {
                return this._mimeVersion;
            }
            set
            {
                this._mimeVersion = value;
            }
        }
        public string ContentId
        {
            get
            {
                return this._contentId;
            }
            set
            {
                this._contentId = value;
            }
        }
        public string ContentDescription
        {
            get
            {
                return this._contentDescription;
            }
            set
            {
                this._contentDescription = value;
            }
        }
        public ContentDisposition ContentDisposition
        {
            get
            {
                return this._contentDisposition;
            }
            set
            {
                this._contentDisposition = value;
            }
        }
        public string TransferEncoding
        {
            get
            {
                return this._transferEncoding;
            }
            set
            {
                this._transferEncoding = value;
            }
        }
        public TransferEncoding ContentTransferEncoding
        {
            get
            {
                return this._contentTransferEncoding;
            }
            set
            {
                this._contentTransferEncoding = value;
            }
        }
        internal bool HasBoundary
        {
            get
            {
                return !string.IsNullOrEmpty(this._contentType.Boundary) || !string.IsNullOrEmpty(this._startBoundary);
            }
        }
        public string StartBoundary
        {
            get
            {
                if (string.IsNullOrEmpty(this._startBoundary) || !string.IsNullOrEmpty(this._contentType.Boundary))
                {
                    return "--" + this._contentType.Boundary;
                }
                return this._startBoundary;
            }
        }
        public string EndBoundary
        {
            get
            {
                return this.StartBoundary + "--";
            }
        }
        public MimeEntity Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                this._parent = value;
            }
        }
        public MemoryStream Content
        {
            get
            {
                return this._content;
            }
            internal set
            {
                this._content = value;
            }
        }
        public MimeEntity()
        {
            this._children = new List<MimeEntity>();
            this._headers = new NameValueCollection();
            this._contentType = MimeReader.GetContentType(string.Empty);
            this._parent = null;
            this._encodedMessage = new StringBuilder();
        }
        public MimeEntity(MimeEntity parent) : this()
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            this._parent = parent;
            this._startBoundary = parent.StartBoundary;
        }
        internal void SetContentType(ContentType contentType)
        {
            this._contentType = contentType;
            this._contentType.MediaType = MimeReader.GetMediaType(contentType.MediaType);
            this._mediaMainType = MimeReader.GetMediaMainType(contentType.MediaType);
            this._mediaSubType = MimeReader.GetMediaSubType(contentType.MediaType);
        }
        public MailMessageEx ToMailMessageEx()
        {
            return this.ToMailMessageEx(this);
        }
        private MailMessageEx ToMailMessageEx(MimeEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            MailMessageEx mailMessageEx = MailMessageEx.CreateMailMessageFromEntity(entity);
            if (!string.IsNullOrEmpty(entity.ContentType.Boundary))
            {
                mailMessageEx = MailMessageEx.CreateMailMessageFromEntity(entity);
                this.BuildMultiPartMessage(entity, mailMessageEx);
            }
            else
            {
                if (string.Equals(entity.ContentType.MediaType, MediaTypes.MessageRfc822, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (entity.Children.Count < 0)
                    {
                        throw new Pop3Exception("Invalid child count on message/rfc822 entity.");
                    }
                    mailMessageEx = MailMessageEx.CreateMailMessageFromEntity(entity.Children[0]);
                    this.BuildMultiPartMessage(entity, mailMessageEx);
                }
                else
                {
                    mailMessageEx = MailMessageEx.CreateMailMessageFromEntity(entity);
                    this.BuildSinglePartMessage(entity, mailMessageEx);
                }
            }
            return mailMessageEx;
        }
        private void BuildSinglePartMessage(MimeEntity entity, MailMessageEx message)
        {
            this.SetMessageBody(message, entity);
        }
        public Encoding GetEncoding()
        {
            if (string.IsNullOrEmpty(this.ContentType.CharSet))
            {
                return Encoding.ASCII;
            }
            Encoding result;
            try
            {
                result = Encoding.GetEncoding(this.ContentType.CharSet);
            }
            catch (ArgumentException)
            {
                result = Encoding.ASCII;
            }
            return result;
        }
        private void BuildMultiPartMessage(MimeEntity entity, MailMessageEx message)
        {
            foreach (MimeEntity current in entity.Children)
            {
                if (current != null)
                {
                    if (string.Equals(current.ContentType.MediaType, MediaTypes.MultipartAlternative, StringComparison.InvariantCultureIgnoreCase) || string.Equals(current.ContentType.MediaType, MediaTypes.MultipartMixed, StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.BuildMultiPartMessage(current, message);
                    }
                    else
                    {
                        if (!MimeEntity.IsAttachment(current) && (string.Equals(current.ContentType.MediaType, MediaTypes.TextPlain) || string.Equals(current.ContentType.MediaType, MediaTypes.TextHtml)))
                        {
                            message.AlternateViews.Add(this.CreateAlternateView(current));
                            this.SetMessageBody(message, current);
                        }
                        else
                        {
                            if (string.Equals(current.ContentType.MediaType, MediaTypes.MessageRfc822, StringComparison.InvariantCultureIgnoreCase) && string.Equals(current.ContentDisposition.DispositionType, "attachment", StringComparison.InvariantCultureIgnoreCase))
                            {
                                message.Children.Add(this.ToMailMessageEx(current));
                            }
                            else
                            {
                                if (MimeEntity.IsAttachment(current))
                                {
                                    message.Attachments.Add(this.CreateAttachment(current));
                                }
                            }
                        }
                    }
                }
            }
        }
        private static bool IsAttachment(MimeEntity child)
        {
            return child.ContentDisposition != null && string.Equals(child.ContentDisposition.DispositionType, "attachment", StringComparison.InvariantCultureIgnoreCase);
        }
        private void SetMessageBody(MailMessageEx message, MimeEntity child)
        {
            Encoding encoding = child.GetEncoding();
            message.Body = this.DecodeBytes(child.Content.ToArray(), encoding);
            message.BodyEncoding = encoding;
            message.IsBodyHtml = string.Equals(MediaTypes.TextHtml, child.ContentType.MediaType, StringComparison.InvariantCultureIgnoreCase);
        }
        private string DecodeBytes(byte[] buffer, Encoding encoding)
        {
            if (buffer == null)
            {
                return null;
            }
            if (encoding == null)
            {
                encoding = Encoding.UTF7;
            }
            return encoding.GetString(buffer);
        }
        private AlternateView CreateAlternateView(MimeEntity view)
        {
            return new AlternateView(view.Content, view.ContentType)
            {
                TransferEncoding = view.ContentTransferEncoding,
                ContentId = MimeEntity.TrimBrackets(view.ContentId)
            };
        }
        public static string TrimBrackets(string value)
        {
            if (value == null)
            {
                return value;
            }
            if (value.StartsWith("<") && value.EndsWith(">"))
            {
                return value.Trim(new char[]
                {
                    '<',
                    '>'
                });
            }
            return value;
        }
        private Attachment CreateAttachment(MimeEntity entity)
        {
            Attachment attachment = new Attachment(entity.Content, entity.ContentType);
            if (entity.ContentDisposition != null)
            {
                attachment.ContentDisposition.Parameters.Clear();
                foreach (string key in entity.ContentDisposition.Parameters.Keys)
                {
                    attachment.ContentDisposition.Parameters.Add(key, entity.ContentDisposition.Parameters[key]);
                }
                attachment.ContentDisposition.CreationDate = entity.ContentDisposition.CreationDate;
                attachment.ContentDisposition.DispositionType = entity.ContentDisposition.DispositionType;
                attachment.ContentDisposition.FileName = entity.ContentDisposition.FileName;
                attachment.ContentDisposition.Inline = entity.ContentDisposition.Inline;
                attachment.ContentDisposition.ModificationDate = entity.ContentDisposition.ModificationDate;
                attachment.ContentDisposition.ReadDate = entity.ContentDisposition.ReadDate;
                attachment.ContentDisposition.Size = entity.ContentDisposition.Size;
            }
            if (!string.IsNullOrEmpty(entity.ContentId))
            {
                attachment.ContentId = MimeEntity.TrimBrackets(entity.ContentId);
            }
            attachment.TransferEncoding = entity.ContentTransferEncoding;
            return attachment;
        }
    }
}