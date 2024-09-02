using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using InventorySystem.Items.Pickups;
using PlayerRoles;
using System.Threading.Tasks;

namespace Event_Helper.Handlers {
    public class Player {
        public void OnVerified(VerifiedEventArgs ev) {
            // If a player joins, the ID list this plugin keeps track of adds them
            Plugin.playerIdList += $"{ev.Player.Id}.";
            Log.Debug($"Someone joined\nEvent Helpers now has the player list of: {Plugin.playerIdList}");
        }

        public void OnWeaponReload(ReloadingWeaponEventArgs ev) {
            // Checks if players should have infinite ammo
            // Note: You do need to have any amount of ammo in your inventory originally for you to get infinite
            if (Plugin.isInfAmmoEnabled) {
                ev.Player.SetAmmo(ev.Firearm.AmmoType, (ushort)(ev.Firearm.MaxAmmo * 2));
            }
        }

        public void OnTeslaGateActivate(TriggeringTeslaEventArgs ev) {
            // Checks if teslas should be triggered
            if (!Plugin.areTeslasTriggering) {
                ev.IsTriggerable = false;
            } else {
                ev.IsTriggerable = true;
            }
        }
    }
}
