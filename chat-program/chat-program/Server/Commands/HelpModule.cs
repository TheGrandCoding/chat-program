using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Server.Commands
{
    [Name("Help Module")]
    public class HelpModule : CommandBase
    {
        string buildParams(Command cmd)
        {
            string TEXT = "";
            foreach(var param in cmd.CommandMethod.GetParameters())
            {
                string INS = $"{param.ParameterType.Name} {param.Name}";
                if(param.IsOptional)
                {
                    INS += $" = {param.DefaultValue}";
                    TEXT += $"<{INS}> ";
                } else
                {
                    TEXT += $"[{INS}]";
                }
            }
            return TEXT;
        }

        [Name("help")]
        public void GetHelp()
        {
            int numCommands = 0;
            string TEXT = $"commands:\n";
            foreach(var group in Context.Server.Commands.Commands)
            {
                TEXT += $"== {group.Name} ==\n";
                foreach(var cmd in group.Children.OrderByDescending(x => x.Priority))
                {
                    TEXT += $"- {cmd.Parent.Group} {cmd.Name} {buildParams(cmd)}\n";
                    numCommands++;
                }
            }
            Reply($"{numCommands} {TEXT}", System.Drawing.Color.Cyan);
        }
    }
}
