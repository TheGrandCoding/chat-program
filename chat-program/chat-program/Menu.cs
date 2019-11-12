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
            string input = Interaction.InputBox("Enter ze IP", "Schnell", Program.DefaultIP);
            if(IPAddress.TryParse(input, out var address))
            {
                if(Client.Connect(address))
                {
                    Client.Show();
                    this.Hide();
                } else
                {
                    Client.Close();
                    Client = null;
                }
            }
            else
            {
                MessageBox.Show("IPAddress could not be parsed from your input.\nPerhaps try to read the prompt next time, eh?");
            }
        }

        public void ButtonRefresh()
        {
            btnHost.Enabled = Server == null;
        }

        private void Menu_Activated(object sender, EventArgs e)
        {
            ButtonRefresh();
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(Server != null || Client != null)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void dgvServers_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            var row = dgvServers.Rows[e.RowIndex];
            var cell = row.Cells[2];
            if(IPAddress.TryParse(cell.Value.ToString(), out var ip))
            {
                Client = new Client.ClientForm();
                if (Client.Connect(ip))
                {
                    Client.Show();
                    this.Hide();
                }
                else
                {
                    Client.Close();
                    Client = null;
                }
            }
        }
    }
}
