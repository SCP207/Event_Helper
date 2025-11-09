using CommandSystem;
using System;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace Event_Helper.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal class StopTeslas : ICommand {
    public string Command => "stopteslas";
    public string[] Aliases => [ "stopt", "steslas", "teslastop", "stopteslastriggering" ];
    public string Description => "Prevent tesla gates from triggering (Toggle)";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        if (!sender.CheckPermission("eh.stopteslas")) {
            response = "You don't have permission to run this command";
            return false;
        }
        if (arguments.Count != 0) {
            response = "You have too many arguments\nUsage: stopteslas";
            return false;
        }

        Plugin.Instance.AreTeslasTriggering = !Plugin.Instance.AreTeslasTriggering;

        string teslaStop = (Plugin.Instance.AreTeslasTriggering) ? "enabled" : "disabled";

        Log.Debug($"Tesla gates are now {teslaStop}");
        response = $"Done! Teslas are now {teslaStop}";
        return true;
    }
}
