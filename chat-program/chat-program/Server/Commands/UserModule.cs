using ChatProgram.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Server.Commands
{
    [Name("User Module")]
    public class UserModule : CommandBase
    {
        [Name("nick")]
        public void SetNickName(string newNick)
        {
            if(string.IsNullOrWhiteSpace(newNick))
            {
                Context.User.NickName = null;
                Reply("Removed nickname");
            } else
            {
                if(newNick.Length > 32)
                {
                    Reply("Nickname is too long", Color.Red);
                    return;
                }
                Context.User.NickName = newNick;
                Reply("Nickname updated.");
            }
            var packet = new Packet(PacketId.UserUpdate, Context.User.ToJson());
            Context.Server.Server.Broadcast(packet);
        }

        [Name("clearnick")]
        [RequireServer]
        public void SetNickOther(User user)
        {
            user.NickName = null;
            var packet = new Packet(PacketId.UserUpdate, user.ToJson());
            Context.Server.Server.Broadcast(packet);
            SendTo(user, $"Your nickname has been removed", Color.Red);
            Reply("User nickname removed");
        }

        [Name("nick")]
        public void RemoveNick()
        {
            SetNickName(null);
        }
    }
}
