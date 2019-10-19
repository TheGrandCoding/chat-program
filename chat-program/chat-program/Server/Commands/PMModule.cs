using ChatProgram.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Server.Commands
{
    [Name("Private Messaging")]
    public class PMModule : CommandBase
    {
        [Name("pm")]
        public void Send(User user, string message)
        {
            Context.Server.Server.PrivateMessage(Context.User, user, message);
            Context.User.SavedValues["pm_reply"] = user.Id.ToString();
        }

        [Name("r")]
        public void Reply(string message)
        {
            if(Context.User.SavedValues.TryGetValue("pm_reply", out var idString))
            {
                if(uint.TryParse(idString, out var id))
                {
                    var u = Common.GetUser(id);
                    if(u != null)
                    {
                        Context.Server.Server.PrivateMessage(Context.User, u, message);
                    }
                }
            } else
            {
                Reply($"Error: you must use /pm first, then /r will work for that user until next /pm");
            }
        }
    }
}
