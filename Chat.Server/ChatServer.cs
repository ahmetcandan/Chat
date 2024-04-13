using Chat.Abstraction.Enum;
using Chat.Abstraction.Event;
using Chat.Abstraction.Model;
using Chat.Server.Event;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

namespace Chat.Server;

public class ChatServer
{
    public int Port { get; }

    public event dgClientConnected ClientConnected;
    public event dgClientDisconnected ClientDisconnected;
    public event dgNewMessageReceivedFromClient NewMessageReceivedFromClient;

    private long _lastClientId = 0;
    private SortedList<long, Client> Clients { get; set; }
    private volatile bool _working;
    private readonly object _objSenk = new();
    private readonly ConnectionListener _connectionListener;

    public ChatServer(int port)
    {
        Port = port;
        Clients = [];
        _connectionListener = new ConnectionListener(this, port);
    }

    public void Start()
    {
        if (_working)
            return;
        if (!_connectionListener.Start())
            return;
        _working = true;
    }

    public void Stop()
    {
        foreach (var item in Clients)
            item.Value.SendCommand(Cmd.ServerStop, string.Empty);
        _connectionListener.Stop();
        _working = false;
        try
        {
            IList<Client> clientList = Clients.Values;
            foreach (Client clt in clientList)
                clt.Stop();
        }
        catch (Exception)
        {

        }
        Clients.Clear();
        _working = false;
    }

    public static bool SendMessage(IClient client, string message)
    {
        return client.SendMessage(message);
    }

    private void NewClientSocketConnected(Socket clientSocket)
    {
        Client client;
        lock (_objSenk)
        {
            client = new Client(this, clientSocket, ++_lastClientId);
            Clients.Add(client.ClientId, client);
        }
        client.Start();
    }

    private void NewMessageReceivedFromClient_Event(Client client, Message message)
    {
        NewMessageReceivedFromClient?.Invoke(new ClientSendMessageArguments(client, message));
    }

    private void ClientConnectionClosed(Client client)
    {
        if (_working)
            lock (_objSenk)
                if (Clients.ContainsKey(client.ClientId))
                    Clients.Remove(client.ClientId);
    }

    public void BlockClient(long clientId)
    {
        var client = Clients[clientId];
        if (client.BlockStatus)
            client.Unblock();
        else
            client.Block();
    }

    public bool ClientBlockStatus(long clientId)
    {
        return Clients[clientId].BlockStatus;
    }

    private class ConnectionListener
    {
        private readonly ChatServer _server;
        private TcpListener _listenerSocket;
        private readonly int _port;
        private volatile bool _working = false;
        private volatile Thread _thread;
        public ConnectionListener(ChatServer server, int port)
        {
            _server = server;
            _port = port;
        }

        public bool Start()
        {
            if (Connect())
            {
                _working = true;
                _thread = new Thread(new ThreadStart(TListen));
                _thread.Start();
                return true;
            }
            else
                return false;
        }

        public bool Stop()
        {
            try
            {
                _working = false;
                Disconnect();
                _thread.Join();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool Connect()
        {
            try
            {
                _listenerSocket = new TcpListener(System.Net.IPAddress.Any, _port);
                _listenerSocket.Start();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool Disconnect()
        {
            try
            {
                _listenerSocket.Stop();
                _listenerSocket = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void TListen()
        {
            Socket clientSocket;
            while (_working)
            {
                try
                {
                    clientSocket = _listenerSocket.AcceptSocket();
                    if (clientSocket.Connected)
                    {
                        try { _server.NewClientSocketConnected(clientSocket); }
                        catch (Exception) { }
                    }
                }
                catch (Exception)
                {
                    if (_working)
                    {
                        Disconnect();
                        try { Thread.Sleep(1000); }
                        catch (Exception) { }
                        Connect();
                    }
                }
            }
        }
    }

    private class Client : IClient
    {
        public ClientStatus Status { get; private set; }

        public void SetStatus(ClientStatus status)
        {
            Status = status;
        }

        public string PublicKey { get; private set; }

        public bool BlockStatus { get; private set; } = false;

        public long ClientId { get; }

        public string Nick { get; private set; }

        public string IPAddress { get; private set; }

        public bool HasConnection
        {
            get { return _working; }
        }
        public event dgConnectionClosed ConnectionClosed;
        public event dgNewMessageReceived NewMessageReceived;

        private readonly Socket _soket;
        private readonly ChatServer _server;
        private NetworkStream _networkStram;
        private BinaryReader _binaryReader;
        private BinaryWriter _binaryWriter;
        private volatile bool _working = false;
        private Thread _thread;

        public Client(ChatServer server, Socket clientSocket, long clientId)
        {
            _server = server;
            _soket = clientSocket;
            ClientId = clientId;
            Status = ClientStatus.Available;
        }

        public bool Start()
        {
            try
            {
                _networkStram = new NetworkStream(_soket);
                _binaryReader = new BinaryReader(_networkStram, Encoding.BigEndianUnicode);
                _binaryWriter = new BinaryWriter(_networkStram, Encoding.BigEndianUnicode);
                _thread = new Thread(new ThreadStart(TRun));
                _working = true;
                _thread.Start();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Stop()
        {
            try
            {
                _working = false;
                _soket.Close();
                _thread.Join();
            }
            catch (Exception)
            {

            }
        }

        public void CloseConnection()
        {
            Stop();
        }

        public bool SendMessage(string message)
        {
            return SendCommand(Cmd.Message, JsonConvert.SerializeObject(new Message(ClientId, message)));
        }

        public bool SendCommand(Cmd cmd, string content)
        {
            try
            {
                string _result = JsonConvert.SerializeObject(new Command(cmd, content));
                _binaryWriter.Write(_result);
                _networkStram.Flush();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void TRun()
        {
            while (_working)
            {
                try
                {
                    string receivedMessage = _binaryReader.ReadString();
                    Command command = JsonConvert.DeserializeObject<Command>(receivedMessage);
                    ReceivedCommand(command);
                }
                catch
                {
                    break;
                }
            }
            _working = false;
            try
            {
                if (_soket.Connected)
                {
                    _soket.Close();
                }
            }
            catch (Exception)
            {

            }
            _server.ClientConnectionClosed(this);
            ConnectionClosed_event();
        }

        private void ReceivedCommand(Command command)
        {
            ClientItem clientItem;
            switch (command.Cmd)
            {
                case Cmd.Message:
                    if (BlockStatus)
                        break;
                    Message message = JsonConvert.DeserializeObject<Message>(command.Content);
                    _server.NewMessageReceivedFromClient_Event(this, message);
                    if (message.To == 0)
                        foreach (var item in _server.Clients.Where(c => c.Key != message.From))
                            item.Value.SendCommand(command.Cmd, command.Content);
                    else
                        foreach (var item in _server.Clients.Where(c => c.Key == message.To))
                            item.Value.SendCommand(command.Cmd, command.Content);

                    break;
                case Cmd.Login:
                    clientItem = JsonConvert.DeserializeObject<ClientItem>(command.Content);
                    Nick = clientItem.Nick;
                    string originalNick = Nick;
                    IPAddress = clientItem.IPAddress;
                    PublicKey = clientItem.PublicKey;
                    int code = 0;
                    while (_server.Clients.Any(c => c.Value.Nick == Nick && c.Key != ClientId))
                    {
                        code++;
                        Nick = $"{originalNick}-{code:000}";
                    }
                    _server.ClientConnected?.Invoke(new ClientConnectionArguments(this));
                    foreach (var item in _server.Clients)
                        item.Value.SendCommand(Cmd.UserList, JsonConvert.SerializeObject(new ClientListResponse()
                        {
                            Clients = (from c in _server.Clients select new ClientItem(c.Key, c.Value.Nick, c.Value.IPAddress, c.Value.PublicKey, c.Value.Status)).ToList(),
                            Client = new ClientItem(item.Value.ClientId, item.Value.Nick, item.Value.IPAddress, item.Value.PublicKey, item.Value.Status),
                            ProcessClient = new ClientItem(ClientId, Nick, IPAddress, PublicKey, Status),
                            ClientEvent = ClientEvent.Login
                        }));
                    break;
                case Cmd.Logout:
                    foreach (var item in _server.Clients.Where(c => c.Key != ClientId))
                        item.Value.SendCommand(Cmd.UserList, JsonConvert.SerializeObject(new ClientListResponse()
                        {
                            Clients = (from c in _server.Clients.Where(c => c.Key != ClientId) select new ClientItem(c.Key, c.Value.Nick, c.Value.IPAddress, c.Value.PublicKey, c.Value.Status)).ToList(),
                            Client = new ClientItem(item.Value.ClientId, item.Value.Nick, item.Value.IPAddress, item.Value.PublicKey, item.Value.Status),
                            ProcessClient = new ClientItem(ClientId, Nick, IPAddress, PublicKey, Status),
                            ClientEvent = ClientEvent.Logout
                        }));
                    _server.ClientDisconnected?.Invoke(new ClientConnectionArguments(this));
                    break;
                case Cmd.SetNick:
                    Nick = command.Content;
                    foreach (var item in _server.Clients)
                        item.Value.SendCommand(Cmd.UserList, JsonConvert.SerializeObject(new ClientListResponse()
                        {
                            Clients = (from c in _server.Clients select new ClientItem(c.Key, c.Value.Nick, c.Value.IPAddress, c.Value.PublicKey, c.Value.Status)).ToList(),
                            Client = new ClientItem(item.Value.ClientId, item.Value.Nick, item.Value.IPAddress, item.Value.PublicKey, item.Value.Status),
                            ProcessClient = new ClientItem(ClientId, Nick, IPAddress, PublicKey, Status),
                            ClientEvent = ClientEvent.Refresh
                        }));
                    break;
                case Cmd.Command:
                    break;
                case Cmd.UserList:
                    break;
                case Cmd.ServerStop:
                    break;
                case Cmd.Block:
                    break;
                case Cmd.Unblock:
                    break;
                case Cmd.SetStatus:
                    int statusCode = int.Parse(command.Content);
                    SetStatus((ClientStatus)statusCode);
                    foreach (var item in _server.Clients)
                        item.Value.SendCommand(Cmd.UserList, JsonConvert.SerializeObject(new ClientListResponse()
                        {
                            Clients = (from c in _server.Clients select new ClientItem(c.Key, c.Value.Nick, c.Value.IPAddress, c.Value.PublicKey, c.Value.Status)).ToList(),
                            Client = new ClientItem(item.Value.ClientId, item.Value.Nick, item.Value.IPAddress, item.Value.PublicKey, item.Value.Status),
                            ProcessClient = new ClientItem(ClientId, Nick, IPAddress, PublicKey, Status),
                            ClientEvent = ClientEvent.Refresh
                        }));
                    break;
                default:
                    break;
            }
        }

        private void ConnectionClosed_event()
        {
            ConnectionClosed?.Invoke();
        }

        internal void NewMessageReceived_event(Message message)
        {
            NewMessageReceived?.Invoke(new MessageReceivingArguments(message));
        }

        public void Block()
        {
            BlockStatus = true;
            SendCommand(Cmd.Block, string.Empty);
        }

        public void Unblock()
        {
            BlockStatus = false;
            SendCommand(Cmd.Unblock, string.Empty);
        }
    }
}
