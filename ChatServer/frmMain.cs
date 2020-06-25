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
        Chat.Core.Server.ChatServer sunucu;
        bool start = false;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            sunucu = new Chat.Core.Server.ChatServer(12079);
            sunucu.NewMessageReceivedFromClient += new dgNewMessageReceivedFromClient(newMessageReceivedFromClient);
            sunucu.ClientConnectionClosed += new dgClientConnectionClosed(clientConnectionClosed);
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
            txtMessages.Text += $"\n{e.Client.Nick} {e.Message}";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!start)
                {
                    sunucu.Start();
                    btnStart.Text = "Stop";
                    btnStart.BackColor = Color.Crimson;
                    BackColor = Color.GhostWhite;
                }
                else
                {
                    sunucu.Stop();
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
    }

    class ListClientItem
    {
        public long Id { get; set; }
        public string Nick { get; set; }
        public string IPAddress { get; set; }
    }
}
