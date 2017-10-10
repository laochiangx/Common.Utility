using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Net.Mime;
using System.Net.Mail;

namespace DotNet.Utilities
{
    /// <summary>
    /// This class represents a Mime entity.
    /// 这个类表示一个MIME实体
    /// </summary>
    public class MimeEntity
    {
        private StringBuilder _encodedMessage;
        /// <summary>
        /// Gets the encoded message.
        /// 获取编码的消息。
        /// </summary>
        /// <value>
        /// 编码的消息
        /// The encoded message.
        /// </value>
        public StringBuilder EncodedMessage
        {
            get { return _encodedMessage; }
        }


        private List<MimeEntity> _children;

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        public List<MimeEntity> Children
        {
            get
            {
                return _children;
            }
        }

        private ContentType _contentType;
        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public ContentType ContentType
        {
            get { return _contentType; }
        }

        private string _mediaSubType;
        /// <summary>
        /// Gets the type of the media sub.
        /// </summary>
        /// <value>The type of the media sub.</value>
        public string MediaSubType
        {
            get { return _mediaSubType; }
        }

        private string _mediaMainType;
        /// <summary>
        /// Gets the type of the media main.
        /// </summary>
        /// <value>The type of the media main.</value>
        public string MediaMainType
        {
            get { return _mediaMainType; }
        }

        private NameValueCollection _headers;
        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>The headers.</value>
        public NameValueCollection Headers
        {
            get { return _headers; }
        }

        private string _mimeVersion;
        /// <summary>
        /// Gets or sets the MIME version.
        /// </summary>
        /// <value>The MIME version.</value>
        public string MimeVersion
        {
            get
            {
                return _mimeVersion;
            }
            set
            {
                _mimeVersion = value;
            }
        }

        private string _contentId;
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>The content id.</value>
        public string ContentId
        {
            get
            {
                return _contentId;
            }
            set
            {
                _contentId = value;
            }
        }

        private string _contentDescription;
        /// <summary>
        /// Gets or sets the content description.
        /// </summary>
        /// <value>The content description.</value>
        public string ContentDescription
        {
            get
            {
                return _contentDescription;
            }
            set
            {
                _contentDescription = value;
            }
        }

        private ContentDisposition _contentDisposition;
        /// <summary>
        /// Gets or sets the content disposition.
        /// </summary>
        /// <value>The content disposition.</value>
        public ContentDisposition ContentDisposition
        {
            get
            {
                return _contentDisposition;
            }
            set
            {
                _contentDisposition = value;
            }
        }

        private string _transferEncoding;
        /// <summary>
        /// Gets or sets the transfer encoding.
        /// </summary>
        /// <value>The transfer encoding.</value>
        public string TransferEncoding
        {
            get
            {
                return _transferEncoding;
            }
            set
            {
                _transferEncoding = value;
            }
        }

        private TransferEncoding _contentTransferEncoding;
        /// <summary>
        /// Gets or sets the content transfer encoding.
        /// </summary>
        /// <value>The content transfer encoding.</value>
        public TransferEncoding ContentTransferEncoding
        {
            get
            {
                return _contentTransferEncoding;
            }
            set
            {
                _contentTransferEncoding = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has boundary.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has boundary; otherwise, <c>false</c>.
        /// </value>
        internal bool HasBoundary
        {
            get
            {
                return (!string.IsNullOrEmpty(_contentType.Boundary))
                    || (!string.IsNullOrEmpty(_startBoundary));
            }
        }

        private string _startBoundary;
        /// <summary>
        /// Gets the start boundary.
        /// </summary>
        /// <value>The start boundary.</value>
        public string StartBoundary
        {
            get
            {
                if (string.IsNullOrEmpty(_startBoundary) || !string.IsNullOrEmpty(_contentType.Boundary))
                {
                    return string.Concat("--", _contentType.Boundary);
                }

                return _startBoundary;
            }
        }

        /// <summary>
        /// Gets the end boundary.
        /// </summary>
        /// <value>The end boundary.</value>
        public string EndBoundary
        {
            get
            {
                return string.Concat(StartBoundary, "--");
            }
        }

        private MimeEntity _parent;
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public MimeEntity Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        private MemoryStream _content;

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public MemoryStream Content
        {
            get { return _content; }
            internal set { _content = value; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MimeEntity"/> class.
        /// </summary>
        public MimeEntity()
        {
            _children = new List<MimeEntity>();
            _headers = new NameValueCollection();
            _contentType = MimeReader.GetContentType(string.Empty);
            _parent = null;
            _encodedMessage = new StringBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MimeEntity"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public MimeEntity(MimeEntity parent)
            : this()
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            _parent = parent;
            _startBoundary = parent.StartBoundary;
        }

        /// <summary>
        /// Sets the type of the content.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        internal void SetContentType(ContentType contentType)
        {
            _contentType = contentType;
            _contentType.MediaType = MimeReader.GetMediaType(contentType.MediaType);
            _mediaMainType = MimeReader.GetMediaMainType(contentType.MediaType);
            _mediaSubType = MimeReader.GetMediaSubType(contentType.MediaType);
        }

        /// <summary>
        /// Toes the mail message ex.
        /// </summary>
        /// <returns></returns>
        public MailMessageEx ToMailMessageEx()
        {
            return ToMailMessageEx(this);
        }

        /// <summary>
        /// Toes the mail message ex.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        private MailMessageEx ToMailMessageEx(MimeEntity entity)
        {
            if (entity == null)
            {
                //throw new ArgumentNullException("entity");
                return null;
            }

            //parse standard headers and create base email.
            MailMessageEx message = MailMessageEx.CreateMailMessageFromEntity(entity);

            if (!string.IsNullOrEmpty(entity.ContentType.Boundary))
            {
                message = MailMessageEx.CreateMailMessageFromEntity(entity);
                BuildMultiPartMessage(entity, message);
            }//parse multipart message into sub parts.
            else if (string.Equals(entity.ContentType.MediaType, MediaTypes.MessageRfc822, StringComparison.InvariantCultureIgnoreCase))
            {
                //use the first child to create the multipart message.
                if (entity.Children.Count < 0)
                {
                    throw new Pop3Exception("Invalid child count on message/rfc822 entity.");
                }

                //create the mail message from the first child because it will
                //contain all of the mail headers.  The entity in this state
                //only contains simple content type headers indicating, disposition, type and description.
                //This means we can't create the mail message from this type as there is no 
                //internet mail headers attached to this entity.
                message = MailMessageEx.CreateMailMessageFromEntity(entity.Children[0]);
                BuildMultiPartMessage(entity, message);
            } //parse nested message.
            else
            {
                message = MailMessageEx.CreateMailMessageFromEntity(entity);
                BuildSinglePartMessage(entity, message);
            } //Create single part message.

            return message;
        }

        /// <summary>
        /// Builds the single part message.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        private void BuildSinglePartMessage(MimeEntity entity, MailMessageEx message)
        {
            SetMessageBody(message, entity);
        }


        /// <summary>
        /// Gets the body encoding.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public Encoding GetEncoding()
        {
            if (string.IsNullOrEmpty(this.ContentType.CharSet))
            {
                return Encoding.ASCII;
            }
            else
            {
                try
                {
                    return Encoding.GetEncoding(this.ContentType.CharSet);
                }
                catch (ArgumentException)
                {
                    return Encoding.ASCII;
                }
            }
        }

        /// <summary>
        /// Builds the multi part message.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        private void BuildMultiPartMessage(MimeEntity entity, MailMessageEx message)
        {
            foreach (MimeEntity child in entity.Children)
            {
                if (child == null)
                {
                    continue;
                }
                if (string.Equals(child.ContentType.MediaType, MediaTypes.MultipartAlternative, StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(child.ContentType.MediaType, MediaTypes.MultipartMixed, StringComparison.InvariantCultureIgnoreCase))
                {
                    BuildMultiPartMessage(child, message);
                }  //if the message is mulitpart/alternative or multipart/mixed then the entity will have children needing parsed.
                else if (!IsAttachment(child) &&
                    (string.Equals(child.ContentType.MediaType, MediaTypes.TextPlain)
                    || string.Equals(child.ContentType.MediaType, MediaTypes.TextHtml)))
                {
                    message.AlternateViews.Add(CreateAlternateView(child));
                    SetMessageBody(message, child);

                } //add the alternative views.
                else if (string.Equals(child.ContentType.MediaType, MediaTypes.MessageRfc822, StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(child.ContentDisposition.DispositionType, DispositionTypeNames.Attachment, StringComparison.InvariantCultureIgnoreCase))
                {
                    message.Children.Add(ToMailMessageEx(child));
                } //create a child message and 
                else if (IsAttachment(child))
                {
                    message.Attachments.Add(CreateAttachment(child));

                }
            }
        }

        private static bool IsAttachment(MimeEntity child)
        {
            return (child.ContentDisposition != null)
                && (string.Equals(child.ContentDisposition.DispositionType, DispositionTypeNames.Attachment, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Sets the message body.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="child">The child.</param>
        private void SetMessageBody(MailMessageEx message, MimeEntity child)
        {
            Encoding encoding = child.GetEncoding();
            message.Body = DecodeBytes(child.Content.ToArray(), encoding);
            message.BodyEncoding = encoding;
            message.IsBodyHtml = string.Equals(MediaTypes.TextHtml,
                child.ContentType.MediaType, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Decodes the bytes.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        private string DecodeBytes(byte[] buffer, Encoding encoding)
        {
            if (buffer == null)
            {
                return null;
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF7;
            }  //email defaults to 7bit.  

            return encoding.GetString(buffer);
        }

        /// <summary>
        /// Creates the alternate view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns></returns>
        private AlternateView CreateAlternateView(MimeEntity view)
        {
            AlternateView alternateView = new AlternateView(view.Content, view.ContentType);
            alternateView.TransferEncoding = view.ContentTransferEncoding;
            alternateView.ContentId = TrimBrackets(view.ContentId);
            return alternateView;
        }

        /// <summary>
        /// Trims the brackets.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string TrimBrackets(string value)
        {
            if (value == null)
            {
                return value;
            }

            if (value.StartsWith("<") && value.EndsWith(">"))
            {
                return value.Trim('<', '>');
            }

            return value;
        }

        /// <summary>
        /// Creates the attachment.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
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
                attachment.ContentId = TrimBrackets(entity.ContentId);
            }

            attachment.TransferEncoding = entity.ContentTransferEncoding;

            return attachment;
        }
    }
}
