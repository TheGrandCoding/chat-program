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
using Microsoft.VisualBasic;

namespace ChatProgram
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            INSTANCE = this;
        }

        public static Menu INSTANCE;

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

        private void btnJoin_Click(object sender, EventArgs e)
        {
            Client = new Client.ClientForm();
            string input = Interaction.InputBox("Enter ze IP", "Schnell", "127.0.0.1");
            if(IPAddress.TryParse(input, out var address))
            {
                Client.Connect(address);
                Client.Show();
                this.Hide();
            } else
            {
                MessageBox.Show("IPAddress could not be parsed from your input.\nPerhaps try to read the prompt next time, eh?");
            }
        }

        private void Menu_Activated(object sender, EventArgs e)
        {
            btnHost.Enabled = Server == null;
            btnJoin.Enabled = Client == null;
        }
    }
}
