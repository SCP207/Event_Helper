using CommandSystem;
using System;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;
using Event_Helper;

namespace Event_Give_Items.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class StopTeslas : ICommand {
        public string Command { get; } = "stopteslas";

        public string[] Aliases { get; } = { "stopt", "steslas", "teslastop", "stopteslastriggering"};

        public string Description { get; } = "Prevent tesla gates from triggering (Toggle)";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission("eh.stopteslas")) {
                response = "You don't have permission to run this command";
                return false;
            }
            if (arguments.Count != 0) {
                response = "You have too many arguments\nUsage: stopteslas";
                return false;
            }

            Plugin.areTeslasTriggering = !Plugin.areTeslasTriggering;

            string teslaStop;
            if (Plugin.areTeslasTriggering) {
                teslaStop = "enabled";
            } else {
                teslaStop = "disabled";
            }


            Log.Debug($"Tesla gates are now {teslaStop}");
            response = $"Done! Teslas are now {teslaStop}";
            return true;
        }
    }
}
