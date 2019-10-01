using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Classes
{
    public class Packet
    {
        public Packet(PacketId id, JObject information)
        {
            Id = id;
            Information = information;
        }

        public Packet(string json)
        {
            var obj = JObject.Parse(json);
            Id = (PacketId)Enum.Parse(typeof(PacketId), obj["id"].ToObject<string>());
            Information = JObject.Parse(obj["content"].ToString());
        }

        public PacketId Id;
        public JObject Information;

        public override string ToString()
        {
            JObject obj = new JObject();
            obj["id"] = (int)Id;
            obj["content"] = Information;
            return obj.ToString();
        }
    }

    public enum PacketId
    {
        // From-Server Packets
        NewMessage,
        UserJoined,
        UserLeft,
        UserList,
        MessageEdited,
        // From-Client Packets
        Connect,
        Disconnect,
        SendMessage

    }
}
