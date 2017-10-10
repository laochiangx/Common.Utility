using System.IO;

namespace DotNet.Utilities
{
    /// <summary>
    /// This command represents the Pop3 RSET command.
    /// </summary>
    internal sealed class RsetCommand : Pop3Command<Pop3Response>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RsetCommand"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public RsetCommand(Stream stream)
            : base(stream, false, Pop3State.Transaction) { }

        /// <summary>
        /// Creates the RSET request message.
        /// </summary>
        /// <returns>
        /// The byte[] containing the RSET request message.
        /// </returns>
        protected override byte[] CreateRequestMessage()
        {
            return GetRequestMessage(Pop3Commands.Rset);
        }
    }
}
