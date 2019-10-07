using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Commands.Builders
{
    public class ParamaterBuilder
    {
        private readonly List<Attribute> _attributes;

        public CommandBuilder Command { get; }
        public string Name { get; internal set; }
        public Type ParamaterType { get; internal set; }
        public TypeReader TypeReader { get; set; }

        public bool IsOptional { get; set; }
        public bool IsRemainder { get; set; }
        public bool IsMultiple { get; set; }
        public object DefaultValue { get; set; }
        public string Summary { get; set; }

        internal ParamaterBuilder(CommandBuilder command)
        {
            _attributes = new List<Attribute>();
            Command = command;
        }

        private TypeReader GetReader(Type type)
        {
            var commands = Command.Module.Service;
            if (type.GetTypeInfo().GetCustomAttribute<NamedArgumentTypeAttribute>() != null)
            {
                IsRemainder = true;
                var reader = commands.GetTypeReaders(type)?.FirstOrDefault().Value;
                if (reader == null)
                {
                    Type readerType;
                    try
                    {
                        readerType = typeof(NamedArgumentTypeReader<>).MakeGenericType(new[] { type });
                    }
                    catch (ArgumentException ex)
                    {
                        throw new InvalidOperationException($"Parameter type '{type.Name}' for command '{Command.Name}' must be a class with a public parameterless constructor to use as a NamedArgumentType.", ex);
                    }

                    reader = (TypeReader)Activator.CreateInstance(readerType, new[] { commands });
                    commands.AddTypeReader(type, reader);
                }

                return reader;
            }


            var readers = commands.GetTypeReaders(type);
            if (readers != null)
                return readers.FirstOrDefault().Value;
            else
                return commands.GetDefaultTypeReader(type);
        }

        internal ParamaterInfo Build(CommandInfo info)
        {
            if ((TypeReader ?? (TypeReader = GetReader(ParamaterType))) == null)
                throw new InvalidOperationException($"No type reader found for type {ParamaterType.Name}, one must be specified");

            return new ParamaterInfo(this, info, Command.Module.Service);
        }
    }
}
