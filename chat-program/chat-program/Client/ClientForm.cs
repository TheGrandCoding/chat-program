using ChatProgram.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatProgram.Client
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
            Client = new ClientConnection(this);
            Logger.LogMsg("Client started");
        }

        public ClientConnection Client;

        public void Connect(IPAddress ip)
        {
            Logger.LogMsg($"Connecting {ip}");
            Client.Client.Connect(ip, Program.Port);
            if(Client.Client.Connected)
            {
                Logger.LogMsg("Connected");
                Client.NewMessage += Client_NewMessage;
                Client.NewUser += Client_NewUser;
                Client.IdentityKnown += Client_NewUser;
                Client.Send(Environment.UserName);
                Logger.LogMsg("Sent username, opened listener");
                Client.Listen();
            } else
            {
                Logger.LogMsg("Failed connect");
            }
        }


        Label createLabelFor(User u, ref int y)
        {
            int x = 5;
            var label = new Label();
            label.Tag = u;
            label.Text = $"#{u.Id} {u.Name}";
            label.Location = new Point(x, y);
            y += 30;
            return label;
        }

        private void Client_NewUser(object sender, Classes.User e)
        {
            gbUsers.Controls.Clear();
            var users = Common.Users.OrderBy(x => x.Key);
            int y = 15;
            foreach (var user in users)
            {
                var lbl = createLabelFor(user.Value, ref y);
                lbl.TextAlign = ContentAlignment.TopCenter;
                gbUsers.Controls.Add(lbl);
                lbl.Click += user_click;
            }
        }

        private void user_click(object sender, EventArgs e)
        {
            if (sender is Label lbl && e is MouseEventArgs me)
            {
                if (me.Button == MouseButtons.Right)
                    lbl.BackColor = Color.LightBlue;
            }
        }

        Label getLabelFor(Classes.Message message, ref int y)
        {
            var lbl = new Label();
            lbl.Text = $"{message.Author.Name}: {message.Content}";
            lbl.Tag = message;
            lbl.AutoSize = true;
            lbl.MaximumSize = new Size(gbMessages.Size.Width - 15, 0);
            lbl.Location = new Point(5, y);
            y += 20;
            return lbl;
        }

        int MESSAGE_Y = 5;
        private void Client_NewMessage(object sender, Classes.Message e)
        {
            var lbl = getLabelFor(e, ref MESSAGE_Y);
            lbl.Click += Lbl_Click;
            this.gbMessages.Controls.Add(lbl);
        }

        private void Lbl_Click(object sender, EventArgs e)
        {
            if(sender is Label lbl && lbl.Tag is Classes.Message msg)
            {
                MessageBox.Show(msg.Author.Name, msg.Id.ToString());
            }
        }

        private void txtMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var msg = new Classes.Message() { Author = Client.CurrentUser, Content = txtMessage.Text };
                txtMessage.Text = "";
                var pcket = new Packet(PacketId.SendMessage, msg.ToJson());
                Client.Send(pcket.ToString());
            }
        }
    }
}
