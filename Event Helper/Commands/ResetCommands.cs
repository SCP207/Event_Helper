using CommandSystem;
using Event_Helper;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;

namespace Event_Helper.Event_Helper.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ResetCommands : ICommand {
    public string Command => "ehreset";

    public string[] Aliases => ["reseteh", "ehr"];

    public string Description => "Resets all Event_Helper.Commands back to their defaunt state";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        if (!sender.CheckPermission("eh.resetEvent_Helper.Commands")) {
            response = "You don't have permission to run this command";
            return false;
        }

        Plugin.Instance.ResetCommands();

        response = "Done! Event_Helper.Commands have been reset";
        return true;
    }
}
