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
                Server.Broadcast(new Packet(PacketId.NewMessage, msg.ToJson()));
                Server._internalServerMessage(msg);
                txtMessage.Text = "";
            }
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            gbMessages.BringToFront();
        }
    }
}
