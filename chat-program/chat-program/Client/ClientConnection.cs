using ChatProgram.Classes;
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

        public User CurrentUser;

        public ClientConnection(ClientForm form) : base ("Server")
        {
            Form = form;
            base.Receieved += parseJson;
        }

        void parseJson(object sender, string json)
        {
            Logger.LogMsg("Recievd: " + json);
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
            }
        }
    }
}
