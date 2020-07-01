﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Core.Server
{
    public class ChatServer
    {
        public int Port
        {
            get { return port; }
        }
        private int port;
        public event dgClientConnected ClientConnected;
        public event dgClientDisconnected ClientDisconnected;
        public event dgNewMessageReceivedFromClient NewMessageReceivedFromClient;

        private long lastClientId = 0;
        public SortedList<long, Client> Clients { get; set; }
        private volatile bool working;
        private object objSenk = new object();
        private ConnectionListener connectionListener;

        public ChatServer(int port)
        {
            this.port = port;
            this.Clients = new SortedList<long, Client>();
            this.connectionListener = new ConnectionListener(this, port);
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
            if (NewMessageReceivedFromClient != null)
                NewMessageReceivedFromClient(new ClientSendMessageArguments(client, message));
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
            private ChatServer server;
            private TcpListener listenerSocket;
            private int port;
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
                Socket istemciSoketi;
                while (working)
                {
                    try
                    {
                        istemciSoketi = listenerSocket.AcceptSocket();
                        if (istemciSoketi.Connected)
                        {
                            try { server.newClientSocketConnected(istemciSoketi); }
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
            private const byte START_BYTE = (byte)60;
            private const byte END_BYTE = (byte)62;

            public bool BlockStatus { get { return blockStatus; } }
            private bool blockStatus = false;
            public long ClientId { get { return clientId; } }
            private long clientId;
            public string Nick { get { return nick; } }
            private string nick;
            public string IPAddress { get { return ipAddress; } }
            private string ipAddress;

            public bool HasConnection
            {
                get { return working; }
            }
            public event dgConnectionClosed connectionClosed;
            public event dgNewMessageReceived newMessageReceived;

            private Socket soket;
            private ChatServer server;
            private NetworkStream networkStram;
            private BinaryReader binaryReader;
            private BinaryWriter binaryWriter;
            private volatile bool working = false;
            private Thread thread;

            public Client(ChatServer server, Socket clientSocket, long clientId)
            {
                this.server = server;
                this.soket = clientSocket;
                this.clientId = clientId;
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
                this.Stop();
            }

            public bool SendMessage(string message)
            {
                return SendCommand(Cmd.Message, JsonConvert.SerializeObject((new Message { From = ClientId, To = 0, Content = message })));
            }

            public bool SendCommand(Cmd cmd, string content)
            {
                try
                {
                    string _result = JsonConvert.SerializeObject(new Command { Cmd = cmd, Content = content });
                    byte[] bMesaj = Encoding.BigEndianUnicode.GetBytes(_result);
                    byte[] b = new byte[bMesaj.Length + 2];
                    Array.Copy(bMesaj, 0, b, 1, bMesaj.Length);
                    b[0] = START_BYTE;
                    b[b.Length - 1] = END_BYTE;
                    binaryWriter.Write(b);
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
                        byte b = binaryReader.ReadByte();
                        if (b != START_BYTE)
                            continue;
                        ClientItem clientItem;
                        List<byte> bList = new List<byte>();
                        while ((b = binaryReader.ReadByte()) != END_BYTE)
                            bList.Add(b);
                        string result = Encoding.BigEndianUnicode.GetString(bList.ToArray());
                        Command command = JsonConvert.DeserializeObject<Command>(result);
                        switch (command.Cmd)
                        {
                            case Cmd.Message:
                                Message message = JsonConvert.DeserializeObject<Message>(command.Content);
                                server.newMessageReceivedFromClient(this, message);
                                if (message.To == 0)
                                    foreach (var item in server.Clients)
                                        item.Value.SendCommand(command.Cmd, command.Content);
                                else
                                    foreach (var item in server.Clients.Where(c => c.Key == message.To || c.Key == message.From))
                                        item.Value.SendCommand(command.Cmd, command.Content);

                                break;
                            case Cmd.Login:
                                clientItem = JsonConvert.DeserializeObject<ClientItem>(command.Content);
                                nick = clientItem.Nick;
                                ipAddress = clientItem.IPAddress;
                                if (server.ClientConnected != null)
                                    server.ClientConnected(new ClientConnectionArguments(this));
                                foreach (var item in server.Clients)
                                    item.Value.SendCommand(Cmd.UserList, JsonConvert.SerializeObject((from c in server.Clients select new ClientItem { Nick = c.Value.Nick, ClientId = c.Key, IPAddress = c.Value.IPAddress }).ToList()));
                                break;
                            case Cmd.Logout:
                                foreach (var item in server.Clients.Where(c => c.Key != clientId))
                                    item.Value.SendCommand(Cmd.UserList, JsonConvert.SerializeObject((from c in item.Value.server.Clients.Where(c => c.Key != clientId) select new ClientItem { Nick = c.Value.Nick, ClientId = c.Key, IPAddress = c.Value.IPAddress }).ToList()));
                                if (server.ClientDisconnected != null)
                                    server.ClientDisconnected(new ClientConnectionArguments(this));
                                break;
                            case Cmd.SetNick:
                                nick = command.Content;
                                foreach (var item in server.Clients)
                                    item.Value.SendCommand(Cmd.UserList, JsonConvert.SerializeObject((from c in server.Clients select new ClientItem { Nick = c.Value.Nick, ClientId = c.Key, IPAddress = c.Value.IPAddress }).ToList()));
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
                            default:
                                break;
                        }
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

            private void connectionClosed_event()
            {
                if (connectionClosed != null)
                    connectionClosed();
            }

            internal void newMessageReceived_event(Message message, ClientItem from)
            {
                if (newMessageReceived != null)
                    newMessageReceived(new MessageReceivingArguments(message, from));
            }

            public void Block()
            {
                blockStatus = true;
                SendCommand(Cmd.Block, string.Empty);
            }

            public void Unblock()
            {
                blockStatus = false;
                SendCommand(Cmd.Unblock, string.Empty);
            }
        }
    }
}
