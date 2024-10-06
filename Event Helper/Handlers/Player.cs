using Exiled.API.Enums;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;

namespace Event_Helper.Handlers {
    public class Player {
        private Plugin plugin = new Plugin();

        public Player(Plugin main) {
            plugin = main;
        }

        public void OnWeaponFire(ShotEventArgs ev) {
            // Checks if players should have infinite ammo without reloading
            if (Plugin.isInfInGunAmmoEnabled) {
                ev.Firearm.Ammo = ev.Firearm.MaxAmmo;
            } else if (Plugin.isInfAmmoEnabled && ev.Item.Type == ItemType.ParticleDisruptor) {
                ev.Firearm.Ammo++;
            }
        }
        public void OnJailbirdUse(ChargingJailbirdEventArgs ev) {
            if (Plugin.isInfInGunAmmoEnabled) {
                ev.Jailbird.TotalCharges--;
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
                        ev.Door.IsOpen = true;
                    }
                    ev.Door.Lock(99999, DoorLockType.AdminCommand);
                } else {
                    ev.Door.Unlock();
                }
            }
        }

        public void OnPlayerDeath(DyingEventArgs ev) {
            if (plugin.Config.TeslaVaporize) {
                if (ev.DamageHandler.Type == DamageType.Tesla) {
                    ev.Player.Vaporize();
                }
            }
        }

        public void OnPlayerDetained(HandcuffingEventArgs ev) {
            if (!plugin.Config.GodModePlayersGetDetained) {
                if (ev.Target.IsGodModeEnabled) {
                    ev.IsAllowed = false;
                }
            }
            if (!plugin.Config.BypassPlayersGetDetained) {
                if (ev.Target.IsBypassModeEnabled) {
                    ev.IsAllowed = false;
                }
            }
        }

        public void OnPickUpItem(PickingUpItemEventArgs ev) {
            foreach (Exiled.API.Features.Player p in Plugin.itemUnableToPickUp.Keys) {
                if (ev.Player == p) {
                    Plugin.itemUnableToPickUp.TryGetValue(p, out var items);
                    foreach (ItemType i in items) {
                        if (ev.Pickup.Type == i) {
                            ev.IsAllowed = false;
                        }
                    }
                }
            }
        }
    }
}
