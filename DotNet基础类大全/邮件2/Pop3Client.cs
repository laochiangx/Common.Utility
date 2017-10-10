using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace DotNet.Utilities
{
    /// <summary>
    /// The Pop3Client class provides a wrapper for the Pop3 commands
    /// that can be executed against a Pop3Server.  This class will 
    /// execute and return results for the various commands that are 
    /// executed.
    /// </summary>
    public sealed class Pop3Client : IDisposable
    {
        private static readonly int DefaultPort = 110;

        private TcpClient _client;
        private Stream _clientStream;

        /// <summary>
        /// Traces the various command objects that executed during this objects
        /// lifetime.
        /// </summary>
        public event Action<string> Trace;

        private void OnTrace(string message)
        {
            if (Trace != null)
            {
                Trace(message);
            }
        }

        private string _hostname;
        /// <summary>
        /// Gets the hostname.
        /// </summary>
        /// <value>The hostname.</value>
        public string Hostname
        {
            get { return _hostname; }
        }

        private int _port;
        /// <summary>
        /// Gets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port
        {
            get { return _port; }
        }

        private bool _useSsl;
        /// <summary>
        /// Gets a value indicating whether [use SSL].
        /// </summary>
        /// <value><c>true</c> if [use SSL]; otherwise, <c>false</c>.</value>
        public bool UseSsl
        {
            get { return _useSsl; }
        }

        private string _username;
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _password;
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private Pop3State _currentState;
        /// <summary>
        /// Gets the state of the current.
        /// </summary>
        /// <value>The state of the current.</value>
        public Pop3State CurrentState
        {
            get { return _currentState; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pop3Client"/> class using the default POP3 port 110
        /// without using SSL.
        /// </summary>
        /// <param name="hostname">The hostname.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public Pop3Client(string hostname, string username, string password)
            : this(hostname, DefaultPort, false, username, password) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pop3Client"/> class using the default POP3 port 110.
        /// </summary>
        /// <param name="hostname">The hostname.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public Pop3Client(string hostname, bool useSsl, string username, string password)
            : this(hostname, DefaultPort, useSsl, username, password) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pop3Client"/> class.
        /// </summary>
        /// <param name="hostname">The hostname.</param>
        /// <param name="port">The port.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public Pop3Client(string hostname, int port, bool useSsl, string username, string password)
            : this()
        {
            if (string.IsNullOrEmpty(hostname))
            {
                throw new ArgumentNullException("hostname");
            }

            if (port < 0)
            {
                throw new ArgumentOutOfRangeException("port");
            }

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            _hostname = hostname;
            _port = port;
            _useSsl = useSsl;
            _username = username;
            _password = password;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pop3Client"/> class.
        /// </summary>
        private Pop3Client()
        {
            _client = new TcpClient();
            _currentState = Pop3State.Unknown;
        }

        /// <summary>
        /// Checks the connection.
        /// </summary>
        private void EnsureConnection()
        {
            if (!_client.Connected)
            {
                throw new Pop3Exception("Pop3 client is not connected.");
            }
        }

        /// <summary>
        /// Resets the state.
        /// </summary>
        /// <param name="state">The state.</param>
        private void SetState(Pop3State state)
        {
            _currentState = state;
        }

        /// <summary>
        /// Ensures the response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="error">The error.</param>
        private void EnsureResponse(Pop3Response response, string error)
        {
            if (response == null)
            {
                throw new Pop3Exception("Unable to get Response.  Response object null.");
            }

            if (response.StatusIndicator)
            {
                return;
            } //the command execution was successful.

            string errorMessage = string.Empty;

            if (string.IsNullOrEmpty(error))
            {
                errorMessage = response.HostMessage;
            }
            else
            {
                errorMessage = string.Concat(error, ": ", error);
            }

            throw new Pop3Exception(errorMessage);
        }

        /// <summary>
        /// Ensures the response.
        /// </summary>
        /// <param name="response">The response.</param>
        private void EnsureResponse(Pop3Response response)
        {
            EnsureResponse(response, string.Empty);
        }

        /// <summary>
        /// Traces the command.
        /// </summary>
        /// <param name="command">The command.</param>
        private void TraceCommand<TCommand, TResponse>(TCommand command)
            where TCommand : Pop3Command<TResponse>
            where TResponse : Pop3Response
        {
            if (Trace != null)
            {
                command.Trace += delegate(string message) { OnTrace(message); };
            }
        }

        /// <summary>
        /// Connects this instance and properly sets the 
        /// client stream to Use Ssl if it is specified.
        /// </summary>
        private void Connect()
        {
            if (_client == null)
            {
                _client = new TcpClient();
            } //If a previous quit command was issued, the client would be disposed of.

            if (_client.Connected)
            {
                return;
            } //if the connection already is established no need to reconnect.

            SetState(Pop3State.Unknown);
            ConnectResponse response;
            using (ConnectCommand command = new ConnectCommand(_client, _hostname, _port, _useSsl))
            {
                TraceCommand<ConnectCommand, ConnectResponse>(command);
                response = command.Execute(CurrentState);
                EnsureResponse(response);
            }

            SetClientStream(response.NetworkStream);

            SetState(Pop3State.Authorization);
        }

        /// <summary>
        /// Sets the client stream.  If UseSsl <c>true</c> then wrap 
        /// the client's <c>NetworkStream</c> in an <c>SslStream</c>, if UseSsl <c>false</c> 
        /// then set the client stream to the <c>NetworkStream</c>
        /// </summary>
        private void SetClientStream(Stream networkStream)
        {
            if (_clientStream != null)
            {
                _clientStream.Dispose();
            }

            _clientStream = networkStream;
        }

        /// <summary>
        /// Authenticates this instance.
        /// </summary>
        /// <remarks>A successful execution of this method will result in a Current State of Transaction.
        /// Unsuccessful USER or PASS commands can be reattempted by resetting the Username or Password 
        /// properties and re-execution of the methods.</remarks>
        /// <exception cref="Pop3Exception">
        /// If the Pop3Server is unable to be connected.
        /// If the User command is unable to be successfully executed.
        /// If the Pass command is unable to be successfully executed.
        /// </exception>
        public void Authenticate()
        {
            Connect();

            //execute the user command.
            using (UserCommand userCommand = new UserCommand(_clientStream, _username))
            {
                ExecuteCommand<Pop3Response, UserCommand>(userCommand);
            }

            //execute the pass command.
            using (PassCommand passCommand = new PassCommand(_clientStream, _password))
            {
                ExecuteCommand<Pop3Response, PassCommand>(passCommand);
            }

            _currentState = Pop3State.Transaction;
        }

        /// <summary>
        /// Executes the POP3 DELE command.
        /// </summary>
        /// <param name="item">The item.</param>
        /// /// <exception cref="Pop3Exception">If the DELE command was unable to be executed successfully.</exception>
        public void Dele(Pop3ListItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            using (DeleCommand command = new DeleCommand(_clientStream, item.MessageId))
            {
                ExecuteCommand<Pop3Response, DeleCommand>(command);
            }
        }

        /// <summary>
        /// Executes the POP3 NOOP command.
        /// </summary>
        /// <exception cref="Pop3Exception">If the NOOP command was unable to be executed successfully.</exception>
        public void Noop()
        {
            using (NoopCommand command = new NoopCommand(_clientStream))
            {
                ExecuteCommand<Pop3Response, NoopCommand>(command);
            }
        }

        /// <summary>
        /// Executes the POP3 RSET command.
        /// </summary>
        /// <exception cref="Pop3Exception">If the RSET command was unable to be executed successfully.</exception>
        public void Rset()
        {
            using (RsetCommand command = new RsetCommand(_clientStream))
            {
                ExecuteCommand<Pop3Response, RsetCommand>(command);
            }
        }

        /// <summary>
        /// Executes the POP3 STAT command.
        /// </summary>
        /// <returns>A Stat object containing the results of STAT command.</returns>
        /// <exception cref="Pop3Exception">If the STAT command was unable to be executed successfully.</exception>
        public Stat Stat()
        {
            StatResponse response;
            using (StatCommand command = new StatCommand(_clientStream))
            {
                response = ExecuteCommand<StatResponse, StatCommand>(command);
            }

            return new Stat(response.MessageCount, response.Octets);
        }

        /// <summary>
        /// Executes the POP3 List command.
        /// </summary>
        /// <returns>A generic List of Pop3Items containing the results of the LIST command.</returns>
        /// <exception cref="Pop3Exception">If the LIST command was unable to be executed successfully.</exception>
        public List<Pop3ListItem> List()
        {
            ListResponse response;
            using (ListCommand command = new ListCommand(_clientStream))
            {
                response = ExecuteCommand<ListResponse, ListCommand>(command);
            }
            return response.Items;
        }

        /// <summary>
        /// Lists the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A <c>Pop3ListItem</c> for the requested Pop3Item.</returns>
        /// <exception cref="Pop3Exception">If the LIST command was unable to be executed successfully for the provided message id.</exception>
        public Pop3ListItem List(int messageId)
        {
            ListResponse response;
            using (ListCommand command = new ListCommand(_clientStream, messageId))
            {
                response = ExecuteCommand<ListResponse, ListCommand>(command);
            }
            return new Pop3ListItem(response.MessageNumber, response.Octets);
        }

        /// <summary>
        /// Retrs the specified message.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>A MimeEntity for the requested Pop3 Mail Item.</returns>
        public MimeEntity RetrMimeEntity(Pop3ListItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (item.MessageId < 1)
            {
                throw new ArgumentOutOfRangeException("item.MessageId");
            }

            RetrResponse response;
            using (RetrCommand command = new RetrCommand(_clientStream, item.MessageId))
            {
                response = ExecuteCommand<RetrResponse, RetrCommand>(command);
            }

            MimeReader reader = new MimeReader(response.MessageLines);
            return reader.CreateMimeEntity();
        }

        public MailMessageEx Top(int messageId, int lineCount)
        {
            if (messageId < 1)
            {
                throw new ArgumentOutOfRangeException("messageId");
            }

            if (lineCount < 0)
            {
                throw new ArgumentOutOfRangeException("lineCount");
            }

            RetrResponse response;
            using (TopCommand command = new TopCommand(_clientStream, messageId, lineCount))
            {
                response = ExecuteCommand<RetrResponse, TopCommand>(command);
            }

            MimeReader reader = new MimeReader(response.MessageLines);
            MimeEntity entity = reader.CreateMimeEntity();
            MailMessageEx message = entity.ToMailMessageEx();
            message.Octets = response.Octets;
            message.MessageNumber = messageId;
            return entity.ToMailMessageEx();
        }

        /// <summary>
        /// Retrs the mail message ex.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public MailMessageEx RetrMailMessageEx(Pop3ListItem item)
        {

            MailMessageEx message = RetrMimeEntity(item).ToMailMessageEx();
            if (message != null)
            {
                message.MessageNumber = item.MessageId;
            }
            return message;
        }


        /// <summary>
        /// Executes the Pop3 QUIT command.
        /// </summary>
        /// <exception cref="Pop3Exception">If the quit command returns a -ERR server message.</exception>
        public void Quit()
        {
            using (QuitCommand command = new QuitCommand(_clientStream))
            {
                ExecuteCommand<Pop3Response, QuitCommand>(command);

                if (CurrentState.Equals(Pop3State.Transaction))
                {
                    SetState(Pop3State.Update);
                } // Messages could have been deleted, reflect the server state.

                Disconnect();

                //Quit command can only be called in Authorization or Transaction state, reset to Unknown.
                SetState(Pop3State.Unknown);
            }
        }

        /// <summary>
        /// Provides a common way to execute all commands.  This method
        /// validates the connection, traces the command and finally
        /// validates the response message for a -ERR response.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The Pop3Response for the provided command</returns>
        /// <exception cref="Pop3Exception">If the HostMessage does not start with '+OK'.</exception>
        /// <exception cref="Pop3Exception">If the client is no longer connected.</exception>
        private TResponse ExecuteCommand<TResponse, TCommand>(TCommand command)
            where TResponse : Pop3Response
            where TCommand : Pop3Command<TResponse>
        {
            EnsureConnection();
            TraceCommand<TCommand, TResponse>(command);
            TResponse response = (TResponse)command.Execute(CurrentState);
            EnsureResponse(response);
            return response;
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        private void Disconnect()
        {
            if (_clientStream != null)
            {
                _clientStream.Close();
            }  //release underlying socket.

            if (_client != null)
            {
                _client.Close();
                _client = null;
            }
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
