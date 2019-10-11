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
            Client = new ClientConnection(this, Disconnected);
            Logger.LogMsg("Client started");
        }

        public ClientConnection Client;

        Task Disconnected(Connection conn, Exception ex)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action(() =>
                    {
                        Disconnected(conn, ex);
                    }));
                } catch { }
                return Task.CompletedTask;
            }

            ChatProgram.Menu.Client = null;
            ChatProgram.Menu.INSTANCE.Show();
            this.Close();
            MessageBox.Show((ex?.Message ?? "Client disconnected from server connection for an unknown reason"), "Disconnected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return Task.CompletedTask;
        }

        public bool Connect(IPAddress ip)
        {
            Logger.LogMsg($"Connecting {ip}");
            try
            {
                Client.Client.Connect(ip, Program.Port);
            }
            catch (Exception ex)
            {
                Logger.LogMsg(ex.ToString(), LogSeverity.Warning);
            }
            if(Client.Client.Connected)
            {
                Logger.LogMsg("Connected");
                Client.NewMessage += Client_NewMessage;
                Client.NewUser += Client_UserListChange;
                Client.IdentityKnown += Client_UserListChange;
                Client.UserUpdate += Client_UserListChange;
                Client.UserDisconnected += Client_UserDisconnected;
                Client.Send(Environment.UserName);
                Logger.LogMsg("Sent username, opened listener");
                Client.Listen();
                Common.Users[999] = new User() { Id = 999, Name = "Server" };
                this.Activated += ClientForm_Activated;
                return true;
            } else
            {
                MessageBox.Show($"Failed to connect to IP");
                this.Close();
                return false;
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
        private void Client_UserDisconnected(object sender, User e)
        {
            Common.Users.Remove(e.Id);
            Client_UserListChange(this, null);
        }

        private void Client_UserListChange(object sender, Classes.User e)
        {
            if(e != null)
                Common.Users[e.Id] = e;
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
            int y_offset = y - gbMessages.VerticalScroll.Value;
            var lbl = new Label();
            lbl.Text = $"{message.Author.Name}: {message.Content}";
            lbl.Tag = message;
            lbl.AutoSize = true;
            lbl.MaximumSize = new Size(gbMessages.Size.Width - 15, 0);
            lbl.Location = new Point(5, y_offset);
            y += 20;
            return lbl;
        }
        public void ClientForm_Activated(object sender, EventArgs e)
        {
            uint latestMax = LAST_SEEN_MESSAGE;
            foreach(var control in gbMessages.Controls)
            {
                if(control is Label lbl)
                {
                    if(lbl.Tag is Classes.Message msg)
                    {
                        lbl.BackColor = Color.FromKnownColor(KnownColor.Control);
                        if(msg.Id > latestMax) // since no guarante of order
                            latestMax = msg.Id;
                    }
                }
            }
            if(LAST_SEEN_MESSAGE < latestMax) // hasnt changed in mean time
                LAST_SEEN_MESSAGE = latestMax;
        }
        int MESSAGE_Y = 5;
        public uint LAST_SEEN_MESSAGE = 0;
        private void Client_NewMessage(object sender, Classes.Message e)
        {
            var lbl = getLabelFor(e, ref MESSAGE_Y);
            lbl.ForeColor = e.Colour;
            lbl.Click += Lbl_Click;
            if(Form.ActiveForm == this)
            {
                LAST_SEEN_MESSAGE = e.Id;
            } else
            {
                lbl.BackColor = Color.LightCoral;
                if(e.Author.Id != Client.CurrentUser.Id)
                {
                    var notForm = new NotificationForm(this);
                    notForm.Show(e);
                }
            }
            this.gbMessages.Controls.Add(lbl);
            int charactors = lbl.Text.Length;
            var rows = charactors / 80d;
            while(rows > 0)
            {
                MESSAGE_Y += 5;
                rows--;
            }
        }

        private void Lbl_Click(object sender, EventArgs e)
        {
            if(sender is Label lbl && lbl.Tag is Classes.Message msg)
            {
                MessageBox.Show(msg.Content, $"#{msg.Id} from {msg.Author.Name}");
            }
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                var msg = new Classes.Message() { Author = Client.CurrentUser, Content = txtMessage.Text };
                txtMessage.Text = "";
                var pcket = new Packet(PacketId.SendMessage, msg.ToJson());
                Client.Send(pcket.ToString());
            }
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {

        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                var packet = new Packet(PacketId.Disconnect, new Newtonsoft.Json.Linq.JObject());
                Client.Send(packet.ToString());
            } catch { }
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Client.Client.Close();
            }
            catch { }
            ChatProgram.Menu.Client = null;
            ChatProgram.Menu.INSTANCE.Show();
            Client = null;
        }
    }
}
