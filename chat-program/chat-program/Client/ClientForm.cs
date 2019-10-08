﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatProgram.Client
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
            Client = new ClientConnection();
            Logger.LogMsg("Client started");
        }

        public ClientConnection Client;

        public void Connect(IPAddress ip)
        {
            Client.Client.Connect(ip, Program.Port);
            if(Client.Client.Connected)
            {
                Client.NewMessage += Client_NewMessage;
                Client.Send(Environment.UserName);
                Client.Listen();
            }
        }

        private void Client_NewMessage(object sender, Classes.Message e)
        {
            MessageBox.Show(e.Content, e.Author.Name);
        }
    }
}
