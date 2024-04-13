using Chat.Core.Cryptography;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Chat.Core.Client
{
    public class ChatClient
    {
        private readonly string privateKey;
        public string publicKey;

        public ClientStatus Status { get; private set; }

        public void SetStatus(ClientStatus status)
        {
            Status = status;
            sendCommand(Cmd.SetStatus, ((int)status).ToString());
        }

        public bool BlockStatus { get; private set; } = false;

        public string ServerIPAddress { get; }

        public string Nick { get; private set; }

        public long ClientId { get; private set; }

        public string ClientIPAddress { get; }

        private List<ClientItem> clients = new List<ClientItem>();
        public int ServerPort { get; set; }

        public event dgNewMessageReceived NewMessgeReceived;
        public event dgClientListRefresh ClientListRefresh;
        public event dgServerStopped ServerStopped;

        private Socket clientConnection;
        private NetworkStream networkStream;
        private BinaryWriter binaryWriter;
        private BinaryReader binaryReader;
        private Thread thread;
        private volatile bool working = false;
        public ChatClient(string serverIPAddress, int serverPort, string nick, string clientIPAddress)
        {
            var rsa = new RSACryptoServiceProvider();
            privateKey = rsa.ToXmlString(true);
            publicKey = rsa.ToXmlString(false);
            Nick = nick;
            ServerIPAddress = serverIPAddress;
            ServerPort = serverPort;
            ClientIPAddress = clientIPAddress;
        }

        public bool Connect()
        {
            try
            {
                clientConnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ServerIPAddress), ServerPort);
                clientConnection.Connect(ipep);
                networkStream = new NetworkStream(clientConnection);
                binaryReader = new BinaryReader(networkStream, Encoding.BigEndianUnicode);
                binaryWriter = new BinaryWriter(networkStream, Encoding.BigEndianUnicode);
                thread = new Thread(new ThreadStart(tRun));
                working = true;
                thread.Start();
                login(new ClientItem(0, Nick, ClientIPAddress, publicKey, Status));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Disconnected()
        {
            try
            {
                sendCommand(Cmd.Logout);
                Thread.Sleep(100);
                working = false;
                clientConnection.Close();
                thread.Abort(100);
                thread.Join();
            }
            catch (Exception)
            {

            }
        }

        private bool login(ClientItem client)
        {
            Status = ClientStatus.Available;
            return sendCommand(Cmd.Login, JsonConvert.SerializeObject(client));
        }

        public bool SendMessage(string message)
        {
            if (sendCommand(Cmd.Message, JsonConvert.SerializeObject(new Message { From = ClientId, To = 0, Content = message })))
            {
                newMessageReceivedTrigger(new Message { Content = message, From = ClientId, To = 0 });
                return true;
            }
            return false;
        }

        public bool SendMessage(string message, long toClientId)
        {
            var toClient = clients.First(c => c.ClientId == toClientId);
            if (sendCommand(Cmd.Message, JsonConvert.SerializeObject(new Message { From = ClientId, To = toClientId, Content = message.Encrypt(toClient.PublicKey) })))
            {
                newMessageReceivedTrigger(new Message { Content = message, From = ClientId, To = toClientId });
                return true;
            }
            return false;
        }

        public bool SetNick(string nickName)
        {
            Nick = nickName;
            return sendCommand(Cmd.SetNick, nickName);
        }

        private bool sendCommand(Cmd cmd)
        {
            return sendCommand(cmd, string.Empty);
        }

        private bool sendCommand(Cmd cmd, string content)
        {
            try
            {
                if (BlockStatus && cmd == Cmd.Message)
                    return false;
                string result = JsonConvert.SerializeObject(new Command { Cmd = cmd, Content = content });
                binaryWriter.Write(result);
                networkStream.Flush();
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
        }

        private void receivedCommand(Command command)
        {
            switch (command.Cmd)
            {
                case Cmd.Message:
                    Message message = JsonConvert.DeserializeObject<Message>(command.Content);
                    if (message.To == ClientId)
                        message.Content = message.Content.Decrypt(privateKey);
                    if (message.To == 0 || message.To == ClientId || message.From == ClientId)
                        newMessageReceivedTrigger(message);
                    break;
                case Cmd.Login:
                    break;
                case Cmd.Logout:
                    break;
                case Cmd.SetNick:
                    break;
                case Cmd.Command:
                    break;
                case Cmd.UserList:
                    var response = JsonConvert.DeserializeObject<ClientListResponse>(command.Content);
                    clients = response.Clients;
                    clientListRefreshTrigger(response);
                    break;
                case Cmd.ServerStop:
                    serverStoppedTrigger();
                    break;
                case Cmd.Block:
                    BlockStatus = true;
                    break;
                case Cmd.Unblock:
                    BlockStatus = false;
                    break;
                case Cmd.SetStatus:
                    break;
                default:
                    break;
            }
        }

        private void newMessageReceivedTrigger(Message message)
        {
            NewMessgeReceived?.Invoke(new MessageReceivingArguments(message));
        }

        private void serverStoppedTrigger()
        {
            ServerStopped?.Invoke();
        }

        private void clientListRefreshTrigger(ClientListResponse response)
        {
            ClientId = response.Client.ClientId;
            ClientListRefresh?.Invoke(response);
        }
    }
}
