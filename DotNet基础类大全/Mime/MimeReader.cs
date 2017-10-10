using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;

namespace DotNet.Utilities
{
    /// <summary>
    /// This class is responsible for parsing a string array of lines
    /// containing a MIME message.
    /// </summary>
    public class MimeReader
    {
        private static readonly char[] HeaderWhitespaceChars = new char[] { ' ', '\t' };

        private Queue<string> _lines;
        /// <summary>
        /// Gets the lines.
        /// </summary>
        /// <value>The lines.</value>
        public Queue<string> Lines
        {
            get
            {
                return _lines;
            }
        }

        private MimeEntity _entity;

        /// <summary>
        /// Initializes a new instance of the <see cref="MimeReader"/> class.
        /// </summary>
        private MimeReader()
        {
            _entity = new MimeEntity();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MimeReader"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="lines">The lines.</param>
        private MimeReader(MimeEntity entity, Queue<string> lines)
            : this()
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (lines == null)
            {
                throw new ArgumentNullException("lines");
            }

            _lines = lines;
            _entity = new MimeEntity(entity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MimeReader"/> class.
        /// </summary>
        /// <param name="lines">The lines.</param>
        public MimeReader(string[] lines)
            : this()
        {
            if (lines == null)
            {
                throw new ArgumentNullException("lines");
            }

            _lines = new Queue<string>(lines);
        }

        /// <summary>
        /// Parse headers into _entity.Headers NameValueCollection.
        /// </summary>
        private int ParseHeaders()
        {
            string lastHeader = string.Empty;
            string line = string.Empty;
            // the first empty line is the end of the headers.
            while (_lines.Count > 0 && !string.IsNullOrEmpty(_lines.Peek()))
            {
                line = _lines.Dequeue();

                //if a header line starts with a space or tab then it is a continuation of the
                //previous line.
                if (line.StartsWith(" ") || line.StartsWith(Convert.ToString('\t')))
                {
                    _entity.Headers[lastHeader] = string.Concat(_entity.Headers[lastHeader], line);
                    continue;
                }

                int separatorIndex = line.IndexOf(':');

                if (separatorIndex < 0)
                {
                    System.Diagnostics.Debug.WriteLine("Invalid header:{0}", line);
                    continue;
                }  //This is an invalid header field.  Ignore this line.

                string headerName = line.Substring(0, separatorIndex);
                string headerValue = line.Substring(separatorIndex + 1).Trim(HeaderWhitespaceChars);

                _entity.Headers.Add(headerName.ToLower(), headerValue);
                lastHeader = headerName;
            }

            if (_lines.Count > 0)
            {
                _lines.Dequeue();
            } //remove closing header CRLF.

            return _entity.Headers.Count;
        }

        /// <summary>
        /// Processes mime specific headers.
        /// </summary>
        /// <returns>A mime entity with mime specific headers parsed.</returns>
        private void ProcessHeaders()
        {
            foreach (string key in _entity.Headers.AllKeys)
            {
                switch (key)
                {
                    case "content-description":
                        _entity.ContentDescription = _entity.Headers[key];
                        break;
                    case "content-disposition":
                        _entity.ContentDisposition = new ContentDisposition(_entity.Headers[key]);
                        break;
                    case "content-id":
                        _entity.ContentId = _entity.Headers[key];
                        break;
                    case "content-transfer-encoding":
                        _entity.TransferEncoding = _entity.Headers[key];
                        _entity.ContentTransferEncoding = MimeReader.GetTransferEncoding(_entity.Headers[key]);
                        break;
                    case "content-type":
                        _entity.SetContentType(MimeReader.GetContentType(_entity.Headers[key]));
                        break;
                    case "mime-version":
                        _entity.MimeVersion = _entity.Headers[key];
                        break;
                }
            }
        }

        /// <summary>
        /// Creates the MIME entity.
        /// </summary>
        /// <returns>A mime entity containing 0 or more children representing the mime message.</returns>
        public MimeEntity CreateMimeEntity()
        {
            try
            {
                ParseHeaders();

                ProcessHeaders();

                ParseBody();

                SetDecodedContentStream();

                return _entity;
            }
            catch
            {
                return null;

            }
        }


        /// <summary>
        /// Sets the decoded content stream by decoding the EncodedMessage 
        /// and writing it to the entity content stream.
        /// </summary>
        /// <param name="entity">The entity containing the encoded message.</param>
        private void SetDecodedContentStream()
        {
            switch (_entity.ContentTransferEncoding)
            {
                case System.Net.Mime.TransferEncoding.Base64:
                    _entity.Content = new MemoryStream(Convert.FromBase64String(_entity.EncodedMessage.ToString()), false);
                    break;

                case System.Net.Mime.TransferEncoding.QuotedPrintable:
                    _entity.Content = new MemoryStream(GetBytes(QuotedPrintableEncoding.Decode(_entity.EncodedMessage.ToString())), false);
                    break;

                case System.Net.Mime.TransferEncoding.SevenBit:
                default:
                    _entity.Content = new MemoryStream(GetBytes(_entity.EncodedMessage.ToString()), false);
                    break;
            }
        }

        /// <summary>
        /// Gets a byte[] of content for the provided string.
        /// </summary>
        /// <param name="decodedContent">Content.</param>
        /// <returns>A byte[] containing content.</returns>
        private byte[] GetBytes(string content)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(content);
                }
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Parses the body.
        /// </summary>
        private void ParseBody()
        {
            if (_entity.HasBoundary)
            {
                while (_lines.Count > 0
                    && !string.Equals(_lines.Peek(), _entity.EndBoundary))
                {
                    /*Check to verify the current line is not the same as the parent starting boundary.  
                       If it is the same as the parent starting boundary this indicates existence of a 
                       new child entity. Return and process the next child.*/
                    if (_entity.Parent != null
                        && string.Equals(_entity.Parent.StartBoundary, _lines.Peek()))
                    {
                        return;
                    }

                    if (string.Equals(_lines.Peek(), _entity.StartBoundary))
                    {
                        AddChildEntity(_entity, _lines);
                    } //Parse a new child mime part.
                    else if (string.Equals(_entity.ContentType.MediaType, MediaTypes.MessageRfc822, StringComparison.InvariantCultureIgnoreCase)
                        && string.Equals(_entity.ContentDisposition.DispositionType, DispositionTypeNames.Attachment, StringComparison.InvariantCultureIgnoreCase))
                    {
                        /*If the content type is message/rfc822 the stop condition to parse headers has already been encountered.
                         But, a content type of message/rfc822 would have the message headers immediately following the mime
                         headers so we need to parse the headers for the attached message now.  This is done by creating
                         a new child entity.*/
                        AddChildEntity(_entity, _lines);

                        break;
                    }
                    else
                    {
                        _entity.EncodedMessage.Append(string.Concat(_lines.Dequeue(), Pop3Commands.Crlf));
                    } //Append the message content.
                }
            } //Parse a multipart message.
            else
            {
                while (_lines.Count > 0)
                {
                    _entity.EncodedMessage.Append(string.Concat(_lines.Dequeue(), Pop3Commands.Crlf));
                }
            } //Parse a single part message.
        }

        /// <summary>
        /// Adds the child entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        private void AddChildEntity(MimeEntity entity, Queue<string> lines)
        {
            /*if (entity == null)
            {
                return;
            }

            if (lines == null)
            {
                return;
            }*/

            MimeReader reader = new MimeReader(entity, lines);
            entity.Children.Add(reader.CreateMimeEntity());
        }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns></returns>
        public static ContentType GetContentType(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                contentType = "text/plain; charset=us-ascii";
            }
            return new ContentType(contentType);

        }

        /// <summary>
        /// Gets the type of the media.
        /// </summary>
        /// <param name="mediaType">Type of the media.</param>
        /// <returns></returns>
        public static string GetMediaType(string mediaType)
        {
            if (string.IsNullOrEmpty(mediaType))
            {
                return "text/plain";
            }
            return mediaType.Trim();
        }

        /// <summary>
        /// Gets the type of the media main.
        /// </summary>
        /// <param name="mediaType">Type of the media.</param>
        /// <returns></returns>
        public static string GetMediaMainType(string mediaType)
        {
            int separatorIndex = mediaType.IndexOf('/');
            if (separatorIndex < 0)
            {
                return mediaType;
            }
            else
            {
                return mediaType.Substring(0, separatorIndex);
            }
        }

        /// <summary>
        /// Gets the type of the media sub.
        /// </summary>
        /// <param name="mediaType">Type of the media.</param>
        /// <returns></returns>
        public static string GetMediaSubType(string mediaType)
        {
            int separatorIndex = mediaType.IndexOf('/');
            if (separatorIndex < 0)
            {
                if (mediaType.Equals("text"))
                {
                    return "plain";
                }
                return string.Empty;
            }
            else
            {
                if (mediaType.Length > separatorIndex)
                {
                    return mediaType.Substring(separatorIndex + 1);
                }
                else
                {
                    string mainType = GetMediaMainType(mediaType);
                    if (mainType.Equals("text"))
                    {
                        return "plain";
                    }
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the transfer encoding.
        /// </summary>
        /// <param name="transferEncoding">The transfer encoding.</param>
        /// <returns></returns>
        /// <remarks>
        /// The transfer encoding determination follows the same rules as 
        /// Peter Huber's article w/ the exception of not throwing exceptions 
        /// when binary is provided as a transferEncoding.  Instead it is left
        /// to the calling code to check for binary.
        /// </remarks>
        public static TransferEncoding GetTransferEncoding(string transferEncoding)
        {
            switch (transferEncoding.Trim().ToLowerInvariant())
            {
                case "7bit":
                case "8bit":
                    return System.Net.Mime.TransferEncoding.SevenBit;
                case "quoted-printable":
                    return System.Net.Mime.TransferEncoding.QuotedPrintable;
                case "base64":
                    return System.Net.Mime.TransferEncoding.Base64;
                case "binary":
                default:
                    return System.Net.Mime.TransferEncoding.Unknown;

            }
        }
    }
}
