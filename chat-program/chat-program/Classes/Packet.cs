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
        /// <summary>
        /// Sent to verify connection is still active
        /// </summary>
        HEARTBEAT,
        // -----------------------------
        // ---- From-Server Packets ----
        // -----------------------------
        /// <summary>
        /// Server indicates that a new message has been made
        /// <para>Content: <see cref="Message"/></para>
        /// </summary>
        NewMessage,
        /// <summary>
        /// Indicates that a user has joined the server
        /// <para>Content: <see cref="User"/></para>
        /// </summary>
        UserJoined,
        /// <summary>
        /// Indicates that a user has disconnected from the server
        /// <para>Content: <see cref="User"/></para>
        /// </summary>
        UserLeft,
        /// <summary>
        /// Indicates that a user has been modified in some way
        /// <para>Content: <see cref="User"/></para>
        /// </summary>
        UserUpdate,
        /// <summary>
        /// Indicates that a specific message has been modified in some way
        /// <para>Content: <see cref="ChatProgram.Classes.Message"/></para>
        /// </summary>
        MessageEdited,
        /// <summary>
        /// Informs a newly connected client of their user (thus, their id)
        /// <para>Content: <see cref="User"/></para>
        /// </summary>
        GiveIdentity,
        /// <summary>
        /// Authorative order from Server for the Client to set their monitor state
        /// <para>Content: <see cref="bool"/></para>
        /// </summary>
        SetMonitorState,
        // -----------------------------
        // ---- From-Client Packets ----
        // -----------------------------
        /// <summary>
        /// Informs Server that client is connecting
        /// </summary>
        [Obsolete]
        Connect,
        /// <summary>
        /// Informs the Server that the client is disconnecting
        /// <para>Content: null</para>
        /// </summary>
        Disconnect,
        /// <summary>
        /// Informs the Server that the client would like to post a message to the Server.
        /// <para>Content: <see cref="Message"/> (lacks proper Id, which is set by Server)</para>
        /// </summary>
        SendMessage
    }
}
