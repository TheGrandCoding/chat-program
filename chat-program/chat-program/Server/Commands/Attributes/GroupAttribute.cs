using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Server.Commands
{
    public class GroupAttribute : ValueStringAttribute
    {
        public GroupAttribute(string value) : base(value)
        {
        }
    }
}
