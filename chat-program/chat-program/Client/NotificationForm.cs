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
        ClientForm Client;
        public NotificationForm(ClientForm form)
        {
            Client = form;
            InitializeComponent();
        }

        public void Show(Classes.Message message)
        {
            lblFrom.Text = $"From {message.Author.Name}";
            lblMessage.Text = message.Content;
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
                txtReply.Text =  "";
            }
        }

        int REMAINING = 30;
        private void timeoutTimer_Tick(object sender, EventArgs e)
        {
            lblTimeout.Text = $"{(REMAINING--).ToString()}";
            if(REMAINING < 0)
            {
                this.Close();
            }
        }

        private void txtReply_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtReply.Text))
                REMAINING = 30;
        }
    }
}
