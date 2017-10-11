using System;
using System.Collections.Generic;

namespace DotNet.Utilities
{
    /// <summary>
    /// This class represents the response message 
    /// returned from both a single line and multi line 
    /// Pop3 LIST Command.
    /// </summary>
    internal sealed class ListResponse : Pop3Response
    {
        private List<Pop3ListItem> _items;

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public List<Pop3ListItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        /// <summary>
        /// Gets the message number.
        /// </summary>
        /// <value>The message number.</value>
        public int MessageNumber
        {
            get { return _items[0].MessageId; }
        }

        /// <summary>
        /// Gets number of octets.
        /// </summary>
        /// <value>The number of octets.</value>
        public long Octets
        {
            get { return _items[0].Octets; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListResponse"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="items">The items.</param>
        public ListResponse(Pop3Response response, List<Pop3ListItem> items)
            : base(response.ResponseContents, response.HostMessage, response.StatusIndicator)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            _items = items;
        }
    }
}
