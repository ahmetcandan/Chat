using Chat.Core.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Core.Client
{
    public class ChatClient
    {
        private const byte START_BYTE = (byte)60;
        private const byte END_BYTE = (byte)62;

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
                login(new ClientItem { ClientId = 0, IPAddress = clientIPAddress, Nick = nick });
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
            return sendCommand(Cmd.Login, JsonConvert.SerializeObject(client));
        }

        public bool SendMessage(string message)
        {
            return sendCommand(Cmd.Message, JsonConvert.SerializeObject((new Message { From = ClientId, To = 0, Content = message })));
        }

        public bool SendMessage(string message, long toClientId)
        {
            return sendCommand(Cmd.Message, JsonConvert.SerializeObject((new Message { From = ClientId, To = toClientId, Content = message })));
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
                string _result = JsonConvert.SerializeObject(new Command { Cmd = cmd, Content = content });
                byte[] bMessage = Encoding.BigEndianUnicode.GetBytes(_result);
                byte[] b = new byte[bMessage.Length + 2];
                Array.Copy(bMessage, 0, b, 1, bMessage.Length);
                b[0] = START_BYTE;
                b[b.Length - 1] = END_BYTE;
                binaryWriter.Write(b);
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
                    byte b = binaryReader.ReadByte();
                    if (b != START_BYTE)
                        break;
                    List<byte> bList = new List<byte>();
                    while ((b = binaryReader.ReadByte()) != END_BYTE)
                        bList.Add(b);
                    ClientItem clientItem;
                    string result = Encoding.BigEndianUnicode.GetString(bList.ToArray());
                    Command command = JsonConvert.DeserializeObject<Command>(result);
                    switch (command.Cmd)
                    {
                        case Cmd.Message:
                            Message message = JsonConvert.DeserializeObject<Message>(command.Content);
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
