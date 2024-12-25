using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;

namespace Event_Helper.Handlers {
    public class Player {
        private Plugin plugin = new();

        public Player(Plugin main) {
            plugin = main;
        }

        public void OnWeaponFire(ShotEventArgs ev) {
            // Checks if players should have infinite ammo without reloading
            if (Plugin.isInfInGunAmmoEnabled) {
                ev.Firearm.MagazineAmmo = ev.Firearm.MaxMagazineAmmo;
            } else if ((Plugin.isInfAmmoEnabled || Plugin.isInfInGunAmmoEnabled) && ev.Item.Type == ItemType.ParticleDisruptor) {
                ev.Firearm.PrimaryMagazine.Ammo = ev.Firearm.MaxMagazineAmmo;
            }
            
            // Checks if players should have infinite ammo
            if (Plugin.isInfAmmoEnabled) {
                ev.Player.SetAmmo(ev.Firearm.AmmoType, (ushort)(ev.Firearm.MaxMagazineAmmo - ev.Firearm.MagazineAmmo));
            }
        }
        public void OnWeaponDryFire(DryfiringWeaponEventArgs ev) {
            // Checks if players should have infinite ammo without reloading
            if (Plugin.isInfInGunAmmoEnabled) {
                ev.Firearm.MagazineAmmo = ev.Firearm.MaxMagazineAmmo;
            } else if ((Plugin.isInfAmmoEnabled || Plugin.isInfInGunAmmoEnabled) && ev.Item.Type == ItemType.ParticleDisruptor) {
                ev.Firearm.PrimaryMagazine.Ammo = ev.Firearm.MaxMagazineAmmo;
            }

            // Checks if players should have infinite ammo
            if (Plugin.isInfAmmoEnabled) {
                ev.Player.SetAmmo(ev.Firearm.AmmoType, (ushort)(ev.Firearm.MaxMagazineAmmo - ev.Firearm.MagazineAmmo));
            }
        }
        public void OnJailbirdUse(ChargingJailbirdEventArgs ev) {
            if (Plugin.isInfAmmoEnabled || Plugin.isInfInGunAmmoEnabled) {
                ev.Jailbird.TotalCharges = 0;
            }
        }
        public void OnMicroEnergyDrain(UsingMicroHIDEnergyEventArgs ev) {
            if (Plugin.isInfAmmoEnabled || Plugin.isInfInGunAmmoEnabled) {
                ev.Drain = 0;
                ev.MicroHID.Energy = 100;
            }
        }

        public void OnAmmoDrop(DroppingAmmoEventArgs ev) {
            // Disallows players from dropping ammo if infinite ammo is enabled
            if (Plugin.isInfAmmoEnabled) {
                ev.IsAllowed = false;
                return;
            }
        }

        public void OnSpawn(SpawnedEventArgs ev) {
            // Clears the inventory of the player if they shouldn't spawn with items
            if (!Plugin.doPlayersSpawnWithItems) {
                if (Plugin.affectsOnlyClassD) {
                    if (ev.Player.Role == RoleTypeId.ClassD) {
                        ev.Player.ClearInventory();
                    }
                } else {
                    ev.Player.ClearInventory();
                }
            }
        }

        public void OnTeslaGateActivate(TriggeringTeslaEventArgs ev) {
            // Checks if teslas should be triggered
            if (!Plugin.areTeslasTriggering) {
                ev.IsTriggerable = false;
            }
        }

        public void OnDoorDamage(DamagingDoorEventArgs ev) {
            // Checks if doors should break
            if (!Plugin.doDoorsBreak) {
                ev.IsAllowed = false;
            }
        }
        public void OnWindowDamage(DamagingWindowEventArgs ev) {
            // Checks if windows should break
            if (!Plugin.doWindowsBreak) {
                ev.IsAllowed = false;
            }
        }

        public void OnDoorInteract(InteractingDoorEventArgs ev) {
            // Checks if the player should be locking the door
            if (Plugin.lockDoors.Contains(ev.Player)) {
                if (!ev.Door.IsLocked) {
                    if (!ev.Player.IsBypassModeEnabled) {
                        // Sets door to open so that it ends in the closed state after the door is interacted with
                        ev.Door.IsOpen = true;
                    }
                    ev.Door.Lock(float.PositiveInfinity, DoorLockType.AdminCommand);
                } else {
                    ev.Door.Unlock();
                }
            }
        }

        public void OnPlayerDeath(DyingEventArgs ev) {
            if (plugin.Config.TeslaVaporize && ev.DamageHandler.Type == DamageType.Tesla) {
                ev.Player.Vaporize();
            }
        }

        public void OnPlayerDetained(HandcuffingEventArgs ev) {
            if ((!plugin.Config.GodModePlayersGetDetained && ev.Target.IsGodModeEnabled) ||
                (!plugin.Config.BypassPlayersGetDetained && ev.Target.IsBypassModeEnabled)) {
                ev.IsAllowed = false;
            }
        }

        public void OnPickUpItem(PickingUpItemEventArgs ev) {
            if (Plugin.itemUnableToPickUp.TryGetValue(ev.Pickup.Type, out var playerList)) {
                if (playerList.Contains(ev.Player))
                    ev.IsAllowed = false;
            }
        }
    }
}
