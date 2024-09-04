using CommandSystem;
using System;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace Event_Helper.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class InfAmmo : ICommand {
        public string Command { get; } = "infammo";
        public string[] Aliases { get; } = { "ia", "infiniteammo" };
        public string Description { get; } = "Gives every player infinite ammo (Toggle)";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission("eh.infammo")) {
                response = "You don't have permission to run this command";
                return false;
            }
            if (arguments.Count != 0) {
                response = "You have too many arguments\nUsage: infammo";
                return false;
            }

            Plugin.isInfAmmoEnabled = !Plugin.isInfAmmoEnabled;

            if (Plugin.isInfAmmoEnabled) {
                ServerConsole.EnterCommand($"/give {Plugin.playerIdList} 19");
                ServerConsole.EnterCommand($"/give {Plugin.playerIdList} 22");
                ServerConsole.EnterCommand($"/give {Plugin.playerIdList} 27");
                ServerConsole.EnterCommand($"/give {Plugin.playerIdList} 28");
                ServerConsole.EnterCommand($"/give {Plugin.playerIdList} 29");
            }

            Log.Debug($"Infinite Ammo is set to {Plugin.isInfAmmoEnabled}");
            response = $"Done! Infinite Ammo is now {Plugin.isInfAmmoEnabled}";
            return true;
        }
    }
}
