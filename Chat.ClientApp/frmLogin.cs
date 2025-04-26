using Chat.Client;
using System.Net;

namespace Chat.ClientApp;

public partial class FrmLogin : Form
{
    public FrmLogin()
    {
        InitializeComponent();
    }

    private int _portNo = 0;

    private bool Validation()
    {
        bool result = true;
        if (string.IsNullOrEmpty(TxtIPAddress.Text))
        {
            ErrorProvider1.SetError(TxtIPAddress, "Cannot be empty");
            result = false;
        }
        if (string.IsNullOrEmpty(TxtNick.Text))
        {
            ErrorProvider1.SetError(TxtNick, "Cannot be empty");
            result = false;
        }
        if (string.IsNullOrEmpty(TxtPortNo.Text))
        {
            ErrorProvider1.SetError(TxtPortNo, "Cannot be empty");
            result = false;
        }
        else if (!int.TryParse(TxtPortNo.Text, out _portNo))
        {
            ErrorProvider1.SetError(TxtPortNo, "Not valid Port No");
            result = false;
        }
        else if (_portNo < 100 && _portNo > 65536)
        {
            ErrorProvider1.SetError(TxtPortNo, "Not valid Port No");
            result = false;
        }
        return result;
    }

    private void BtnConnect_Click(object sender, EventArgs e)
    {
        Connect();
    }

    private void Connect()
    {
        if (Validation())
        {
            try
            {
                string hostName = Dns.GetHostName();
                var addressList = Dns.GetHostEntry(hostName).AddressList;
                Session.Client = new ChatClient(TxtIPAddress.Text, _portNo, TxtNick.Text, addressList.Length > 0 ? addressList[0].ToString() : hostName);
                if (Session.Client.Connect())
                {
                    Session.HasConnection = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Connection to the server failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Session.HasConnection = false;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void Txt_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == 13)
            Connect();
    }
}
