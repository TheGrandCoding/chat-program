using ChatProgram.Server.Commands;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Server
{
    public class CommandManager
    {
        public List<CommandGroup> Commands;
        public Dictionary<Type, TypeParsers.TypeParser> TypeParsers;

        List<Command> Search(string text)
        {
            List<Command> POSSIBLE_COMMANDS = new List<Command>();
            text = text.Substring(1); // remove /
            string[] splitith = text.Split(' ');
            foreach (var group in Commands)
            {
                string combined = "";
                int indexForGroup = -1;
                int counter = -1;
                foreach (var word in splitith)
                {
                    counter++;
                    combined += word.ToLower() + " ";
                    if (combined.StartsWith(group.Group))
                    {
                        indexForGroup = counter;
                        break;
                    }
                }
                if (group.Group == "" || indexForGroup > -1)
                {
                    var args = splitith.Skip(indexForGroup - 1);
                    foreach(var command in group.Children)
                    {
                        combined = "";
                        indexForGroup = -1;
                        counter = -1;
                        foreach(var word in args)
                        {
                            combined += word.ToLower() + " ";
                            if(combined.StartsWith(command.Name))
                            {
                                indexForGroup = counter;
                                POSSIBLE_COMMANDS.Add(command);
                                break;
                            }
                        }
                    }
                }
            }
            return POSSIBLE_COMMANDS;
        }

        object parseArg(CommandContext context, string text, ParameterInfo par)
        {
            if(TypeParsers.TryGetValue(par.ParameterType, out var parser))
            {
                var result = parser.Parse(context, text);
                if (result.IsSuccess)
                    return result.Object;
            }
            return null;
        }

        ParseParamaterResult ParseParamaters(CommandContext context, Command command, string[] arguments)
        {
            List<object> OUTPUTS = new List<object>();
            var parameters = command.CommandMethod.GetParameters();
            if (parameters.Count() > arguments.Count())
                return new ParseParamaterResult($"Expected at least {parameters.Count()} args, but {arguments.Count()} given");
            if (parameters.Count() == 0 && arguments.Count() > 0)
                return new ParseParamaterResult($"Expected no args, but {arguments.Count()} given");
            for (int i = 0; i < arguments.Count() && i < parameters.Count(); i++)
            {
                var arg = arguments.ElementAt(i);
                var par = parameters.ElementAt(i);
                if (i == parameters.Count() - 1 && par.ParameterType == typeof(string))
                { // this is last param, so we shall see if this be a string
                    // if it is, so we take all the remainder of the argument string given
                    var args = arguments.Skip(i);
                    string joined = string.Join(" ", args);
                    OUTPUTS.Add(joined);
                } else
                {
                    try
                    {
                        var parsed = parseArg(context, arg, par);
                        if (parsed != null)
                            OUTPUTS.Add(parsed);
                        else
                            return new ParseParamaterResult($"Could not parse {arg} to {par.ParameterType.FullName}");
                    } catch (Exception ex)
                    {
                        return new ParseParamaterResult($"{ex.Message}");
                    }
                }
            }
            return new ParseParamaterResult(OUTPUTS);
        }

        CommandValidResult checkSingularCmd(CommandContext context, Command command)
        {
            var pars = ParseParamaters(context, command, GetArguments(command, context.Message.Content));
            if (!pars.IsSuccess)
                return new CommandValidResult(pars.ErrorReason);

            foreach(var cond in command.Preconditions)
            {
                var rs = cond.CheckPrecondition(context);
                if(!rs.IsSuccess)
                {
                    return new CommandValidResult(rs.ErrorReason);
                }
            }

            return new CommandValidResult(pars);
        }

        List<CommandValidResult> FilterInvalids(CommandContext context, List<Command> commands)
        {
            if(commands.Count == 1)
            {
                var cmd = commands[0];
                var res = checkSingularCmd(context, cmd);
                if(!res.IsSuccess)
                {
                    ReplyToUser($"Error: {res.ErrorReason}", context.User);
                    return new List<CommandValidResult>();
                }
                return new List<CommandValidResult>() { new CommandValidResult(cmd, res.ParamResult)  };
            }
            var FILTERED = new List<CommandValidResult>();
            foreach(var cmd in commands)
            {
                var result = checkSingularCmd(context, cmd);
                if (result.IsSuccess)
                    FILTERED.Add(new CommandValidResult(cmd, result.ParamResult));
            }
            return FILTERED;
        }

        string[] GetArguments(Command cmd, string input)
        {
            if (input.StartsWith("/")) input = input.Substring(1);
            string starts = $"{(string.IsNullOrWhiteSpace(cmd.Parent.Group) ? "" : $"{cmd.Parent.Group} ")}{cmd.Name}";
            string argument = input.Replace(starts, "");
            return argument.Split(' ').Where(x => string.IsNullOrWhiteSpace(x) == false).ToArray();
        }

        void ReplyToUser(string message, Classes.User usr)
        {
            var msg = new Classes.Message();
            msg.Colour = message.StartsWith("Error:") ? Color.Red : Color.Black;
            msg.Author = Menu.Server.SERVERUSER;
            msg.Content = message;
            msg.Id = Common.IterateMessageId();
            Menu.Server.Server.SendTo(usr, new Classes.Packet(Classes.PacketId.NewMessage, msg.ToJson()));
        }

        public void Execute(Classes.Message message)
        {
            var context = new CommandContext(message.Author, message);
            var cmds = Search(message.Content);
            var valids = FilterInvalids(context, cmds);
            var first = valids.OrderByDescending(x => x.Command.Priority).FirstOrDefault();
            if(first == null)
            {
                ReplyToUser($"Unable to find any command that matches your input", context.User);
            } else
            {
                var command = first.Command;
                command.Invoke(context, first.ParamResult.Output.ToArray());
            }
        }

        public void LoadCommands()
        {
            Commands = new List<CommandGroup>();
            var type = typeof(CommandBase);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsSubclassOf(type));
            foreach(var groupType in types)
            {
                if (groupType.FullName == "ChatProgram.Server.CommandGroup")
                    continue;
                try
                {
                    var group = walkGroupTree(groupType);
                    Commands.Add(group);
                } catch (Exception ex)
                {
                    Logger.LogMsg($"{groupType.FullName}: {ex.ToString()}", LogSeverity.Error);
                }
            }
        }

        public void LoadTypeParsers()
        {
            TypeParsers = new Dictionary<Type, TypeParsers.TypeParser>();
            TypeParsers[typeof(string)] = new TypeParsers.StringTypeParser();
            TypeParsers[typeof(int)] = new TypeParsers.IntTypeParser();
            TypeParsers[typeof(bool)] = new TypeParsers.BoolTypeParser();
            TypeParsers[typeof(Classes.User)] = new TypeParsers.UserTypeParser();
        }

        static string GetNotNull(ValueStringAttribute attribute, string message)
        {
            if (attribute == null)
                throw new Exception(message);
            if (string.IsNullOrWhiteSpace(attribute.Value))
                throw new Exception(message);
            return attribute.Value;
        }

        CommandGroup walkGroupTree(Type groupType)
        {
            var group = new CommandGroup();
            var nameAttribute = groupType.GetCustomAttribute<NameAttribute>();
            var name = GetNotNull(nameAttribute, $"{groupType.FullName} has no Name attribute set");
            group.Name = name;
            var groupAttr = groupType.GetCustomAttribute<GroupAttribute>();
            group.Group = groupAttr?.Value ?? "";
            group.CommandClass = groupType;
            var methods = groupType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            group.Children = new List<Command>();
            foreach(var method in methods)
            {
                try
                {
                    walkCommand(group, method);
                } catch (Exception ex)
                {
                    Logger.LogMsg($"{name}/{method.Name}: {ex.ToString()}", LogSeverity.Error);
                }
            }
            return group;
        }

        void walkCommand(CommandGroup parent, MethodInfo method)
        {
            var cmd = new Command();
            cmd.Parent = parent;
            cmd.CommandMethod = method;
            cmd.CommandClass = parent.CommandClass;
            var nameAttr = method.GetCustomAttribute<NameAttribute>();
            var name = GetNotNull(nameAttr, $"{method.Name} cmd has no name");
            cmd.Name = name;
            var intAttr = method.GetCustomAttribute<PriorityAttribute>();
            cmd.Priority = (intAttr?.Value ?? 0);
            cmd.Preconditions = new List<PreconditionAttribute>();
            foreach (var attribute in method.GetCustomAttributes<PreconditionAttribute>())
                cmd.Preconditions.Add(attribute);
            parent.Children.Add(cmd);
        }
    }

    public class CommandContext
    {
        public ChatProgram.Classes.User User { get; }
        public ChatProgram.Classes.Message Message { get; }
        public Classes.User ServerUser { get; }
        public ServerForm Server { get; }
        internal CommandContext(Classes.User user, Classes.Message message)
        {
            User = user;
            Message = message;
            Server = Menu.Server;
            ServerUser = Server.SERVERUSER;
        }
    }

    public abstract class CommandBase
    {
        protected CommandContext Context { get; private set; }
        internal CommandBase() { }

        internal CommandBase WithContext(CommandContext c)
        {
            Context = c;
            return this;
        }

        protected virtual void BeforeExecute()
        {
        }

        protected virtual void AfterExecute()
        {
        }

        protected virtual void BroadCast(string message, System.Drawing.Color? color = null)
        {
            var msg = new Classes.Message();
            msg.Colour = color ?? Color.Black;
            msg.Author = Context.ServerUser;
            msg.Content = message;
            msg.Id = Common.IterateMessageId();
            var packet = new Classes.Packet(Classes.PacketId.NewMessage, msg.ToJson());
            Context.Server.Server.Broadcast(packet);
            Context.Server.Server._internalServerMessage(msg);
        }

        protected virtual void Reply(string message, System.Drawing.Color? color = null)
        {
            var msg = new Classes.Message();
            msg.Colour = color ?? Color.DarkGray;
            msg.Author = Context.ServerUser;
            msg.Content = message;
            msg.Id = Common.IterateMessageId();
            if(Context.User.Id == Context.ServerUser.Id)
            {
                Context.Server.Server._internalServerMessage(msg);
            } else
            {
                var packet = new Classes.Packet(Classes.PacketId.NewMessage, msg.ToJson());
                Context.Server.Server.SendTo(Context.User, packet);
            }
        }

        protected virtual void SendTo(Classes.User user, string message, System.Drawing.Color? color = null)
        {
            var msg = new Classes.Message();
            msg.Colour = color ?? Color.Black;
            msg.Author = Context.ServerUser;
            msg.Content = message;
            msg.Id = Common.IterateMessageId();
            if (user.Id == Context.ServerUser.Id)
            {
                Context.Server.Server._internalServerMessage(msg);
            }
            else
            {
                var packet = new Classes.Packet(Classes.PacketId.NewMessage, msg.ToJson());
                Context.Server.Server.SendTo(user, packet);
            }
        }
    }

    public class Command
    {
        public string Name { get; set; }
        public int Priority { get; set; }
        public List<PreconditionAttribute> Preconditions { get; set; }
        public Type CommandClass;
        public MethodInfo CommandMethod;
        public CommandGroup Parent;

        public void Invoke(CommandContext context, object[] args)
        {
            CommandBase cmdBase = (CommandBase)Activator.CreateInstance(CommandClass);
            cmdBase.WithContext(context);
            CommandMethod.Invoke(cmdBase, parameters: args);
        }
    }

    public class CommandGroup
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public Type CommandClass;
        public List<Command> Children;
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public string ErrorReason { get; }
        public Result(bool s, string err)
        {
            IsSuccess = s;
            ErrorReason = err;
        }
    }

    public class CommandValidResult : Result
    {
        public Command Command;
        public ParseParamaterResult ParamResult;
        public CommandValidResult(string err) : base(false, err)
        {
        }
        public CommandValidResult(ParseParamaterResult r) : base(true, null)
        {
            ParamResult = r;
        }
        public CommandValidResult(Command cmd, ParseParamaterResult r) : base(true, null)
        {
            Command = cmd;
            ParamResult = r;
        }
    }

    public class ParseParamaterResult : Result
    {
        public List<object> Output { get; }
        public ParseParamaterResult(List<object> objs) : base(true, "")
        {
            Output = objs;
        }
        public ParseParamaterResult(string err) : base(false, err) { }
    }

    public abstract class PreconditionAttribute : Attribute
    {
        public abstract Result CheckPrecondition(CommandContext context);
    }
}
