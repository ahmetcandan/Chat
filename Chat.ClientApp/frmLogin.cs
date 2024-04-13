using Chat.Client;
using System.Net;

namespace Chat.ClientApp;

public partial class frmLogin : Form
{
    public frmLogin()
    {
        InitializeComponent();
    }

    private int _portNo = 0;

    private bool Validation()
    {
        bool result = true;
        if (string.IsNullOrEmpty(txtIPAddress.Text))
        {
            errorProvider1.SetError(txtIPAddress, "Cannot be empty");
            result = false;
        }
        if (string.IsNullOrEmpty(txtNick.Text))
        {
            errorProvider1.SetError(txtNick, "Cannot be empty");
            result = false;
        }
        if (string.IsNullOrEmpty(txtPortNo.Text))
        {
            errorProvider1.SetError(txtPortNo, "Cannot be empty");
            result = false;
        }
        else if (!int.TryParse(txtPortNo.Text, out _portNo))
        {
            errorProvider1.SetError(txtPortNo, "Not valid Port No");
            result = false;
        }
        else if (_portNo < 100 && _portNo > 65536)
        {
            errorProvider1.SetError(txtPortNo, "Not valid Port No");
            result = false;
        }
        return result;
    }

    private void BtnConnect_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            try
            {
                string hostName = Dns.GetHostName();
                var addressList = Dns.GetHostByName(hostName).AddressList;
                Session.Client = new ChatClient(txtIPAddress.Text, _portNo, txtNick.Text, addressList.Length > 0 ? addressList[0].ToString() : hostName);
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
}
