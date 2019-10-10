using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatProgram.Client
{
    public partial class NotificationForm : Form
    {


        public Point OpenLocation
        {
            get
            {
                var regX = Program.GetRegistry("notificationX", "-1");
                var regY = Program.GetRegistry("notificationY", "-1");
                if(int.TryParse(regX, out int x) && int.TryParse(regY, out int y))
                {
                    return new Point(x, y);
                }
                return new Point(-1, -1);
            } 
            set
            {
                Program.SetRegistry("notificationX", $"{value.X}");
                Program.SetRegistry("notificationY", $"{value.Y}");
            }
        }

        uint MESSAGE_ID;
        ClientForm Client;
        public NotificationForm(ClientForm form)
        {
            Client = form;
            InitializeComponent();
        }

        public void Show(Classes.Message message)
        {
            MESSAGE_ID = message.Id;
            lblFrom.Text = $"From {message.Author.Name}";
            lblMessage.Text = message.Content;
            timeoutTimer.Start();
            var loc = OpenLocation;
            this.SetDesktopLocation(loc.X, loc.Y);
            this.Show();
        }

        private void txtReply_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtReply.Text))
            {
                var msg = new Classes.Message();
                msg.Author = Client.Client.CurrentUser;
                msg.Content = txtReply.Text;
                var packet = new Classes.Packet(Classes.PacketId.SendMessage, msg.ToJson());
                Client.Client.Send(packet.ToString());
                this.Close(); // since we replied.
            }
        }

        int REMAINING = 30;
        int STARTED = 0;
        bool COLOR_CHANGE = true;
        private void timeoutTimer_Tick(object sender, EventArgs e)
        {
            STARTED = 1;
            var loc = OpenLocation;
            this.SetDesktopLocation(loc.X, loc.Y);
            lblTimeout.Text = $"{(REMAINING).ToString()}";
            if(REMAINING < 0)
            {
                this.Close();
            } else if (REMAINING > 20 && COLOR_CHANGE)
            {
                this.BackColor = (REMAINING % 2 == 0) ? Color.Red : Color.FromKnownColor(KnownColor.Control);
            }
            REMAINING--;
        }

        private void txtReply_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtReply.Text))
                REMAINING = 30;
            COLOR_CHANGE = false;
            this.BackColor = Color.FromKnownColor(KnownColor.Control);
            if (Client.LAST_SEEN_MESSAGE < MESSAGE_ID)
            {
                Client.LAST_SEEN_MESSAGE = MESSAGE_ID;
                Client.ClientForm_Activated(this, null);
            }
        }

        private void NotificationForm_LocationChanged(object sender, EventArgs e)
        {
            if(STARTED > 0)
            {
                OpenLocation = this.Location;
            }
        }

        private void NotificationForm_Activated(object sender, EventArgs e)
        {
            var loc = OpenLocation;
            this.SetDesktopLocation(loc.X, loc.Y);
        }

        private void NotificationForm_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
