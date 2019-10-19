using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Server.TypeParsers
{
    public abstract class TypeParser
    {
        public abstract TypeParseResult Parse(CommandContext context, string text);
    }

    public class TypeParseResult : Result
    {
        public object Object;
        public TypeParseResult(object obj) : base(true, null)
        {
            Object = obj;
        }
        public TypeParseResult(string err) : base(false, err)
        {

        }
    }
}
