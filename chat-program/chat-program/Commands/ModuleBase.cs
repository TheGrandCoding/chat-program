using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Commands
{
    public class ModuleBase
    {
        public CommandContext Context { get; }

        protected virtual void ReplyPrivate(string message)
        {
            throw new NotImplementedException();
        }

        protected virtual void ReplyPublic(string message)
        {
            throw new NotImplementedException();
        }
    }
}
