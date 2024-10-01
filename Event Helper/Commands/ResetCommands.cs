using CommandSystem;
using Event_Helper;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;

namespace Event_Give_Items.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ResetCommands : ICommand {
        public string Command { get; } = "ehreset";

        public string[] Aliases { get; } = { "reseteh", "ehr" };

        public string Description { get; } = "Resets all commands back to their defaunt state";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission("eh.resetcommands")) {
                response = "You don't have permission to run this command";
                return false;
            }

            Plugin.ResetCommands();
            response = "Done! Commands have been reset";
            return true;
        }
    }
}
