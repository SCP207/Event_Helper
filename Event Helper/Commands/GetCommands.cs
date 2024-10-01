using CommandSystem;
using Event_Helper;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;

namespace Event_Helper.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class GetCommands : ICommand {
        public string Command { get; } = "ehhelp";

        public string[] Aliases { get; } = { "ehgetcommands", "ehh" };

        public string Description { get; } = "Gets a list of all commands in the Event Helpers plugin";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            string message = string.Empty;
            foreach (string i in Plugin.commandList) {
                message += $"\n{i}";
            }
            response = $"Commands:{message}";
            return true;
        }
    }
}
