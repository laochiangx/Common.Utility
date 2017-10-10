using System;
//using System.Net.Sockets;
using System.IO;

namespace DotNet.Utilities
{
    /// <summary>
    /// This class represents the Pop3 DELE command.
    /// </summary>
    internal sealed class DeleCommand : Pop3Command<Pop3Response>
    {
        int _messageId = int.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleCommand"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="messageId">The message id.</param>
        public DeleCommand(Stream stream, int messageId)
            : base(stream, false, Pop3State.Transaction)
        {
            if (messageId < 0)
            {
                throw new ArgumentOutOfRangeException("_messageId");
            }
            _messageId = messageId;
        }

        /// <summary>
        /// Creates the DELE request message.
        /// </summary>
        /// <returns>
        /// The byte[] containing the DELE request message.
        /// </returns>
        protected override byte[] CreateRequestMessage()
        {
            return GetRequestMessage(string.Concat(Pop3Commands.Dele, _messageId.ToString(), Pop3Commands.Crlf));
        }
    }
}
