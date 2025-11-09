using CommandSystem;
using Event_Helper;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace Event_Helper.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class BreakDoors : ICommand {
    public string Command => "doorsbreaking";
    public string[] Aliases => ["db", "indestructabledoors", "id"];
    public string Description => "Disallows doors to break";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        if (!sender.CheckPermission("eh.breakable")) {
            response = "You don't have permission to run this command";
            return false;
        }
        if (arguments.Count != 0) {
            response = "You have too many arguments\nUsage: doorsbreaking";
            return false;
        }

        Plugin.Instance.DoDoorsBreak = !Plugin.Instance.DoDoorsBreak;

        Log.Debug($"Doors breaking is set to {Plugin.Instance.DoDoorsBreak}");
        response = $"Done! Doors breaking is now {Plugin.Instance.DoDoorsBreak}";
        return true;
    }
}
