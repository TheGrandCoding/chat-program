using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Server.TypeParsers
{
    public class UserTypeParser : TypeParser
    {
        public override TypeParseResult Parse(CommandContext context, string text)
        {
            if(uint.TryParse(text, out var id))
            {
                var u = Common.GetUser(id);
                return u == null ? new TypeParseResult($"No user with id {id}") : new TypeParseResult(u);
            }
            else
            {
                var u = Common.GetUser(text);
                return u == null ? new TypeParseResult($"No user with name {text}") : new TypeParseResult(u);
            }
        }
    }
}
