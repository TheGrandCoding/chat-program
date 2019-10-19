using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Server.TypeParsers
{

    public class StringTypeParser : TypeParser
    {
        public override TypeParseResult Parse(CommandContext context, string text)
        {
            return new TypeParseResult((object)text);
        }
    }

    public class IntTypeParser : TypeParser
    {
        public override TypeParseResult Parse(CommandContext context, string text)
        {
            if(int.TryParse(text, out var numb))
            {
                return new TypeParseResult(numb);
            } else
            {
                return new TypeParseResult($"Could not parse {text} to an integer");
            }
        }
    }

    public class BoolTypeParser : TypeParser
    {
        public override TypeParseResult Parse(CommandContext context, string text)
        {
            if (bool.TryParse(text, out var numb))
            {
                return new TypeParseResult(numb);
            }
            else
            {
                return new TypeParseResult($"Could not parse {text} to a boolean value");
            }
        }
    }
}
