using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatProgram.Classes;

namespace ChatProgram.Commands
{
    public class CommandContext
    {
        public User User { get; }
        public Message Message { get; }
        
        public CommandContext(User u, Message m)
        {
            User = u;
            Message = m;
        }
    }
}
