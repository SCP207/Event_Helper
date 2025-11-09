using CommandSystem;
using Event_Helper;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Event_Helper.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class GetCommands : ICommand {
    public string Command => "ehhelp";

    public string[] Aliases => ["ehgetcommands", "ehh"];

    public string Description => "Gets a list of all commands in the Event Helpers plugin";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        response = "Commands:\n\n";
        foreach (var commandDict in Plugin.Instance.Commands.Values)
            response += string.Join("\n\n", commandDict.Values
                .Select(c => $"{c.Command}{(c.Aliases.Length > 0 ? $"\nAliases: ({string.Join(", ", c.Aliases)})" : "")}\nDescription: {c.Description}"));

        return true;
    }
}
