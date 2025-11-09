using CommandSystem;
using Event_Helper;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;

namespace Event_Helper.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class GetCommands : ICommand {
    public string Command => "ehhelp";

    public string[] Aliases => ["ehgetcommands", "ehh"];

    public string Description => "Gets a list of all commands in the Event Helpers plugin";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        response = "Commands:";
        foreach (var commandDict in Plugin.Instance.Commands.Values) {
            foreach (var command in commandDict.Values) {
                response += $"\n{command.Command}";
            }
        }

        return true;
    }
}
