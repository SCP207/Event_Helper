using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using Exiled.API.Features;
using Exiled.API.Features.Items;

using PlayerHandlers = Exiled.Events.Handlers.Player;
using ItemHandlers = Exiled.Events.Handlers.Item;

using EPlayer = Exiled.API.Features.Player;

namespace Event_Helper.Handlers {
    public static class Player {
        public static void RegisterEvents() {
            ItemHandlers.ChargingJailbird += OnJailbirdUse;

            PlayerHandlers.UsingMicroHIDEnergy += OnMicroEnergyDrain;
            PlayerHandlers.Shot += OnWeaponFire;
            PlayerHandlers.DryfiringWeapon += OnWeaponDryFire;
            PlayerHandlers.DroppingAmmo += OnAmmoDrop;
            PlayerHandlers.Spawned += OnSpawn;
            PlayerHandlers.TriggeringTesla += OnTeslaGateActivate;
            PlayerHandlers.DamagingDoor += OnDoorDamage;
            PlayerHandlers.InteractingDoor += OnDoorInteract;
            PlayerHandlers.Dying += OnPlayerDeath;
            PlayerHandlers.Handcuffing += OnPlayerDetained;
            PlayerHandlers.PickingUpItem += OnPickUpItem;
        }

        public static void UnregisterEvents() {
            ItemHandlers.ChargingJailbird -= OnJailbirdUse;

            PlayerHandlers.UsingMicroHIDEnergy -= OnMicroEnergyDrain;
            PlayerHandlers.Shot -= OnWeaponFire;
            PlayerHandlers.DryfiringWeapon -= OnWeaponDryFire;
            PlayerHandlers.DroppingAmmo -= OnAmmoDrop;
            PlayerHandlers.Spawned -= OnSpawn;
            PlayerHandlers.TriggeringTesla -= OnTeslaGateActivate;
            PlayerHandlers.DamagingDoor -= OnDoorDamage;
            PlayerHandlers.InteractingDoor -= OnDoorInteract;
            PlayerHandlers.Dying -= OnPlayerDeath;
            PlayerHandlers.Handcuffing -= OnPlayerDetained;
            PlayerHandlers.PickingUpItem -= OnPickUpItem;
        }

        public static void OnWeaponFire(ShotEventArgs ev) {
            // Checks if players should have infinite ammo without reloading //
            if (Plugin.Instance.isInfInGunAmmoEnabled) {
                ev.Firearm.MagazineAmmo = ev.Firearm.MaxMagazineAmmo;
            } else if ((Plugin.Instance.isInfAmmoEnabled || Plugin.Instance.isInfInGunAmmoEnabled) && ev.Item.Type == ItemType.ParticleDisruptor) {
                ev.Firearm.PrimaryMagazine.Ammo = ev.Firearm.MaxMagazineAmmo;
            }
            
            // Checks if players should have infinite ammo //
            if (Plugin.Instance.isInfAmmoEnabled) {
                ev.Player.SetAmmo(ev.Firearm.AmmoType, (ushort)(ev.Firearm.MaxMagazineAmmo - ev.Firearm.MagazineAmmo));
            }
        }
        public static void OnWeaponDryFire(DryfiringWeaponEventArgs ev) {
            // Checks if players should have infinite ammo without reloading //
            if (Plugin.Instance.isInfInGunAmmoEnabled) {
                ev.Firearm.MagazineAmmo = ev.Firearm.MaxMagazineAmmo;
            } else if ((Plugin.Instance.isInfAmmoEnabled || Plugin.Instance.isInfInGunAmmoEnabled) && ev.Item.Type == ItemType.ParticleDisruptor) {
                ev.Firearm.PrimaryMagazine.Ammo = ev.Firearm.MaxMagazineAmmo;
            }

            // Checks if players should have infinite ammo //
            if (Plugin.Instance.isInfAmmoEnabled) {
                ev.Player.SetAmmo(ev.Firearm.AmmoType, (ushort)(ev.Firearm.MaxMagazineAmmo - ev.Firearm.MagazineAmmo));
            }
        }
        public static void OnJailbirdUse(ChargingJailbirdEventArgs ev) {
            if (Plugin.Instance.isInfAmmoEnabled || Plugin.Instance.isInfInGunAmmoEnabled) {
                ev.Jailbird.TotalCharges = 0;
            }
        }
        public static void OnMicroEnergyDrain(UsingMicroHIDEnergyEventArgs ev) {
            if (Plugin.Instance.isInfAmmoEnabled || Plugin.Instance.isInfInGunAmmoEnabled) {
                ev.Drain = 0;
                ev.MicroHID.Energy = 100;
            }
        }

        public static void OnAmmoDrop(DroppingAmmoEventArgs ev) {
            // Disallows players from dropping ammo if infinite ammo is enabled //
            if (Plugin.Instance.isInfAmmoEnabled) {
                ev.IsAllowed = false;
                return;
            }
        }

        public static void OnSpawn(SpawnedEventArgs ev) {
            // Clears the inventory of the player if they shouldn't spawn with items //
            if (!Plugin.Instance.doPlayersSpawnWithItems) {
                if (Plugin.Instance.affectsOnlyClassD) {
                    if (ev.Player.Role == RoleTypeId.ClassD) {
                        ev.Player.ClearInventory();
                    }
                } else {
                    ev.Player.ClearInventory();
                }
            }

            // Checks if an item should be given, then gives and force equips the item //
            if (Plugin.Instance.areItemsBeingGivenOnWave && Plugin.Instance.itemsOnlyOnWaves) {
                Log.Debug("Items are being given out on waves from the command \"giveitemonspawn\"");
                Log.Debug($"The item being given is {Plugin.Instance.itemsBeingGiven}");

                IEnumerable<EPlayer> players = EPlayer.Dictionary.Values;
                foreach (EPlayer p in players) {
                    Item i = p.AddItem(Plugin.Instance.itemsBeingGiven);
                    p.CurrentItem = i;
                }
            }

            // Checks if an effect should be gien, gives it, then adds the amount you wanted to add //
            if (Plugin.Instance.areEffectsBeingGivenOnSpawn && !Plugin.Instance.effectsOnlyOnWaves) {
                Log.Debug("Effects are being given out on waves from the command \"giveitemonspawn\"");

                // Gives the requested effect
                IEnumerable<EPlayer> players = EPlayer.Dictionary.Values;
                foreach (EPlayer p in players) {
                    foreach (string effectName in Plugin.Instance.effectNames) {
                        p.EnableEffect(effectName, Plugin.Instance.effectIntensity, Plugin.Instance.effectDuration);
                    }
                }
            }
        }

        public static void OnTeslaGateActivate(TriggeringTeslaEventArgs ev) {
            // Checks if teslas should be triggered //
            if (!Plugin.Instance.areTeslasTriggering) {
                ev.IsTriggerable = false;
            }
        }

        public static void OnDoorDamage(DamagingDoorEventArgs ev) {
            // Checks if doors should break //
            if (!Plugin.Instance.doDoorsBreak) {
                ev.IsAllowed = false;
            }
        }

        public static void OnDoorInteract(InteractingDoorEventArgs ev) {
            // Checks if the player should be locking the door //
            if (Plugin.Instance.lockDoors.Contains(ev.Player)) {
                if (!ev.Door.IsLocked) {
                    if (!ev.Player.IsBypassModeEnabled) {
                        // Sets door to open so that it ends in the closed state after the door is interacted with //
                        ev.Door.IsOpen = true;
                    }
                    ev.Door.Lock(float.PositiveInfinity, DoorLockType.AdminCommand);
                } else {
                    ev.Door.Unlock();
                }
            }
        }

        public static void OnPlayerDeath(DyingEventArgs ev) {
            if (Plugin.Instance.Config.TeslaVaporize && ev.DamageHandler.Type == DamageType.Tesla) {
                ev.Player.Vaporize();
            }
        }

        public static void OnPlayerDetained(HandcuffingEventArgs ev) {
            if ((!Plugin.Instance.Config.GodModePlayersGetDetained && ev.Target.IsGodModeEnabled) ||
                (!Plugin.Instance.Config.BypassPlayersGetDetained && ev.Target.IsBypassModeEnabled)) {
                ev.IsAllowed = false;
            }
        }

        public static void OnPickUpItem(PickingUpItemEventArgs ev) {
            if (Plugin.Instance.itemUnableToPickUp.TryGetValue(ev.Pickup.Type, out var playerList)) {
                if (playerList.Contains(ev.Player))
                    ev.IsAllowed = false;
            }
        }
    }
}
