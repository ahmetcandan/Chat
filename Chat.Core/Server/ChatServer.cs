using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Chat.Core.Server
{
    public class ChatServer
    {
        public int Port { get; }

        public event dgClientConnected ClientConnected;
        public event dgClientDisconnected ClientDisconnected;
        public event dgNewMessageReceivedFromClient NewMessageReceivedFromClient;

        private long lastClientId = 0;
        public SortedList<long, Client> Clients { get; set; }
        private volatile bool working;
        private readonly object objSenk = new object();
        private readonly ConnectionListener connectionListener;

        public ChatServer(int port)
        {
            Port = port;
            Clients = new SortedList<long, Client>();
            connectionListener = new ConnectionListener(this, port);
        }

        public void Start()
        {
            if (working)
                return;
            if (!connectionListener.Start())
                return;
            working = true;
        }

        public void Stop()
        {
            foreach (var item in Clients)
                item.Value.SendCommand(Cmd.ServerStop, string.Empty);
            connectionListener.Stop();
            working = false;
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
            working = false;
        }

        public bool SendMessage(IClient client, string message)
        {
            return client.SendMessage(message);
        }

        private void newClientSocketConnected(Socket clientSocket)
        {
            Client client = null;
            lock (objSenk)
            {
                client = new Client(this, clientSocket, ++lastClientId);
                Clients.Add(client.ClientId, client);
            }
            client.Start();
        }

        private void newMessageReceivedFromClient(Client client, Message message)
        {
            NewMessageReceivedFromClient?.Invoke(new ClientSendMessageArguments(client, message));
        }

        private void clientConnectionClosed(Client client)
        {
            if (working)
                lock (objSenk)
                    if (Clients.ContainsKey(client.ClientId))
                        Clients.Remove(client.ClientId);
        }

        private class ConnectionListener
        {
            private readonly ChatServer server;
            private TcpListener listenerSocket;
            private readonly int port;
            private volatile bool working = false;
            private volatile Thread thread;
            public ConnectionListener(ChatServer server, int port)
            {
                this.server = server;
                this.port = port;
            }

            public bool Start()
            {
                if (connect())
                {
                    working = true;
                    thread = new Thread(new ThreadStart(tListen));
                    thread.Start();
                    return true;
                }
                else
                    return false;
            }

            public bool Stop()
            {
                try
                {
                    working = false;
                    disconnect();
                    thread.Join();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            private bool connect()
            {
                try
                {
                    listenerSocket = new TcpListener(System.Net.IPAddress.Any, port);
                    listenerSocket.Start();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            private bool disconnect()
            {
                try
                {
                    listenerSocket.Stop();
                    listenerSocket = null;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public void tListen()
            {
                Socket clientSocket;
                while (working)
                {
                    try
                    {
                        clientSocket = listenerSocket.AcceptSocket();
                        if (clientSocket.Connected)
                        {
                            try { server.newClientSocketConnected(clientSocket); }
                            catch (Exception) { }
                        }
                    }
                    catch (Exception)
                    {
                        if (working)
                        {
                            disconnect();
                            try { Thread.Sleep(1000); }
                            catch (Exception) { }
                            connect();
                        }
                    }
                }
            }
        }

        public class Client : IClient
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
                get { return working; }
            }
            public event dgConnectionClosed connectionClosed;
            public event dgNewMessageReceived newMessageReceived;

            private readonly Socket soket;
            private readonly ChatServer server;
            private NetworkStream networkStram;
            private BinaryReader binaryReader;
            private BinaryWriter binaryWriter;
            private volatile bool working = false;
            private Thread thread;

            public Client(ChatServer server, Socket clientSocket, long clientId)
            {
                this.server = server;
                soket = clientSocket;
                ClientId = clientId;
                Status = ClientStatus.Available;
            }

            public bool Start()
            {
                try
                {
                    networkStram = new NetworkStream(soket);
                    binaryReader = new BinaryReader(networkStram, Encoding.BigEndianUnicode);
                    binaryWriter = new BinaryWriter(networkStram, Encoding.BigEndianUnicode);
                    thread = new Thread(new ThreadStart(tRun));
                    working = true;
                    thread.Start();
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
                    working = false;
                    soket.Close();
                    thread.Abort();
                    thread.Join();
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
                return SendCommand(Cmd.Message, JsonConvert.SerializeObject(new Message { From = ClientId, To = 0, Content = message }));
            }

            public bool SendCommand(Cmd cmd, string content)
            {
                try
                {
                    string _result = JsonConvert.SerializeObject(new Command { Cmd = cmd, Content = content });
                    binaryWriter.Write(_result);
                    networkStram.Flush();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            private void tRun()
            {
                while (working)
                {
                    try
                    {
                        string receivedMessage = binaryReader.ReadString();

                        try
                        {
                            string result = receivedMessage;
                            Command command = JsonConvert.DeserializeObject<Command>(result);
                            receivedCommand(command);
                        }
                        catch { }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
                working = false;
                try
                {
                    if (soket.Connected)
                    {
                        soket.Close();
                    }
                }
                catch (Exception)
                {

                }
                server.clientConnectionClosed(this);
                connectionClosed_event();
            }

            private void receivedCommand(Command command)
            {
                ClientItem clientItem;
                switch (command.Cmd)
                {
                    case Cmd.Message:
                        if (BlockStatus)
                            break;
                        Message message = JsonConvert.DeserializeObject<Message>(command.Content);
                        server.newMessageReceivedFromClient(this, message);
                        if (message.To == 0)
                            foreach (var item in server.Clients.Where(c => c.Key != message.From))
                                item.Value.SendCommand(command.Cmd, command.Content);
                        else
                            foreach (var item in server.Clients.Where(c => c.Key == message.To))
                                item.Value.SendCommand(command.Cmd, command.Content);

                        break;
                    case Cmd.Login:
                        clientItem = JsonConvert.DeserializeObject<ClientItem>(command.Content);
                        Nick = clientItem.Nick;
                        string originalNick = Nick;
                        IPAddress = clientItem.IPAddress;
                        PublicKey = clientItem.PublicKey;
                        int code = 0;
                        while (server.Clients.Any(c => c.Value.Nick == Nick && c.Key != ClientId))
                        {
                            code++;
                            Nick = $"{originalNick}-{code:000}";
                        }
                        server.ClientConnected?.Invoke(new ClientConnectionArguments(this));
                        foreach (var item in server.Clients)
                            item.Value.SendCommand(Cmd.UserList, JsonConvert.SerializeObject(new ClientListResponse()
                            {
                                Clients = (from c in server.Clients select new ClientItem { Nick = c.Value.Nick, ClientId = c.Key, IPAddress = c.Value.IPAddress, PublicKey = c.Value.PublicKey, Status = c.Value.Status }).ToList(),
                                Client = new ClientItem { Nick = item.Value.Nick, ClientId = item.Value.ClientId, IPAddress = item.Value.IPAddress, PublicKey = item.Value.PublicKey, Status = item.Value.Status },
                                ProcessClient = new ClientItem { Nick = Nick, ClientId = ClientId, IPAddress = IPAddress, PublicKey = PublicKey, Status = Status },
                                ClientEvent = ClientEvent.Login
                            }));
                        break;
                    case Cmd.Logout:
                        foreach (var item in server.Clients.Where(c => c.Key != ClientId))
                            item.Value.SendCommand(Cmd.UserList, JsonConvert.SerializeObject(new ClientListResponse()
                            {
                                Clients = (from c in server.Clients.Where(c => c.Key != ClientId) select new ClientItem { Nick = c.Value.Nick, ClientId = c.Key, IPAddress = c.Value.IPAddress, PublicKey = c.Value.PublicKey, Status = c.Value.Status }).ToList(),
                                Client = new ClientItem { Nick = item.Value.Nick, ClientId = item.Value.ClientId, IPAddress = item.Value.IPAddress, PublicKey = item.Value.PublicKey, Status = item.Value.Status },
                                ProcessClient = new ClientItem { Nick = Nick, ClientId = ClientId, IPAddress = IPAddress, PublicKey = PublicKey, Status = Status },
                                ClientEvent = ClientEvent.Logout
                            }));
                        server.ClientDisconnected?.Invoke(new ClientConnectionArguments(this));
                        break;
                    case Cmd.SetNick:
                        Nick = command.Content;
                        foreach (var item in server.Clients)
                            item.Value.SendCommand(Cmd.UserList, JsonConvert.SerializeObject(new ClientListResponse()
                            {
                                Clients = (from c in server.Clients select new ClientItem { Nick = c.Value.Nick, ClientId = c.Key, IPAddress = c.Value.IPAddress, PublicKey = c.Value.PublicKey, Status = c.Value.Status }).ToList(),
                                Client = new ClientItem { Nick = item.Value.Nick, ClientId = item.Value.ClientId, IPAddress = item.Value.IPAddress, PublicKey = item.Value.PublicKey, Status = item.Value.Status },
                                ProcessClient = new ClientItem { Nick = Nick, ClientId = ClientId, IPAddress = IPAddress, PublicKey = PublicKey, Status = Status },
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
                        foreach (var item in server.Clients)
                            item.Value.SendCommand(Cmd.UserList, JsonConvert.SerializeObject(new ClientListResponse()
                            {
                                Clients = (from c in server.Clients select new ClientItem { Nick = c.Value.Nick, ClientId = c.Key, IPAddress = c.Value.IPAddress, PublicKey = c.Value.PublicKey, Status = c.Value.Status }).ToList(),
                                Client = new ClientItem { Nick = item.Value.Nick, ClientId = item.Value.ClientId, IPAddress = item.Value.IPAddress, PublicKey = item.Value.PublicKey, Status = item.Value.Status },
                                ProcessClient = new ClientItem { Nick = Nick, ClientId = ClientId, IPAddress = IPAddress, PublicKey = PublicKey, Status = Status },
                                ClientEvent = ClientEvent.Refresh
                            }));
                        break;
                    default:
                        break;
                }
            }

            private void connectionClosed_event()
            {
                connectionClosed?.Invoke();
            }

            internal void newMessageReceived_event(Message message)
            {
                newMessageReceived?.Invoke(new MessageReceivingArguments(message));
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
}
