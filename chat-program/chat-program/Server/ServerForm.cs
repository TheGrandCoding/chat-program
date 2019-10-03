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
            Server = new ConnectionManager();
            Server.Start();
            Server.NewMessage += Server_NewMessage;
            SERVERUSER = new User();
            SERVERUSER.Id = 999;
            SERVERUSER.Name = "Server";
            Common.Users[SERVERUSER.Id] = SERVERUSER;
        }

        public User SERVERUSER;

        private void Server_NewMessage(object sender, Classes.Message e)
        {
            MessageBox.Show(e.Content);
        }

        public ConnectionManager Server;

        private void txtMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Server.Broadcast(new Packet(PacketId.NewMessage, new Classes.Message() { Author = SERVERUSER, Content = txtMessage.Text }.ToJson()));
            }
        }
    }
}
