using System.Collections.Generic;
using Microsoft.Win32;
using Shared.Network;

namespace Shared.Util.Commands
{
    /// <summary>
    /// </summary>
    public class ChatCommands : CommandManager<ChatCommand, ChatCommandFunc>
    {
        protected ChatCommands()
        {
            Commands = new Dictionary<string, ChatCommand>();
        }

        /// <summary>
        ///     Adds new command handler.
        /// </summary>
        /// <param name="name">The name of the command</param>
        /// <param name="permission">The minimum required permission level</param>
        /// <param name="description">A help description of the command</param>
        /// <param name="handler">The function that gets executed when the command was entered</param>
        public void Add(string name, int permission, string description, ChatCommandFunc handler)
        {
            Add(name, "Unspecified", permission, description, handler);
        }

        /// <summary>
        ///     Adds new command handler.
        /// </summary>
        /// <param name="name">The name of the command</param>
        /// <param name="usage">How to use the command (Syntax)</param>
        /// <param name="permission">The minimum required permission level</param>
        /// <param name="description">A help description of the command</param>
        /// <param name="handler">The function that gets executed when the command was entered</param>
        public void Add(string name, string usage, int permission, string description, ChatCommandFunc handler)
        {
            Commands[name] = new ChatCommand(name, usage, permission, description, handler);
        }
    }

    public class ChatCommand : Command<ChatCommandFunc>
    {
        public readonly int RequiredPermission;
        
        public ChatCommand(string name, string usage, int permission, string description, ChatCommandFunc func)
            : base(name, usage, description, func)
        {
            RequiredPermission = permission;
        }
    }

    public delegate CommandResult ChatCommandFunc(DefaultServer server, Client sender, string command, IList<string> args);
}