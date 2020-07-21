using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat
{
    public partial class MainForm : Form
    {
        int mode;
        Client client;
        ServerClient server;

        public MainForm()
        {
            InitializeComponent();
            mode = 0;
        }

        private void buttonServer_Click(object sender, EventArgs e)
        {
            if (mode == 0)
            {
                server = new ServerClient();
                server.InquireFunctions(MessageReceived, Connection, ClientDisconnection);
                server.InitNetworkAndStart(25255);
                labelMode.Text = "Server";
                labelMode.Visible = true;

                buttonServer.Enabled = false;
                buttonConnection.Enabled = false;
                AddMessage("Server started on port 25255", 1);

                mode = 1;
            }
        }

        private void buttonConnection_Click(object sender, EventArgs e)
        {
            if (mode == 0)
            {
                try
                {
                    client = new Client();
                    client.InquireFunctions(MessageReceived, ServerDisconnect);
                    client.StartClient("82.232.220.135", 25255);
                    labelMode.Text = "Client";
                    labelMode.Visible = true;

                    buttonServer.Enabled = false;
                    buttonConnection.Enabled = false;
                    mode = 2;

                    AddMessage("Connection to " + "82.232.220.135:" + 25255, 2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error -> ex");
                    AddMessage("Connection problem with the server ...", 2);
                }
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (Utils.IsEmptyProperty(textBoxMessage.Text))
            {
                return;
            }

            if (mode == 1)
            {
                AddMessage(textBoxMessage.Text, mode);
                server.SendMessage(textBoxMessage.Text);
            }
            else if (mode == 2)
            {
                AddMessage(textBoxMessage.Text, mode);
                client.SendMessage(textBoxMessage.Text);
            }

            if (mode != 0)
            {
                textBoxMessage.Text = "";
            }
        }

        private void ServerDisconnect(NetMessage message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<NetMessage>(ServerDisconnect), new object[] { });
                return;
            }

            client.Stop();
            AddMessage("Disconnection of server", 1);

            buttonServer.Enabled = true;
            buttonConnection.Enabled = true;

            mode = 0;
        }

        private void ClientDisconnection(NetMessage message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<NetMessage>(ClientDisconnection), new object[] { message });
                return;
            }
        }

        private void Connection(NetMessage message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<NetMessage>(Connection), new object[] { message });
                return;
            }

            AddMessage("Connection of " + message.Author, 1);
        }

        private void MessageReceived(NetMessage message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<NetMessage>(MessageReceived), new object[] { message });
                return;
            }
            Console.WriteLine(message);
            AddMessage(message.Message, 3 - mode);
        }

        private void AddMessage(string message, int mode)
        {
            if (mode == 1)
            {
                richTextBox.SelectionColor = Color.Black;
                richTextBox.AppendText("server > ");
            }
            else if (mode == 2)
            {
                richTextBox.SelectionColor = Color.Black;
                richTextBox.AppendText("client > ");
            }

            List<Tuple<Color, string>> parts = Utils.StringToColoredString(message, richTextBox.ForeColor);

            for (int i = 0; i < parts.Count; ++i)
            {
                richTextBox.SelectionColor = parts[i].Item1;
                richTextBox.AppendText(parts[i].Item2);
            }

            richTextBox.AppendText("\n");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mode == 1)
            {
                server.Stop();
            }
            else if (mode == 2)
            {
                client.Stop();
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !textBoxMessage.Focused)
            {
                textBoxMessage.Focus();
            }
        }

        private void textBoxMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSend_Click(this, e);
            }
        }
    }
}
