using ChatProgram.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Server
{
    public class ServerNetworkManager
    {
        public ServerForm Server;
        public ServerNetworkManager(ServerForm server)
        {
            Server = server;
            Server.UserJoined += Server_UserJoined;
        }

        public Dictionary<uint, Connection> Connections = new Dictionary<uint, Connection>();

        void Broadcast(Packet packet, params User[] except)
        {
            foreach(var conn in Connections.Values)
            {
                conn.Send(packet.ToString());
            }
        }

        private void Server_UserJoined(object sender, Classes.User e)
        {
            var packet = new Packet(PacketId.UserJoined, e.ToJson());
        }
    }
}
