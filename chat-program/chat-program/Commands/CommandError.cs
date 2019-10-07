using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Commands
{
    public enum CommandError
    {
        UnknownCommand = 1,
        ParseFailed,
        BadArgCount,
        ObjectNotFound,
        MultipleMatches,
        UnmetPrecondition,
        Exception,
        Unsuccessful
    }
}
