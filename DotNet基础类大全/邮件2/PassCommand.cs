using System;
using System.IO;


namespace DotNet.Utilities
{
    /// <summary>
    /// This class represents the Pop3 PASS command.
    /// </summary>
    internal sealed class PassCommand : Pop3Command<Pop3Response>
    {
        private string _password;

        /// <summary>
        /// Initializes a new instance of the <see cref="PassCommand"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="password">The password.</param>
        public PassCommand(Stream stream, string password)
            : base(stream, false, Pop3State.Authorization)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            _password = password;
        }

        /// <summary>
        /// Creates the PASS request message.
        /// </summary>
        /// <returns>
        /// The byte[] containing the PASS request message.
        /// </returns>
        protected override byte[] CreateRequestMessage()
        {
            return GetRequestMessage(Pop3Commands.Pass, _password, Pop3Commands.Crlf);
        }
    }
}
