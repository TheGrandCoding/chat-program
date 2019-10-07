using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Commands
{
    public abstract class TypeReader
    {
        public abstract Task<TypeReaderResult> ReadAsync(CommandContext context, string input);
    }
}
