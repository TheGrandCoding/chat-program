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
        public ServerNetworkManager Network;
        public ServerForm()
        {
            InitializeComponent();
            Network = new ServerNetworkManager(this);
        }

        public event EventHandler<User> UserJoined;
        public event EventHandler<string> Message;
    }
}
