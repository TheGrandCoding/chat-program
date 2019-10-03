﻿using ChatProgram.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatProgram.Server
{
    public class ConnectionManager
    {
        TcpListener Server = new TcpListener(IPAddress.Loopback, Program.Port);

        Dictionary<uint, Connection> Connections = new Dictionary<uint, Connection>();

        /// <summary>
        /// Client has sent this <see cref="Message"/> to be broadcasted
        /// </summary>
        public event EventHandler<Message> NewMessage;

        /// <summary>
        /// A new client has connected, this is it.
        /// </summary>
        public event EventHandler<User> NewUser;

        public void Start()
        {
            Server.Start();
            newClientThread = new Thread(newClientHandle);
            newClientThread.Start();
        }

        public void Broadcast(Packet packet)
        {
            foreach(var conn in Connections.Values)
            {
                conn.Send(packet.ToString());
            }
        }

        bool _listen;
        public bool Listening {  get
            {
                return _listen;
            }  set
            {
                _listen = value;
            }
        }

        uint _id = 0;

        Thread newClientThread;
        void newClientHandle()
        {
            do
            {
                TcpClient client = Server.AcceptTcpClient();
                var stream = client.GetStream();
                var bytes = new Byte[client.ReceiveBufferSize];
                stream.Read(bytes, 0, bytes.Length);
                var data = Encoding.UTF8.GetString(bytes);

                var nClient = new User();
                nClient.Id = _id++;
                nClient.Name = data;
                Common.Users[nClient.Id] = nClient;
                var conn = new Connection();
                Connections[nClient.Id] = conn;
                conn.Client = client;
                conn.Listen();

                NewUser?.Invoke(this, nClient);
            } while (_listen);
        }
    }
}