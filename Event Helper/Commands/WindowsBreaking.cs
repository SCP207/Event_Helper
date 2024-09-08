using CommandSystem;
using Event_Helper;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace Event_Give_Items.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class BreakWindows : ICommand {
        public string Command { get; } = "windowsbreaking";
        public string[] Aliases { get; } = { "wb", "indestructablewindows", "iw" };
        public string Description { get; } = "Disallows windows to break";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission("eh.windowsbreaking")) {
                response = "You don't have permission to run this command";
                return false;
            }
            if (arguments.Count != 0) {
                response = "You have too many arguments\nUsage: windowsbreaking";
                return false;
            }

            Plugin.doWindowsBreak = !Plugin.doWindowsBreak;

            Log.Debug($"Windows breaking is set to {Plugin.doWindowsBreak}");
            response = $"Done! Windows breaking is now {Plugin.doWindowsBreak}";
            return true;
        }
    }
}
