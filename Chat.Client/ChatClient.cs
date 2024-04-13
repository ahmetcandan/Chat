using Chat.Abstraction.Cryptography;
using Chat.Abstraction.Enum;
using Chat.Abstraction.Event;
using Chat.Abstraction.Model;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Chat.Client;

public class ChatClient
{
    private readonly string _privateKey;
    private readonly string _publicKey;

    public ClientStatus Status { get; private set; }

    public void SetStatus(ClientStatus status)
    {
        Status = status;
        SendCommand(Cmd.SetStatus, ((int)status).ToString());
    }

    public bool BlockStatus { get; private set; } = false;

    public string ServerIPAddress { get; }

    public string Nick { get; private set; }

    public long ClientId { get; private set; }

    public string ClientIPAddress { get; }

    private List<ClientItem> _clients = [];
    public int ServerPort { get; set; }

    public event dgNewMessageReceived NewMessgeReceived;
    public event dgClientListRefresh ClientListRefresh;
    public event dgServerStopped ServerStopped;

    private Socket _clientConnection;
    private NetworkStream _networkStream;
    private BinaryWriter _binaryWriter;
    private BinaryReader _binaryReader;
    private Thread _thread;
    private volatile bool _working = false;
    public ChatClient(string serverIPAddress, int serverPort, string nick, string clientIPAddress)
    {
        var rsa = new RSACryptoServiceProvider();
        _privateKey = rsa.ToXmlString(true);
        _publicKey = rsa.ToXmlString(false);
        Nick = nick;
        ServerIPAddress = serverIPAddress;
        ServerPort = serverPort;
        ClientIPAddress = clientIPAddress;
    }

    public bool Connect()
    {
        try
        {
            _clientConnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipep = new(IPAddress.Parse(ServerIPAddress), ServerPort);
            _clientConnection.Connect(ipep);
            _networkStream = new NetworkStream(_clientConnection);
            _binaryReader = new BinaryReader(_networkStream, Encoding.BigEndianUnicode);
            _binaryWriter = new BinaryWriter(_networkStream, Encoding.BigEndianUnicode);
            _thread = new Thread(new ThreadStart(TRun));
            _working = true;
            _thread.Start();
            Login(new ClientItem(0, Nick, ClientIPAddress, _publicKey, Status));
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
            SendCommand(Cmd.Logout);
            Thread.Sleep(100);
            _working = false;
            _clientConnection.Close();
            _thread.Join();
        }
        catch (Exception)
        {

        }
    }

    private bool Login(ClientItem client)
    {
        Status = ClientStatus.Available;
        return SendCommand(Cmd.Login, JsonConvert.SerializeObject(client));
    }

    public bool SendMessage(string message)
    {
        if (SendCommand(Cmd.Message, JsonConvert.SerializeObject(new Message(ClientId, message))))
        {
            NewMessageReceivedTrigger(new Message(ClientId, message));
            return true;
        }
        return false;
    }

    public bool SendMessage(string message, long toClientId)
    {
        var toClient = _clients.First(c => c.ClientId == toClientId);
        if (SendCommand(Cmd.Message, JsonConvert.SerializeObject(new Message(ClientId, toClientId, message.Encrypt(toClient.PublicKey)))))
        {
            NewMessageReceivedTrigger(new Message(ClientId, toClientId, message));
            return true;
        }
        return false;
    }

    public bool SetNick(string nickName)
    {
        Nick = nickName;
        return SendCommand(Cmd.SetNick, nickName);
    }

    private bool SendCommand(Cmd cmd)
    {
        return SendCommand(cmd, string.Empty);
    }

    private bool SendCommand(Cmd cmd, string content)
    {
        try
        {
            if (BlockStatus && cmd == Cmd.Message)
                return false;
            string result = JsonConvert.SerializeObject(new Command { Cmd = cmd, Content = content });
            _binaryWriter.Write(result);
            _networkStream.Flush();
            return true;
        }
        catch (Exception)
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
                try
                {
                    string result = receivedMessage;
                    Command command = JsonConvert.DeserializeObject<Command>(result);
                    ReceivedCommand(command);
                }
                catch { }
            }
            catch (Exception)
            {
                break;
            }
        }
        _working = false;
    }

    private void ReceivedCommand(Command command)
    {
        switch (command.Cmd)
        {
            case Cmd.Message:
                Message message = JsonConvert.DeserializeObject<Message>(command.Content);
                if (message.To == ClientId)
                {
                    if (string.IsNullOrWhiteSpace(message.Content) && message.EncryptContent.Length > 0)
                    {
                        message.Content = message.EncryptContent.Decrypt(_privateKey);
                    }
                }
                if (message.To == 0 || message.To == ClientId || message.From == ClientId)
                    NewMessageReceivedTrigger(message);
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
                _clients = response.Clients;
                ClientListRefreshTrigger(response);
                break;
            case Cmd.ServerStop:
                ServerStoppedTrigger();
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

    private void NewMessageReceivedTrigger(Message message)
    {
        NewMessgeReceived?.Invoke(new MessageReceivingArguments(message));
    }

    private void ServerStoppedTrigger()
    {
        ServerStopped?.Invoke();
    }

    private void ClientListRefreshTrigger(ClientListResponse response)
    {
        ClientId = response.Client.ClientId;
        ClientListRefresh?.Invoke(response);
    }
}
