using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Server.Commands
{
    public class PriorityAttribute : ValueIntAttribute
    {
        public PriorityAttribute(int value) : base(value)
        {
        }
    }
}
