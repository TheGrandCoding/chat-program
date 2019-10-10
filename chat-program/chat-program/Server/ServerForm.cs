using ChatProgram.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatProgram.Server
{
    public partial class ServerForm : Form
    {
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
            SERVERUSER.Name = "Server";
            Common.Users[SERVERUSER.Id] = SERVERUSER;
            Logger.LogMsg("Server start finished");
        }

        delegate void SafeCall(Action x);


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
            msg.Content = $"{e.Name} has connected";
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
            int y_offset = y - gbMessages.VerticalScroll.Value; ;
            var lbl = new Label();
            lbl.Text = $"{message.Author.Name}: {message.Content}";
            lbl.Tag = message;
            lbl.AutoSize = true;
            lbl.MaximumSize = new Size(gbMessages.Size.Width - 15, 0);
            lbl.Location = new Point(5, y_offset);
            y += 20;
            return lbl;
        }

        int MESSAGE_Y = 5;
        private void Server_NewMessage(object sender, Classes.Message e)
        {
            var lbl = getLabelFor(e, ref MESSAGE_Y);
            lbl.Click += msg_click;
            gbMessages.Controls.Add(lbl);
        }

        private void msg_click(object sender, EventArgs e)
        {
            if(sender is Label lbl && lbl.Tag is Classes.Message msg)
            {
                MessageBox.Show(msg.Id.ToString());
            }
        }

        public ConnectionManager Server;

        private void txtMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                var msg = new Classes.Message() { Author = SERVERUSER, Content = txtMessage.Text };
                msg.Id = Common.IterateMessageId();
                Server.Broadcast(new Packet(PacketId.NewMessage, msg.ToJson()));
                Server._internalServerMessage(msg);
                txtMessage.Text = "";
            }
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            gbMessages.BringToFront();
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
    }
}
