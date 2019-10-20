using ChatProgram.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatProgram.Server
{
    public partial class ServerForm : Form
    {
        public CommandManager Commands;
        public ServerForm()
        {
            InitializeComponent();
            Logger.LogMsg("Server start init");
            Server = new ConnectionManager(this);
            Server.Start();
            Server.NewMessage += Server_NewMessage;
            Server.NewUser += Server_NewUser;
            SERVERUSER = new User();
            SERVERUSER.Id = 999;
            SERVERUSER.UserName = "Server";
            Common.Users[SERVERUSER.Id] = SERVERUSER;

            Commands = new CommandManager();
            Commands.LoadCommands();
            Commands.LoadTypeParsers();

            Logger.LogMsg("Server start finished");
        }

        delegate void SafeCall(Action x);


        Label createLabelFor(User u, ref int y)
        {
            int x = 5;
            var label = new Label();
            label.Tag = u;
            label.Text = $"#{u.Id} {u.UserName}";
            label.Location = new Point(x, y);
            y += 30;
            return label;
        }

        private void Server_NewUser(object sender, User e)
        {
            gbUsers.Controls.Clear();
            var users = Common.Users.OrderBy(x => x.Key);
            int y = 15;
            foreach(var user in users)
            {
                var lbl = createLabelFor(user.Value, ref y);
                lbl.TextAlign = ContentAlignment.TopCenter;
                gbUsers.Controls.Add(lbl);
                lbl.Click += user_click;
            }

            var msg = new Classes.Message();
            msg.Author = SERVERUSER;
            msg.Id = Common.IterateMessageId();
            msg.Content = $"{e.UserName} ({e.Id}) has connected";
            msg.Colour = Color.Red;
            Server.Broadcast(new Packet(PacketId.NewMessage, msg.ToJson()));
            Server_NewMessage(this, msg);
        }

        private void user_click(object sender, EventArgs e)
        {
            if(sender is Label lbl && e is MouseEventArgs me)
            {
                if(me.Button == MouseButtons.Right)
                    lbl.BackColor = Color.LightBlue;
            }
        }

        public User SERVERUSER;

        Label getLabelFor(Classes.Message message, ref int y)
        {
            int y_offset = y - gbMessages.VerticalScroll.Value;
            var lbl = new Label();
            lbl.Text = $"{message.Author.UserName}: {message.Content}";
            lbl.Tag = message;
            lbl.AutoSize = true;
            lbl.MaximumSize = new Size(gbMessages.Size.Width - 15, 0);
            lbl.Location = new Point(5, y_offset);
            y += 5;
            return lbl;
        }

        int MESSAGE_Y = 5;
        private void Server_NewMessage(object sender, Classes.Message e)
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new Action(() => { Server_NewMessage(sender, e); }));
                return;
            }
            var lbl = getLabelFor(e, ref MESSAGE_Y);
            int height = lbl.Height;
            if (e.Content.StartsWith("/") && e.Colour == Color.Black)
                lbl.ForeColor = Color.Coral;
            else
                lbl.ForeColor = e.Colour;
            lbl.Click += msg_click;
            gbMessages.Controls.Add(lbl);
            height = lbl.Height;
            MESSAGE_Y += height;
        }

        private void msg_click(object sender, EventArgs e)
        {
            if(sender is Label lbl && lbl.Tag is Classes.Message msg)
            {
                MessageBox.Show(msg.Id.ToString());
            }
        }

        public ConnectionManager Server;

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                var msg = new Classes.Message() { Author = SERVERUSER, Content = txtMessage.Text };
                msg.Id = Common.IterateMessageId();
                Server._internalServerMessage(msg);
                if(txtMessage.Text.StartsWith("/"))
                {
                    Commands.Execute(msg);
                } else
                {
                    Server.Broadcast(new Packet(PacketId.NewMessage, msg.ToJson()));
                }
                txtMessage.Text = "";
            }
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            this.Text = Program.GetIPAddress();
            gbMessages.BringToFront();
            var token = Program.GetRegistry("apiKey", "");
            if(string.IsNullOrWhiteSpace(token))
            {
                var msg = new Classes.Message();
                msg.Author = SERVERUSER;
                msg.Colour = Color.Blue;
                msg.Content = $"You need to enter the server's API token. Use /token [value]";
                Server_NewMessage(this, msg);
            } else
            {
                var th = new System.Threading.Thread(() => doSetIPOnBot(token));
                th.Start();
                var msg = new Classes.Message();
                msg.Author = SERVERUSER;
                msg.Colour = Color.Blue;
                msg.Content = $"Attempting to set IP as {Program.GetIPAddress()} with '{token}'";
                Server_NewMessage(this, msg);
            }
        }

        void doSetIPOnBot(string token)
        {
            var msg = new Classes.Message();
            msg.Author = SERVERUSER;
            msg.Colour = Color.Blue;
            msg.Content = "Internal error occured when performing api POST.";
            try
            { 
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                var ip = Program.GetIPAddress();
                using(HttpClient client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, $"{Program.APIBASE}/chat/ip?value={ip}&token={token}");
                    var response = client.SendAsync(request).Result;
                    if(response.IsSuccessStatusCode)
                    {
                        msg.Content = $"API reports IP was correctly set.";
                    } else
                    {
                        msg.Colour = Color.DarkCyan;
                        msg.Content = $"API reports error: {response.StatusCode} :: {response.Content.ReadAsStringAsync().Result}";
                    }
                }
            } catch (Exception ex)
            {
                msg.Content = ex.Message;
            }
            Server_NewMessage(this, msg);
        }

        private void heartBeatTimer_Tick(object sender, EventArgs e)
        {
            var packet = new Packet(PacketId.HEARTBEAT, new Newtonsoft.Json.Linq.JObject());
            Server.Broadcast(packet);
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                var packet = new Packet(PacketId.Disconnect, new Newtonsoft.Json.Linq.JObject());
                Server.Broadcast(packet);
            } catch { }
            try
            {
                Server.Listening = false;
            }
            catch { }
        }

        private void ServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Server.Server.Stop();
            } catch { }
            ChatProgram.Menu.Server = null;
            Server = null;
            ChatProgram.Menu.INSTANCE.Show();
        }

        const int WM_SYSCOMMAND = 0x0112, SC_MONITORPOWER = 0xF170;

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            Logger.LogMsg($"{m} {m.Msg}");
            if (m.Msg == WM_SYSCOMMAND) //Intercept System Command
            {
                if ((m.WParam.ToInt32() & 0xFFF0) == SC_MONITORPOWER)
                { //Intercept Monitor Power Message
                    Logger.LogMsg($"Monitor was turned off");
                }

            }
            base.WndProc(ref m);
        }
    }
}
