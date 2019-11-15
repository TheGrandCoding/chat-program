using ChatProgram.Classes;
using Newtonsoft.Json.Linq;
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
        ClientForm Form;
        public event EventHandler<Message> NewMessage;
        public event EventHandler<User> NewUser;
        public event EventHandler<User> UserDisconnected;
        public event EventHandler<User> IdentityKnown;
        public event EventHandler<User> UserUpdate;
        public event EventHandler<uint> MessageDeleted;
        public event EventHandler<Message> MessagedEdited;
        public event EventHandler<Image> NewImageUploaded;
        public event EventHandler<UploadStatusEvent> UploadStatus;

        ManualResetEvent UserRespondWait = new ManualResetEvent(false);


        public event EventHandler<bool> SetMonitorState;

        public User CurrentUser;

        public ClientConnection(ClientForm form, Func<Connection, Exception, Task> callback) : base ("Server", callback)
        {
            Form = form;
            base.Receieved += parseJson;
        }

        public void GetUserFromServer(uint id)
        {
            var jobj = new JObject();
            jobj["id"]
            var packet = new Packet(PacketId.NeedUserInfo, )
            Send()
        }

        void parseJson(object sender, string json)
        {
            Logger.LogMsg("Recieved: " + json);
            var packet = new Packet(json);
            if (packet.Id == PacketId.NewMessage)
            {
                var msg = new Message();
                msg.FromJson(packet.Information);

                Form.Invoke(new Action(() =>
                {
                    NewMessage?.Invoke(this, msg);
                }));
            } else if (packet.Id == PacketId.GiveIdentity)
            {
                var usr = new User();
                usr.FromJson(packet.Information);
                this.CurrentUser = usr;

                Form.Invoke(new Action(() =>
                {
                    IdentityKnown?.Invoke(this, usr);
                }));
            } else if(packet.Id == PacketId.UserUpdate)
            {
                var usr = new User();
                usr.FromJson(packet.Information);
                Form.Invoke(new Action(() =>
                {
                    UserUpdate?.Invoke(this, usr);
                }));
            } else if(packet.Id == PacketId.UserLeft)
            {
                var usr = new User();
                usr.FromJson(packet.Information);
                Form.Invoke(new Action(() =>
                {
                    UserDisconnected?.Invoke(this, usr);
                }));
            } else if(packet.Id == PacketId.SetMonitorState)
            {
                var state = packet.Information["state"].ToObject<bool>();
                Form.Invoke(new Action(() =>
                {
                    SetMonitorState?.Invoke(this, state);
                }));
            } else if(packet.Id == PacketId.Disconnect)
            {
                this.Close();
            } else if(packet.Id == PacketId.MessageDeleted)
            {
                var id = packet.Information["id"].ToObject<uint>();
                Form.Invoke(new Action(() =>
                {
                    MessageDeleted?.Invoke(this, id);
                }));
            } else if(packet.Id == PacketId.ImageNeedSlice)
            { // Server needs us to send the slice.
                var id = packet.Information["id"].ToObject<uint>();
                var shiftedId = (uint)0;
                if(packet.Information.TryGetValue("originalId", out var val))
                {
                    shiftedId = id;
                    id = val.ToObject<uint>();
                }
                var sliceNum = packet.Information["slice"].ToObject<int>();
                if(Common.Images.TryGetValue(id, out var image))
                {
                    if(shiftedId > 0)
                    { // move it into the new location, remove old one. 
                        Common.Images[shiftedId] = image;
                    }
                    var slice = image.Slices[sliceNum];
                    var jobj = new JObject(packet.Information);
                    jobj["done"] = (sliceNum == image.Slices.Count - 1);
                    jobj["data"] = slice;
                    var pongPacket = new Packet(PacketId.ImageSlice, jobj);
                    Send(pongPacket.ToString());
                    Form.Invoke(new Action(() =>
                    {
                        UploadStatus?.Invoke(this, new UploadStatusEvent(image, sliceNum));
                    }));
                }
            } else if(packet.Id == PacketId.ImageInitialInformation)
            {
                var image = new Classes.Image();
                image.FromJson(packet.Information);
                if(Common.TryGetImage(image.Id, out var existingImage))
                {
                    if(existingImage.Slices.Count == image.MaximumSlices)
                    { // We already have this image, so we dont need to download it
                        Form.Invoke(new Action(() =>
                        {
                            NewImageUploaded?.Invoke(this, existingImage);
                        }));
                        return;
                    }
                }
                Common.AddImage(image);
                var jobj = new JObject();
                jobj["id"] = image.Id;
                jobj["slice"] = 0;
                var response = new Packet(PacketId.ImageNeedSlice, jobj);
                Send(response.ToString());
            } else if(packet.Id == PacketId.ImageSlice)
            {
                var id = packet.Information["id"].ToObject<uint>();
                var sliceNum = packet.Information["slice"].ToObject<int>();
                var done = packet.Information["done"].ToObject<bool>();
                var content = packet.Information["data"].ToObject<string>();
                if (Common.TryGetImage(id, out var image))
                {
                    image.SetSlice(sliceNum, content);
                    if (done)
                    {
                        Form.Invoke(new Action(() =>
                        {
                            NewImageUploaded?.Invoke(this, image);
                        }));
                    }
                    else
                    {
                        var jobj = new JObject();
                        jobj["id"] = image.Id;
                        jobj["slice"] = sliceNum + 1;
                        var pongPacket = new Packet(PacketId.ImageNeedSlice, jobj);
                        Send(pongPacket.ToString());
                        Form.Invoke(new Action(() =>
                        {
                            Form.pbProgressBar.Maximum = image.MaximumSlices;
                            Form.pbProgressBar.Value = sliceNum;
                            Form.pbProgressBar.Tag = $"Downloading image '{image.Name}'; {sliceNum}/{image.MaximumSlices} {((int)(sliceNum + 1 / image.MaximumSlices + 1))}";
                        }));
                    }
                }
            }
        }
    }
}
