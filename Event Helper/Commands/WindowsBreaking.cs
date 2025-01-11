using CommandSystem;
using Event_Helper;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;

namespace Event_Helper.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class BreakWindows : ICommand {
        public string Command { get; } = "windowsbreaking";
        public string[] Aliases { get; } = { "indestructablewindows", "iw" };
        public string Description { get; } = "Disallows windows to break";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission("eh.breakable")) {
                response = "You don't have permission to run this command";
                return false;
            }
            if (arguments.Count != 0) {
                response = "You have too many arguments\nUsage: windowsbreaking";
                return false;
            }

            Plugin.Instance.doWindowsBreak = !Plugin.Instance.doWindowsBreak;

            IEnumerable<Window> windows = Window.List;
            if (!Plugin.Instance.doWindowsBreak) Plugin.Instance.windowHealthList.Clear();
            foreach (Window w in windows) {
                if (!Plugin.Instance.doWindowsBreak) {
                    Plugin.Instance.windowHealthList.Add(w, w.Health);
                    w.Health = float.PositiveInfinity;
                } else {
                    Plugin.Instance.windowHealthList.TryGetValue(w, out var health);
                    w.Health = health;
                }
            }

            Log.Debug($"Windows breaking is set to {Plugin.Instance.doWindowsBreak}");
            response = $"Done! Windows breaking is now {Plugin.Instance.doWindowsBreak}";
            return true;
        }
    }
}
