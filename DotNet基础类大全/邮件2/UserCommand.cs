using System;
using System.IO;


namespace DotNet.Utilities
{
    /// <summary>
    /// This command represents a Pop3 USER command.
    /// </summary>
    internal sealed class UserCommand : Pop3Command<Pop3Response>
    {
        private string _username;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCommand"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="username">The username.</param>
        public UserCommand(Stream stream, string username)
            : base(stream, false, Pop3State.Authorization)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }

            _username = username;
        }

        /// <summary>
        /// Creates the USER request message.
        /// </summary>
        /// <returns>
        /// The byte[] containing the USER request message.
        /// </returns>
        protected override byte[] CreateRequestMessage()
        {
            return GetRequestMessage(Pop3Commands.User, _username, Pop3Commands.Crlf);
        }
    }
}
