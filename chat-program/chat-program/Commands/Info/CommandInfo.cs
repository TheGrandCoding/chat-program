using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Commands
{
    [DebuggerDisplay("{Name,nq}")]
    public class CommandInfo
    {
        private readonly Func<CommandContext, object[], CommandInfo, Task> _action;

        private readonly CommandService _commandService;
        public ModuleInfo Module { get; }
        public string Name { get; }
        public string Summary { get; }
        public int Priority { get; }
        public List<string> Aliases { get; }
        public List<ParamaterInfo> Paramaters { get; }
        public List<Attribute> Attributes { get; }

        internal CommandInfo(CommandBuilder builder, ModuleInfo module, CommandService service)
        {

            Module = module;

            Name = builder.Name;
            Summary = builder.Summary;
            Priority = builder.Priority;

            Aliases = module.Aliases
                .Permutate(builder.Aliases, (first, second) =>
                {
                    if (first == "")
                        return second;
                    else if (second == "")
                        return first;
                    else
                        return first + service._separatorChar + second;
                })
                .Select(x => service._caseSensitive ? x : x.ToLowerInvariant())
                .ToImmutableArray();

            Attributes = builder.Attributes.ToImmutableArray();

            Paramaters = builder.Parameters.Select(x => x.Build(this)).ToImmutableArray();
            _action = builder.Callback;
            _commandService = service;
        }
    }
}
