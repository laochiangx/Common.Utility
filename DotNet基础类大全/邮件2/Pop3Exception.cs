using System;

namespace DotNet.Utilities
{
    /// <summary>
    /// This class represents a Pop3 Exception.
    /// </summary>
    [global::System.Serializable]
    public class Pop3Exception : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pop3Exception"/> class.
        /// </summary>
        public Pop3Exception() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Pop3Exception"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public Pop3Exception(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Pop3Exception"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public Pop3Exception(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pop3Exception"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
        /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
        protected Pop3Exception(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
