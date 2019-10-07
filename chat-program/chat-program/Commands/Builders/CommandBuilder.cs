using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Commands.Builders
{
    public class CommandBuilder
    {

        private readonly List<ParamaterBuilder> _paramaters;
        private readonly List<Attribute> _attributes;
        private readonly List<string> _aliases;

        public ModuleBuilder Module { get; }
        internal Func<CommandContext, object[], CommandInfo, Task> Callback { get; set; }

        public string Name { get; set; }
        public string Summary { get; set; }
        public string PrimaryAlias { get; set; }
        public int Priority { get; set; }
        public bool IgnoreExtraArgs { get; set; }
        
        internal CommandBuilder(ModuleBuilder module)
        {
            Module = module;
            _paramaters = new List<ParamaterBuilder>();
            _attributes = new List<Attribute>();
            _aliases = new List<string>();
        }

        internal CommandInfo Build(ModuleInfo info, CommandService service)
        {
            //Default name to primary alias
            if (Name == null)
                Name = PrimaryAlias;

            if (_paramaters.Count > 0)
            {
                var lastParam = _paramaters[_paramaters.Count - 1];

                var firstMultipleParam = _paramaters.FirstOrDefault(x => x.IsMultiple);
                if ((firstMultipleParam != null) && (firstMultipleParam != lastParam))
                    throw new InvalidOperationException($"Only the last paramater in a command may have the Multiple flag. paramater: {firstMultipleParam.Name} in {PrimaryAlias}");

                var firstRemainderParam = _paramaters.FirstOrDefault(x => x.IsRemainder);
                if ((firstRemainderParam != null) && (firstRemainderParam != lastParam))
                    throw new InvalidOperationException($"Only the last paramater in a command may have the Remainder flag. paramater: {firstRemainderParam.Name} in {PrimaryAlias}");
            }

            return new CommandInfo(this, info, service);
        }

    }
}
