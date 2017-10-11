using System;

namespace DotNet.Utilities
{
    /// <summary>
    /// This class represents a RETR response message resulting
    /// from a Pop3 RETR command execution against a Pop3 Server.
    /// </summary>
    internal sealed class RetrResponse : Pop3Response
    {
        private string[] _messageLines;
        /// <summary>
        /// Gets the message lines.
        /// </summary>
        /// <value>The Pop3 message lines.</value>
        public string[] MessageLines
        {
            get { return _messageLines; }
        }

        private long _octects;
        public long Octets
        {
            get
            {
                return _octects;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetrResponse"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="messageLines">The message lines.</param>
        public RetrResponse(Pop3Response response, string[] messageLines)
            : base(response.ResponseContents, response.HostMessage, response.StatusIndicator)
        {
            if (messageLines == null)
            {
                throw new ArgumentNullException("messageLines");
            }
            string[] values = response.HostMessage.Split(' ');
            if (values.Length == 2)
            {
                _octects = Convert.ToInt64(values[1]);
            }

            _messageLines = messageLines;
        }
    }
}
