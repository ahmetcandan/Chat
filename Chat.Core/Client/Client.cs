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

        public string ServerIPAddress
        {
            get { return serverIPAddress; }
        }
        private string serverIPAddress;
        public string Nick { get { return nick; } }
        private string nick;
        public int ServerPort
        {
            get { return serverPort; }
            set { serverPort = value; }
        }
        private int serverPort;
        public event dgConnectionClosed CloseConnected;
        public event dgNewMessageReceived NewMessgeReceived;

        private Socket clientConnection;
        private NetworkStream networkStream;
        private BinaryWriter binaryWriter;
        private BinaryReader binaryReader;
        private Thread thread;
        private volatile bool working = false;
        public ChatClient(string serverIPAddress, int serverPort, string nick)
        {
            this.nick = nick;
            this.serverIPAddress = serverIPAddress;
            this.serverPort = serverPort;
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
                thread = new Thread(new ThreadStart(wRun));
                working = true;
                thread.Start();

                SetNick(nick);
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
                working = false;
                clientConnection.Close();
                thread.Join();
            }
            catch (Exception)
            {

            }
        }

        public bool SendMessage(string message)
        {
            return sendCommand(Cmd.Message, message);
        }

        public bool SetNick(string nickName)
        {
            nick = nickName;
            return sendCommand(Cmd.SetNick, nickName);
        }

        private bool sendCommand(Cmd cmd, string content)
        {
            try
            {
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

        private void wRun()
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
                    string resul = Encoding.BigEndianUnicode.GetString(bList.ToArray());
                    Command command = JsonConvert.DeserializeObject<Command>(resul);
                    switch (command.Cmd)
                    {
                        case Cmd.Message:
                            yeniMesajAlindiTetikle(command.Content);
                            break;
                        case Cmd.Login:
                            break;
                        case Cmd.Logout:
                            break;
                        case Cmd.SetNick:
                            break;
                        case Cmd.Command:
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
            closeConnected_event();
        }

        private void closeConnected_event()
        {
            if (CloseConnected != null)
                CloseConnected();
        }

        private void yeniMesajAlindiTetikle(string message)
        {
            if (NewMessgeReceived != null)
                NewMessgeReceived(new MessageReceivingArguments(message));
        }
    }
}
