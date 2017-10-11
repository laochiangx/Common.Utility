

namespace DotNet.Utilities
{
    /// <summary>
    /// This class contains the Positive and Negative starting response strings
    /// that can be returned from a Pop3 server.
    /// </summary>
    internal static class Pop3Responses
    {
        /// <summary>
        /// The +OK starting of a positive response from the server.
        /// </summary>
        internal const string Ok = "+OK";

        /// <summary>
        /// The -ERR starting of a negative response from the server.
        /// </summary>
        internal const string Err = "-ERR";
    }
}
