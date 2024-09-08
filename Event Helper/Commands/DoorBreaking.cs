using CommandSystem;
using Event_Helper;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace Event_Give_Items.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class BreakDoors : ICommand {
        public string Command { get; } = "doorsbreaking";
        public string[] Aliases { get; } = { "db", "indestructabledoors", "id" };
        public string Description { get; } = "Disallows doors to break";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission("eh.doorbreaking")) {
                response = "You don't have permission to run this command";
                return false;
            }
            if (arguments.Count != 0) {
                response = "You have too many arguments\nUsage: doorsbreaking";
                return false;
            }

            Plugin.doDoorsBreak = !Plugin.doDoorsBreak;

            Log.Debug($"Doors breaking is set to {Plugin.doDoorsBreak}");
            response = $"Done! Doors breaking is now {Plugin.doDoorsBreak}";
            return true;
        }
    }
}
