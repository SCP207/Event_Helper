using CommandSystem;
using System;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using Exiled.Events.Commands.Reload;

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
                IEnumerable<Player> players = Player.Dictionary.Values;
                foreach (Player p in players) {
                    p.AddAmmo(AmmoType.Nato9, 1);
                    p.AddAmmo(AmmoType.Nato556, 1);
                    p.AddAmmo(AmmoType.Nato762, 1);
                    p.AddAmmo(AmmoType.Ammo12Gauge, 1);
                    p.AddAmmo(AmmoType.Ammo44Cal, 1);
                }
            }

            Log.Debug($"InfAmmo is set to {Plugin.isInfAmmoEnabled}");
            response = $"Done! InfAmmo is set to {Plugin.isInfAmmoEnabled}";
            return true;
        }
    }
}
