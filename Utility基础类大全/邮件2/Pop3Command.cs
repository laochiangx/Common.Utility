using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DotNet.Utilities
{
    /// <summary>
    /// This class represents a generic Pop3 command and 
    /// encapsulates the major operations when executing a
    /// Pop3 command against a Pop3 Server.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class Pop3Command<T> : IDisposable where T : Pop3Response
    {
        public event Action<string> Trace;

        protected void OnTrace(string message)
        {
            if (Trace != null)
            {
                Trace(message);
            }
        }

        private const int BufferSize = 1024;
        private const string MultilineMessageTerminator = "\r\n.\r\n";
        private const string MessageTerminator = ".";

        private ManualResetEvent _manualResetEvent;

        private byte[] _buffer;
        private MemoryStream _responseContents;

        private Pop3State _validExecuteState;
        public Pop3State ValidExecuteState
        {
            get { return _validExecuteState; }
        }

        private Stream _networkStream;
        public Stream NetworkStream
        {
            get { return _networkStream; }
            set { _networkStream = value; }
        }

        bool _isMultiline;
        /// <summary>
        /// Sets a value indicating whether this instance is multiline.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is multiline; otherwise, <c>false</c>.
        /// </value>
        protected bool IsMultiline
        {
            get
            {
                return _isMultiline;
            }
            set
            {
                _isMultiline = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pop3CommandBase"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="isMultiline">if set to <c>true</c> [is multiline].</param>
        /// <param name="validExecuteState">State of the valid execute.</param>
        public Pop3Command(Stream stream, bool isMultiline, Pop3State validExecuteState)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            _manualResetEvent = new ManualResetEvent(false);
            _buffer = new byte[BufferSize];
            _responseContents = new MemoryStream();
            _networkStream = stream;
            _isMultiline = isMultiline;
            _validExecuteState = validExecuteState;
        }

        /// <summary>
        /// Abstract method intended for inheritors to 
        /// build out the byte[] request message for 
        /// the specific command.
        /// </summary>
        /// <returns>The byte[] containing the request message.</returns>
        protected abstract byte[] CreateRequestMessage();

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void Send(byte[] message)
        {
            //EnsureConnection();

            try
            {
                _networkStream.Write(message, 0, message.Length);
            }
            catch (SocketException e)
            {
                throw new Pop3Exception("Unable to send the request message: " + Encoding.ASCII.GetString(message), e);
            }
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns></returns>
        internal virtual T Execute(Pop3State currentState)
        {
            EnsurePop3State(currentState);

            byte[] message = CreateRequestMessage();

            if (message != null)
            {
                Send(message);
            }

            T response = CreateResponse(GetResponse());

            if (response == null)
            {
                return null;
            }

            OnTrace(response.HostMessage);
            return response;
        }

        /// <summary>
        /// Ensures the state of the POP3.
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        protected void EnsurePop3State(Pop3State currentState)
        {
            if (!((currentState & ValidExecuteState) == currentState))
            {
                throw new Pop3Exception(string.Format("This command is being executed" +
                    "in an invalid execution state.  Current:{0}, Valid:{1}",
                    currentState, ValidExecuteState));
            }
        }

        /// <summary>
        /// Creates the response.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>The <c>Pop3Response</c> containing the results of the
        /// Pop3 command execution.</returns>
        protected virtual T CreateResponse(byte[] buffer)
        {
            return Pop3Response.CreateResponse(buffer) as T;
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <returns></returns>
        private byte[] GetResponse()
        {
            //EnsureConnection();

            AsyncCallback callback;

            if (_isMultiline)
            {
                callback = new AsyncCallback(GetMultiLineResponseCallback);
            }
            else
            {
                callback = new AsyncCallback(GetSingleLineResponseCallback);
            }
            try
            {
                Receive(callback);

                _manualResetEvent.WaitOne();

                return _responseContents.ToArray();
            }
            catch (SocketException e)
            {
                throw new Pop3Exception("Unable to get response.", e);
            }
        }

        /// <summary>
        /// Receives the specified callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        private IAsyncResult Receive(AsyncCallback callback)
        {
            return _networkStream.BeginRead(_buffer, 0, _buffer.Length, callback, null);
        }

        /// <summary>
        /// Writes the received bytes to buffer.
        /// </summary>
        /// <param name="bytesReceived">The bytes received.</param>
        /// <returns></returns>
        private string WriteReceivedBytesToBuffer(int bytesReceived)
        {
            _responseContents.Write(_buffer, 0, bytesReceived);
            byte[] contents = _responseContents.ToArray();
            return Encoding.ASCII.GetString(contents, (contents.Length > 5 ? contents.Length - 5 : 0), 5);
        }

        /// <summary>
        /// Gets the single line response callback.
        /// </summary>
        /// <param name="ar">The ar.</param>
        private void GetSingleLineResponseCallback(IAsyncResult ar)
        {
            int bytesReceived = _networkStream.EndRead(ar);
            string message = WriteReceivedBytesToBuffer(bytesReceived);

            if (message.EndsWith(Pop3Commands.Crlf))
            {
                _manualResetEvent.Set();
            }
            else
            {
                Receive(new AsyncCallback(GetSingleLineResponseCallback));
            }
        }

        /// <summary>
        /// Gets the multi line response callback.
        /// </summary>
        /// <param name="ar">The ar.</param>
        private void GetMultiLineResponseCallback(IAsyncResult ar)
        {
            int bytesReceived = _networkStream.EndRead(ar);
            string message = WriteReceivedBytesToBuffer(bytesReceived);
            if (message.EndsWith(MultilineMessageTerminator)
                || bytesReceived == 0) //if the POP3 server times out we'll get an error message, then we'll get a following callback w/ 0 bytes.
            {
                _manualResetEvent.Set();
            }
            else
            {
                Receive(new AsyncCallback(GetMultiLineResponseCallback));
            }
        }


        /// <summary>
        /// Gets the request message.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>A byte[] request message to send to the host.</returns>
        protected byte[] GetRequestMessage(params string[] args)
        {
            string message = string.Join(string.Empty, args);
            OnTrace(message);
            return Encoding.ASCII.GetBytes(message);
        }

        /// <summary>
        /// Strips the POP3 host message.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="header">The header.</param>
        /// <returns>A <c>MemoryStream</c> without the Pop3 server message.</returns>
        protected MemoryStream StripPop3HostMessage(byte[] bytes, string header)
        {
            int position = header.Length + 2;
            MemoryStream stream = new MemoryStream(bytes, position, bytes.Length - position);
            return stream;
        }

        /// <summary>
        /// Gets the response lines.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>A string[] of Pop3 response lines.</returns>
        protected string[] GetResponseLines(MemoryStream stream)
        {
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(stream))
            {
                try
                {
                    string line;
                    do
                    {
                        line = reader.ReadLine();

                        //pop3 protocol states if a line starts w/ a 
                        //'.' that line will be byte stuffed w/ a '.'
                        //if it is byte stuffed the remove the byte, 
                        //otherwise we have reached the end of the message.
                        if (line.StartsWith(MessageTerminator))
                        {
                            if (line == MessageTerminator)
                            {
                                break;
                            }

                            line = line.Substring(1);
                        }

                        lines.Add(line);

                    } while (true);

                }
                catch (IOException e)
                {
                    throw new Pop3Exception("Unable to get response lines.", e);
                }

                return lines.ToArray();
            }
        }

        public void Dispose()
        {
            if (_responseContents != null)
            {
                _responseContents.Dispose();
            }
        }
    }
}
