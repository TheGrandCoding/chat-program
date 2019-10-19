using ChatProgram.Classes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Server.Commands
{
    [Name("Muting Module")]
    public class AdminModule : CommandBase
    {
        [Name("mute")]
        [RequireServer]
        public void Mute(User user)
        {
            user.SavedValues["muted"] = "true";
            SendTo(user, $"You have been muted.\nYou will remain muted until manually unmuted", System.Drawing.Color.DarkRed);
            Reply("User was muted; use /unmute to unmute");
        }

        [Name("unmute")]
        [RequireServer]
        public void Unmute(User user)
        {
            user.SavedValues["muted"] = "false";
            SendTo(user, $"You have been unmuted\nYou are able to talk again.", System.Drawing.Color.Red);
            Reply("User was unmuted");
        }

        [Name("lockdown")]
        [RequireServer]
        public void LockAllClients()
        {
            var jobj = new JObject();
            jobj["state"] = false;
            var packet = new Packet(PacketId.SetMonitorState, jobj);
            Context.Server.Server.Broadcast(packet);
        }
    }
}
