using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Server.Commands
{
    public class RequireServerAttribute : PreconditionAttribute
    {
        public override Result CheckPrecondition(CommandContext context)
        {
            return new Result(context.User.Id == context.ServerUser.Id, "Command only available for Server");
        }
    }
}
