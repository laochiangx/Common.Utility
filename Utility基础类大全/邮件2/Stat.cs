using System;


namespace DotNet.Utilities
{
    /// <summary>
    /// This class represents the results from the execution 
    /// of a pop3 STAT command.
    /// </summary>
    public sealed class Stat
    {
        private int _messageCount;

        /// <summary>
        /// Gets or sets the message count.
        /// </summary>
        /// <value>The message count.</value>
        public int MessageCount
        {
            get { return _messageCount; }
            set { _messageCount = value; }
        }

        private long _octets;

        /// <summary>
        /// Gets or sets the octets.
        /// </summary>
        /// <value>The octets.</value>
        public long Octets
        {
            get { return _octets; }
            set { _octets = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stat"/> class.
        /// </summary>
        /// <param name="messageCount">The message count.</param>
        /// <param name="octets">The octets.</param>
        public Stat(int messageCount, long octets)
        {
            if (messageCount < 0)
            {
                throw new ArgumentOutOfRangeException("messageCount");
            }

            if (octets < 0)
            {
                throw new ArgumentOutOfRangeException("octets");
            }
            _messageCount = messageCount;
            _octets = octets;
        }
    }
}
