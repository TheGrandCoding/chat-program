using ChatProgram.Commands.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Commands
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ParamaterInfo
    {
        private readonly TypeReader _reader;

        public CommandInfo Command { get; }
        public string Name { get; }
        public string Summary { get; }
        public bool IsOptional { get; }
        public bool IsRemainder { get; }
        public bool IsMultiple { get; }
        public Type Type { get; }
        public object DefaultValue { get; }

        internal ParamaterInfo(ParamaterBuilder builder, CommandInfo command, CommandService serrvice)
        {
            Command = command;
            Name = builder.Name;
        }
    }
}
