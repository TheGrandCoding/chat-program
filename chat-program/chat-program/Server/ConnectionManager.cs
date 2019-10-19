using ChatProgram.Classes;
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
        public ServerForm Form;
        public ConnectionManager(ServerForm form)
        {
            Form = form;
        }
        public TcpListener Server = new TcpListener(IPAddress.Loopback, Program.Port);

        Dictionary<uint, Connection> Connections = new Dictionary<uint, Connection>();

        /// <summary>
        /// Client has sent this <see cref="Message"/> to be broadcasted
        /// </summary>
        public event EventHandler<Message> NewMessage;

        public void _internalServerMessage(Message m)
        {
            if(Form.InvokeRequired)
            {
                Form.Invoke(new Action(() =>
                {
                    _internalServerMessage(m);
                }));
                return;
            }
            NewMessage?.Invoke(this, m);
        }

        /// <summary>
        /// A new client has connected, this is it.
        /// </summary>
        public event EventHandler<User> NewUser;

        public event EventHandler<User> DisconnectUser;

        public void Start()
        {
            Server.Start();
            newClientThread = new Thread(newClientHandle);
            newClientThread.Start();
            Form.heartBeatTimer.Start();
        }

        public void Broadcast(Packet packet)
        {
            foreach(var conn in Connections.Values)
            {
                conn.Send(packet.ToString());
            }
        }

        public bool SendTo(uint id, Packet packet)
        {
            if(Connections.TryGetValue(id, out var conn))
            {
                conn.Send(packet.ToString());
                return true;
            }
            return false;
        }
        public bool SendTo(Classes.User usr, Packet packet)
        {
            return SendTo(usr.Id, packet);
        }

        public void PrivateMessage(Classes.User from, User to, string message)
        {
            var msg = new Message();
            msg.Author = Menu.Server.SERVERUSER;
            msg.Colour = System.Drawing.Color.Gold;
            msg.Content = $"[PM] {from.Name}({from.Id}) -> me: {message}";
            msg.Id = Common.IterateMessageId();
            SendTo(to, new Packet(PacketId.NewMessage, msg.ToJson()));

            msg.Colour = System.Drawing.Color.Gray;
            msg.Id = Common.IterateMessageId();
            msg.Content = $"[PM] me -> {to.Name}({to.Id}): {message}";
            SendTo(from, new Packet(PacketId.NewMessage, msg.ToJson()));

            msg.Id = Common.IterateMessageId();
            msg.Content = $"[PM] {from.Name}({from.Id}) -> {to.Name}({to.Id}): {message}";
            _internalServerMessage(msg);
        }

        bool _listen = true;
        public bool Listening {  get
            {
                return _listen;
            }  set
            {
                _listen = value;
            }
        }

        Thread newClientThread;
        void newClientHandle()
        {
            do
            {
                TcpClient client = Server.AcceptTcpClient();
                Logger.LogMsg("New TcpClient connected");
                var stream = client.GetStream();
                var bytes = new Byte[client.ReceiveBufferSize];
                stream.Read(bytes, 0, bytes.Length);
                var data = Encoding.UTF8.GetString(bytes);

                data = data.Replace("\0", "").Trim();
                data = data.Substring(1, data.Length - 2);

                var nClient = new User();
                nClient.Id = Common.USER_ID++;
                nClient.Name = data;
                Logger.LogMsg($"New User: '{data}' ({nClient.Id})");
                Common.Users[nClient.Id] = nClient;
                var conn = new Connection(nClient.Id.ToString(), HandleConnDisconnect);
                Connections[nClient.Id] = conn;
                conn.Client = client;
                conn.Listen();
                conn.Receieved += Conn_Receieved;
                var identity = new Packet(PacketId.GiveIdentity, nClient.ToJson());
                conn.Send(identity.ToString());

                Broadcast(new Packet(PacketId.UserJoined, nClient.ToJson()));
                if (Menu.Client != null)
                    Menu.Client.Client_UserListChange(this, nClient); // since it sometimes doesnt
                foreach (var id in Connections.Keys)
                {
                    if(Common.Users.TryGetValue(id, out var user))
                    {
                        // logically, this should be UserJoined, but for some reason
                        // that doesnt work, yet this does; so...
                        var packet = new Packet(PacketId.UserUpdate, user.ToJson());
                        conn.Send(packet.ToString());
                    }
                }

                Form.Invoke(new Action(() =>
                {
                    NewUser?.Invoke(this, nClient);
                }));
            } while (_listen);
        }

        private void Conn_Receieved(object sender, string e)
        {
            if(sender is Connection connection)
            {
                if(uint.TryParse(connection.Reference, out var id))
                {
                    if(Common.Users.TryGetValue(id, out var user))
                    {
                        Logger.LogMsg($"From {user.Name}({user.Id}): {e}");
                        Form.Invoke(new Action(() => {
                            var packet = new Packet(e);
                            HandleConnMessage(connection, user, packet);
                        }));
                    } else
                    {
                        Logger.LogMsg($"No User ({id}): {e}", LogSeverity.Warning);
                    }
                } else
                {
                    Logger.LogMsg($"No Reference ({connection.Reference}): {e}");
                }
            }
        }

        private void HandleConnMessage(Connection connection, User user, Packet packet)
        {
            if(packet.Id == PacketId.SendMessage)
            {
                var msg = new Message();
                msg.FromJson(packet.Information);
                msg.Id = Common.IterateMessageId();
                NewMessage?.Invoke(this, msg); // so Server can see all messages
                if(msg.Content.StartsWith("/"))
                { // command, so we dont broadcast to all users
                    try
                    {
                        Menu.Server.Commands.Execute(msg);
                    } catch (Exception ex)
                    {
                        Logger.LogMsg(ex.ToString(), LogSeverity.Error);
                        var reply = new Classes.Message();
                        reply.Author = Menu.Server.SERVERUSER;
                        reply.Id = Common.IterateMessageId();
                        reply.Colour = System.Drawing.Color.Red;
                        SendTo(user, new Packet(PacketId.NewMessage, reply.ToJson()));
                    }
                } else
                { // non-command, so we broadcast
                    var pong = new Packet(PacketId.NewMessage, msg.ToJson());
                    Broadcast(pong);
                }
            } else if (packet.Id == PacketId.Disconnect)
            {
                HandleConnDisconnect(connection, new Exception("User self-disconnected"));
            }
        }

        private Task HandleConnDisconnect(Connection connection, Exception error)
        {
            if(uint.TryParse(connection.Reference, out var id))
            {
                if(Common.Users.TryGetValue(id, out var user))
                {
                    Connections.Remove(id);
                    // Dont remove from users, since that might be helpful to keep
                    Logger.LogMsg($"Disconnect on {id} {user.Name}");
                    Form.Invoke(new Action(() => {
                        var msg = new Classes.Message();
                        msg.Content = $"{user.Name} ({user.Id}) has disconnected";
                        msg.Colour = System.Drawing.Color.Red;
                        msg.Author = Form.SERVERUSER;
                        msg.Id = Common.IterateMessageId();
                        NewMessage?.Invoke(this, msg);
                        var packet = new Packet(PacketId.NewMessage, msg.ToJson());
                        Broadcast(packet);

                        var leftPacket = new Packet(PacketId.UserLeft, user.ToJson());
                        Broadcast(leftPacket);
                    }));
                    return Task.CompletedTask;
                }
            }
            Logger.LogMsg($"Disconnect on {id}, no user available.");
            return Task.CompletedTask;
        }
    }
}
