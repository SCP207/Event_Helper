using CommandSystem;
using System;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;

namespace Event_Helper.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class InfAmmo : ICommand {
        public string Command { get; } = "infammo";
        public string[] Aliases { get; } = { "ia", "infiniteammo" };
        public string Description { get; } = "Gives a player reloading a weapon ammo, affects everyone (Toggle)";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission("eh.infammo")) {
                response = "You don't have permission to run this command";
                return false;
            }
            if (arguments.Count != 0) {
                response = "You have too many arguments\nUsage: infammo";
                return false;
            }

            Plugin.Instance.isInfAmmoEnabled = !Plugin.Instance.isInfAmmoEnabled;

            Log.Debug($"Infinite Ammo is set to {Plugin.Instance.isInfAmmoEnabled}");
            response = $"Done! Infinite Ammo is now {Plugin.Instance.isInfAmmoEnabled}";
            return true;
        }
    }
}
