using Chat.Core.Cryptography;
using Chat.Core.Server;
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
using System.Threading.Tasks;

namespace Chat.Core.Client
{
    public class ChatClient
    {
        private const byte START_BYTE = (byte)60;
        private const byte END_BYTE = (byte)62;
        private string privateKey;
        public string publicKey;

        private ClientStatus status;
        public ClientStatus Status { get { return status; } }

        public void SetStatus(ClientStatus status)
        {
            this.status = status;
            sendCommand(Cmd.SetStatus, ((int)status).ToString());
        }

        public bool BlockStatus { get { return blockStatus; } }
        private bool blockStatus = false;
        public string ServerIPAddress { get { return serverIPAddress; } }
        private string serverIPAddress;
        public string Nick { get { return nick; } }
        private string nick;
        public long ClientId { get { return clientId; } }
        private long clientId;
        public string ClientIPAddress { get { return clientIPAddress; } }
        private string clientIPAddress;
        List<ClientItem> clients = new List<ClientItem>();
        public int ServerPort
        {
            get { return serverPort; }
            set { serverPort = value; }
        }
        private int serverPort;
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
            this.nick = nick;
            this.serverIPAddress = serverIPAddress;
            this.serverPort = serverPort;
            this.clientIPAddress = clientIPAddress;
        }

        public bool Connect()
        {
            try
            {
                clientConnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIPAddress), serverPort);
                clientConnection.Connect(ipep);
                networkStream = new NetworkStream(clientConnection);
                binaryReader = new BinaryReader(networkStream, Encoding.BigEndianUnicode);
                binaryWriter = new BinaryWriter(networkStream, Encoding.BigEndianUnicode);
                thread = new Thread(new ThreadStart(tRun));
                working = true;
                thread.Start();
                login(new ClientItem(0, nick, clientIPAddress, publicKey, status));
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
            this.status = ClientStatus.Available;
            return sendCommand(Cmd.Login, JsonConvert.SerializeObject(client));
        }

        public bool SendMessage(string message)
        {
            if (sendCommand(Cmd.Message, JsonConvert.SerializeObject((new Message { From = ClientId, To = 0, Content = message }))))
            {
                newMessageReceivedTrigger(new Message { Content = message, From = ClientId, To = 0 });
                return true;
            }
            return false;
        }

        public bool SendMessage(string message, long toClientId)
        {
            var toClient = clients.First(c => c.ClientId == toClientId);
            if (sendCommand(Cmd.Message, JsonConvert.SerializeObject((new Message { From = ClientId, To = toClientId, Content = message.Encrypt(toClient.PublicKey) }))))
            {
                newMessageReceivedTrigger(new Message { Content = message, From = ClientId, To = toClientId });
                return true;
            }
            return false;
        }

        public bool SetNick(string nickName)
        {
            nick = nickName;
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
                if (blockStatus && cmd == Cmd.Message)
                    return false;
                string result = JsonConvert.SerializeObject(new Command { Cmd = cmd, Content = content });
                byte[] bMessage = Encoding.BigEndianUnicode.GetBytes(result);
                byte[] b = new byte[bMessage.Length + 4];
                Array.Copy(bMessage, 0, b, 2, bMessage.Length);
                b[0] = START_BYTE;
                b[1] = START_BYTE + 1;
                b[b.Length - 2] = END_BYTE;
                b[b.Length - 1] = END_BYTE + 1;
                binaryWriter.Write(b);
                networkStream.Flush();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        bool startByte1 = false;
        bool startByte2 = false;
        bool endByte = false;

        private void tRun()
        {
            while (working)
            {
                try
                {
                    byte b = binaryReader.ReadByte();
                    if (!startByte1 && !startByte2 && b == START_BYTE)
                        startByte1 = true;
                    else if (startByte1 && !startByte2 && b == (START_BYTE + 1))
                        startByte2 = true;
                    else
                    {
                        startByte1 = false;
                        startByte2 = false;
                    }

                    if (!startByte1 || !startByte2)
                        continue;
                    List<byte> bList = new List<byte>();
                    while (!endByte)
                    {
                        byte b1 = binaryReader.ReadByte();
                        byte b2;
                        if (!endByte && b1 == END_BYTE)
                        {
                            b2 = binaryReader.ReadByte();
                            if (b2 == (END_BYTE + 1))
                            {
                                endByte = true;
                                break;
                            }
                            else
                            {
                                bList.Add(b1);
                                bList.Add(b2);
                            }
                        }
                        else
                            bList.Add(b1);
                    }
                    startByte1 = false;
                    startByte2 = false;
                    endByte = false;

                    try
                    {
                        string result = Encoding.BigEndianUnicode.GetString(bList.ToArray());
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
                    if (message.To == 0 || message.To == clientId || message.From == clientId)
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
                    blockStatus = true;
                    break;
                case Cmd.Unblock:
                    blockStatus = false;
                    break;
                case Cmd.SetStatus:
                    break;
                default:
                    break;
            }
        }

        private void newMessageReceivedTrigger(Message message)
        {
            if (NewMessgeReceived != null)
                NewMessgeReceived(new MessageReceivingArguments(message));
        }

        private void serverStoppedTrigger()
        {
            if (ServerStopped != null)
                ServerStopped();
        }

        private void clientListRefreshTrigger(ClientListResponse response)
        {
            clientId = response.Client.ClientId;
            if (ClientListRefresh != null)
                ClientListRefresh(response);
        }
    }
}
