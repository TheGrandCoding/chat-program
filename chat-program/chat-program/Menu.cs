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

namespace ChatProgram
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        public static Client.ClientForm Client;
        public static Server.ServerForm Server;

        private void btnHost_Click(object sender, EventArgs e)
        {
            Server = new Server.ServerForm();
            Server.Show();
            System.Threading.Thread.Sleep(1500);
            Client = new Client.ClientForm();
            Client.Connect(IPAddress.Parse("127.0.0.1"));
            Client.Show();
            this.Hide();
        }
    }
}
