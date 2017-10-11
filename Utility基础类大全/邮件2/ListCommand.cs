using System;
using System.Collections.Generic;
using System.IO;


namespace DotNet.Utilities
{
    /// <summary>
    /// This class represents both the multiline and single line Pop3 LIST command.
    /// </summary>
    internal sealed class ListCommand : Pop3Command<ListResponse>
    {
        // the id of the message on the server to retrieve.
        int _messageId;

        public ListCommand(Stream stream)
            : base(stream, true, Pop3State.Transaction)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommand"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="messageId">The message id.</param>
        public ListCommand(Stream stream, int messageId)
            : this(stream)
        {
            if (messageId < 0)
            {
                throw new ArgumentOutOfRangeException("messageId");
            }

            _messageId = messageId;

            base.IsMultiline = false;
        }

        /// <summary>
        /// Creates the LIST request message.
        /// </summary>
        /// <returns>The byte[] containing the LIST request message.</returns>
        protected override byte[] CreateRequestMessage()
        {
            string requestMessage = Pop3Commands.List;

            if (!IsMultiline)
            {
                requestMessage += _messageId.ToString();
            } // Append the message id to perform the LIST command for.

            return GetRequestMessage(requestMessage, Pop3Commands.Crlf);
        }

        /// <summary>
        /// Creates the response.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>A <c>ListResponse</c> containing the results of the Pop3 LIST command.</returns>
        protected override ListResponse CreateResponse(byte[] buffer)
        {
            Pop3Response response = Pop3Response.CreateResponse(buffer);

            List<Pop3ListItem> items;

            if (IsMultiline)
            {
                items = new List<Pop3ListItem>();
                string[] values;
                string[] lines = GetResponseLines(StripPop3HostMessage(buffer, response.HostMessage));

                foreach (string line in lines)
                {
                    //each line should consist of 'n m' where n is the message number and m is the number of octets
                    values = line.Split(' ');
                    if (values.Length < 2)
                    {
                        throw new Pop3Exception(string.Concat("Invalid line in multiline response:  ", line));
                    }

                    items.Add(new Pop3ListItem(Convert.ToInt32(values[0]),
                        Convert.ToInt64(values[1])));
                }
            } //Parse the multiline response.
            else
            {
                items = new List<Pop3ListItem>(1);
                string[] values = response.HostMessage.Split(' ');

                //should consist of '+OK messageNumber octets'
                if (values.Length < 3)
                {
                    throw new Pop3Exception(string.Concat("Invalid response message: ", response.HostMessage));
                }
                items.Add(new Pop3ListItem(Convert.ToInt32(values[1]), Convert.ToInt64(values[2])));
            } //Parse the single line results.

            return new ListResponse(response, items);
        }
    }
}
