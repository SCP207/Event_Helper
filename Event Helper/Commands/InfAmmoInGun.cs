using CommandSystem;
using System;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;
using Event_Helper;

namespace Event_Helper.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class InfAmmoInGun : ICommand {
    public string Command => "infammoingun";
    public string[] Aliases => ["iag", "guninfiniteammo"];
    public string Description => "Gives every player infinite ammo so they don't have to reload (Toggle)";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        if (!sender.CheckPermission("eh.infammo")) {
            response = "You don't have permission to run this command";
            return false;
        }
        if (arguments.Count != 0) {
            response = "You have too many arguments\nUsage: infammoingun";
            return false;
        }

        Plugin.Instance.IsInfInGunAmmoEnabled = !Plugin.Instance.IsInfInGunAmmoEnabled;

        Log.Debug($"Infinite Ammo in Guns is set to {Plugin.Instance.IsInfInGunAmmoEnabled}");
        response = $"Done! Infinite Ammo in Guns is now {Plugin.Instance.IsInfInGunAmmoEnabled}";
        return true;
    }
}
