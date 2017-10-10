using System;
using System.IO;


namespace DotNet.Utilities
{
    /// <summary>
    /// This class represents the Pop3 STAT command.
    /// </summary>
    internal sealed class StatCommand : Pop3Command<StatResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommand"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StatCommand(Stream stream)
            : base(stream, false, Pop3State.Transaction) { }

        /// <summary>
        /// Creates the STAT request message.
        /// </summary>
        /// <returns>
        /// The byte[] containing the STAT request message.
        /// </returns>
        protected override byte[] CreateRequestMessage()
        {
            return GetRequestMessage(Pop3Commands.Stat);
        }

        /// <summary>
        /// Creates the response.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>
        /// The <c>Pop3Response</c> containing the results of the
        /// Pop3 command execution.
        /// </returns>
        protected override StatResponse CreateResponse(byte[] buffer)
        {
            Pop3Response response = Pop3Response.CreateResponse(buffer);
            string[] values = response.HostMessage.Split(' ');

            //should consist of '+OK', 'messagecount', 'octets'
            if (values.Length < 3)
            {
                throw new Pop3Exception(string.Concat("Invalid response message: ", response.HostMessage));
            }

            int messageCount = Convert.ToInt32(values[1]);
            long octets = Convert.ToInt64(values[2]);

            return new StatResponse(response, messageCount, octets);
        }
    }
}
