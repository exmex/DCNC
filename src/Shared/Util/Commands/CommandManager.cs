using System;
using System.Collections.Generic;

namespace Shared.Util.Commands
{
    /// <summary>
    ///     Generalized command manager
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TFunc"></typeparam>
    public abstract class CommandManager<TCommand, TFunc>
        where TCommand : Command<TFunc>
        where TFunc : class
    {
        protected Dictionary<string, TCommand> Commands;

        protected CommandManager()
        {
            Commands = new Dictionary<string, TCommand>();
        }

        /// <summary>
        ///     Adds command to list of command handlers.
        /// </summary>
        /// <param name="command"></param>
        protected void Add(TCommand command)
        {
            Commands[command.Name] = command;
        }

        /// <summary>
        ///     Returns command or null, if the command doesn't exist.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TCommand GetCommand(string name)
        {
            TCommand command;
            Commands.TryGetValue(name, out command);
            return command;
        }
    }

    /// <summary>
    ///     Generalized command holder
    /// </summary>
    /// <typeparam name="TFunc"></typeparam>
    public abstract class Command<TFunc> where TFunc : class
    {
        protected Command(string name, string usage, string description, TFunc func)
        {
            if (!typeof(TFunc).IsSubclassOf(typeof(Delegate)))
                throw new InvalidOperationException(typeof(TFunc).Name + " is not a delegate type");

            Name = name;
            Usage = usage;
            Description = description;
            Func = func;
        }

        public string Name { get; protected set; }
        public string Usage { get; protected set; }
        public string Description { get; protected set; }
        public TFunc Func { get; protected set; }
    }

    public enum CommandResult
    {
        Okay,
        Fail,
        InvalidArgument,
        Break
    }
}