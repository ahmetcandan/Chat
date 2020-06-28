using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chat.Core.Server;

namespace ChatServer
{
    public partial class frmMain : Form
    {
        Chat.Core.Server.ChatServer server;
        bool start = false;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void clientConnectionClosed(ClientConnectionArguments e)
        {
            Invoke(new dgClientConnectionClosed(closeConnection), e);
        }

        private void closeConnection(ClientConnectionArguments e)
        {

        }

        private void newMessageReceivedFromClient(ClientSendMessageArguments e)
        {
            Invoke(new dgNewMessageReceivedFromClient(newMessage), e);
        }

        private void newMessage(ClientSendMessageArguments e)
        {
            txtMessages.Text += $"\n{e.Client.Nick}: {e.Message.Content}";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!start)
                {
                    int portNo = 0;
                    if (string.IsNullOrEmpty(txtPortNo.Text))
                    {
                        errorProvider1.SetError(txtPortNo, "Cannot be empty");
                        return;
                    }
                    else if (!int.TryParse(txtPortNo.Text, out portNo))
                    {
                        errorProvider1.SetError(txtPortNo, "Not valid Port No");
                        return;
                    }
                    else if (portNo < 100 && portNo > 65535)
                    {
                        errorProvider1.SetError(txtPortNo, "Not valid Port No");
                        return;
                    }

                    server = new Chat.Core.Server.ChatServer(portNo);
                    server.NewMessageReceivedFromClient += new dgNewMessageReceivedFromClient(newMessageReceivedFromClient);
                    server.ClientConnectionClosed += new dgClientConnectionClosed(clientConnectionClosed);

                    server.Start();
                    btnStart.Text = "Stop";
                    btnStart.BackColor = Color.Crimson;
                    BackColor = Color.GhostWhite;
                }
                else
                {
                    server.Stop();
                    btnStart.Text = "Start";
                    btnStart.BackColor = Color.LimeGreen;
                    BackColor = Color.AliceBlue;
                }

                start = !start;
                txtPortNo.ReadOnly = start;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (start && server != null)
                server.Stop();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

    class ListClientItem
    {
        public long Id { get; set; }
        public string Nick { get; set; }
        public string IPAddress { get; set; }
    }
}
