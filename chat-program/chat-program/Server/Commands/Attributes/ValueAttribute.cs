using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Server.Commands
{
    public class ValueStringAttribute : Attribute
    {
        public readonly string Value;
        public ValueStringAttribute(string value)
        {
            Value = value;
        }
    }

    public class ValueIntAttribute : Attribute
    {
        public readonly int Value;
        public ValueIntAttribute(int value) { Value = value; }
    }
}
