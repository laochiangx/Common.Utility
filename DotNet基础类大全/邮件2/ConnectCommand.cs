using System;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;

namespace DotNet.Utilities
{
    /// <summary>
    /// Performs the connect to a Pop3 server and returns a Pop3 
    /// response indicating the attempt to connect results and the
    /// network stream to use for all subsequent Pop3 Commands.
    /// </summary>
    internal sealed class ConnectCommand : Pop3Command<ConnectResponse>
    {
        private TcpClient _client;
        private string _hostname;
        private int _port;
        private bool _useSsl;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectCommand"/> class.
        /// </summary>
        /// <remarks>
        /// Even though a network stream is provided to the base constructor the stream
        /// does not already exist so we have to send in a dummy stream until the actual
        /// connect has taken place.  Then we'll reset network stream to the 
        /// stream made available by the TcpClient.GetStream() to read the data returned
        /// after a connect.
        /// </remarks>
        /// <param name="client">The client.</param>
        /// <param name="hostname">The hostname.</param>
        /// <param name="port">The port.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        public ConnectCommand(TcpClient client, string hostname, int port, bool useSsl)
            : base(new System.IO.MemoryStream(), false, Pop3State.Unknown)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            if (string.IsNullOrEmpty(hostname))
            {
                throw new ArgumentNullException("hostname");
            }

            if (port < 1)
            {
                throw new ArgumentOutOfRangeException("port");
            }

            _client = client;
            _hostname = hostname;
            _port = port;
            _useSsl = useSsl;
        }

        /// <summary>
        /// Creates the connect request message.
        /// </summary>
        /// <returns>A byte[] containing connect request message.</returns>
        protected override byte[] CreateRequestMessage()
        {
            return null;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns></returns>
        internal override ConnectResponse Execute(Pop3State currentState)
        {
            EnsurePop3State(currentState);

            try
            {
                _client.Connect(_hostname, _port);

                SetClientStream();
            }
            catch (SocketException e)
            {
                throw new Pop3Exception(string.Format("Unable to connect to {0}:{1}.", _hostname, _port), e);
            }

            return base.Execute(currentState);
        }

        /// <summary>
        /// Sets the client stream.
        /// </summary>
        private void SetClientStream()
        {
            if (_useSsl)
            {
                try
                {
                    NetworkStream = new SslStream(_client.GetStream(), true); //make sure the inner stream stays available for the Pop3Client to make use of.
                    ((SslStream)NetworkStream).AuthenticateAsClient(_hostname);
                }
                catch (ArgumentException e)
                {
                    throw new Pop3Exception("Unable to create Ssl Stream for hostname: " + _hostname, e);
                }
                catch (AuthenticationException e)
                {
                    throw new Pop3Exception("Unable to authenticate ssl stream for hostname: " + _hostname, e);
                }
                catch (InvalidOperationException e)
                {
                    throw new Pop3Exception("There was a problem  attempting to authenticate this SSL stream for hostname: " + _hostname, e);
                }
            } //wrap NetworkStream in an SSL stream
            else
            {
                NetworkStream = _client.GetStream();
            }

        }

        /// <summary>
        /// Creates the response.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>
        /// The <c>Pop3Response</c> containing the results of the
        /// Pop3 command execution.
        /// </returns>
        protected override ConnectResponse CreateResponse(byte[] buffer)
        {
            Pop3Response response = Pop3Response.CreateResponse(buffer);
            return new ConnectResponse(response, NetworkStream);
        }
    }
}
