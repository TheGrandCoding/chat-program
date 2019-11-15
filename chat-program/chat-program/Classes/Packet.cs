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
        /// <summary>
        /// If sent by Server, Client should disconnect.
        /// If sent by Client, Server assumes Client is disconnecting
        /// <para>Content: null</para>
        /// </summary>
        Disconnect,
        /// <summary>
        /// If sent by Server, Client should send the requested slice
        /// If sent by Client, Server should send the requested slice.
        /// </summary>
        ImageNeedSlice,
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
        /// Indicates that a specific message has been removed
        /// <para>Content: <see cref="int"/> Id</para>
        /// </summary>
        MessageDeleted,
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
        /// <summary>
        /// Confers minmal information about an image, eg its Id, uploadedby/name
        /// </summary>
        ImageInitialInformation,
        /// <summary>
        /// Image slice responded to by request
        /// </summary>
        ImageSlice,
        /// <summary>
        /// Server is providing <see cref="User"/> information to Client
        /// </summary>
        RespondUserInfo,
        // -----------------------------
        // ---- From-Client Packets ----
        // -----------------------------
        /// <summary>
        /// Informs Server that client is connecting
        /// </summary>
        [Obsolete]
        Connect,
        /// <summary>
        /// Informs the Server that the client would like to post a message to the Server.
        /// <para>Content: <see cref="Message"/> (lacks proper Id, which is set by Server)</para>
        /// </summary>
        SendMessage,
        /// <summary>
        /// Informs Server that Client wishes to remove a given message
        /// <para>Content: <see cref="int"/> Id of Message</para>
        /// </summary>
        RequestDeleteMessage,
        /// <summary>
        /// Informs Server that Client wishes to edit a given message
        /// <para>Content: <see cref="Message"/> Id equal to existing, Content equal to new edited content</para>
        /// </summary>
        RequestEditMessage,
        /// <summary>
        /// Informs Server that we are uploading an image.
        /// <para>Content: <see cref="Image"/>Image to upload</para>
        /// </summary>
        RequestUploadImage,
        /// <summary>
        /// Sends the Server a slice of the Image.
        /// </summary>
        UploadImageSlice,
        /// <summary>
        /// Sends the Server indicating that it needs a <see cref="User"/> cache
        /// </summary>
        NeedUserInfo
    }
}
