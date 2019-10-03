﻿using ChatProgram.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatProgram.Client
{
    public class ClientConnection : Connection
    {
        public event EventHandler<Message> NewMessage;
        public event EventHandler<User> NewUser;
        public event EventHandler<User> UserDisconnected;

        public ClientConnection()
        {
            base.Receieved += parseJson;
        }

        void parseJson(object sender, string json)
        {
            var packet = new Packet(json);
            if (packet.Id == PacketId.NewMessage)
            {
                var msg = new Message();
                msg.FromJson(packet.Information);
                NewMessage?.Invoke(this, msg);
            }
        }
    }
}
