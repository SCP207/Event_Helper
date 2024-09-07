using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace Event_Helper.Handlers {
    public class Player {
        public void OnVerified(VerifiedEventArgs ev) {
            // If a player joins, the ID list this plugin keeps track of adds them
            Plugin.playerIdList += $"{ev.Player.Id}.";
            Log.Debug($"Someone joined\nEvent Helpers now has the player list of: {Plugin.playerIdList}");
        }

        public void OnWeaponFire(ShotEventArgs ev) {
            // Checks if players should have infinite ammo without reloading
            if (Plugin.isInfInGunAmmoEnabled) {
                ev.Firearm.Ammo = ev.Firearm.MaxAmmo;
            }
        }

        public void OnWeaponReload(ReloadingWeaponEventArgs ev) {
            // Checks if players should have infinite ammo
            if (Plugin.isInfAmmoEnabled) {
                ev.Player.SetAmmo(ev.Firearm.AmmoType, (ushort)(ev.Firearm.MaxAmmo + 1));
            }
        }
        public void OnAmmoDrop(DroppingAmmoEventArgs ev) {
            // Disallows players from dropping ammo if infinite ammo is enabled
            if (Plugin.isInfAmmoEnabled) {
                ev.IsAllowed = false;
                return;
            }
            ev.IsAllowed = true;
        }
        public void OnSpawn(SpawnedEventArgs ev) {
            // Adds ammo whenever someone spawns if infinite ammo is enabled
            if (Plugin.isInfAmmoEnabled) {
                ev.Player.AddAmmo(AmmoType.Nato9, 1);
                ev.Player.AddAmmo(AmmoType.Nato556, 1);
                ev.Player.AddAmmo(AmmoType.Nato762, 1);
                ev.Player.AddAmmo(AmmoType.Ammo12Gauge, 1);
                ev.Player.AddAmmo(AmmoType.Ammo44Cal, 1);
            }
        }

        public void OnTeslaGateActivate(TriggeringTeslaEventArgs ev) {
            // Checks if teslas should be triggered
            if (!Plugin.areTeslasTriggering) {
                ev.IsTriggerable = false;
                return;
            }
            ev.IsTriggerable = true;
        }
    }
}
