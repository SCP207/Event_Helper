using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;

using PlayerHandlers = Exiled.Events.Handlers.Player;
using ItemHandlers = Exiled.Events.Handlers.Item;

using ExiledPlayer = Exiled.API.Features.Player;
using Exiled.API.Features.DamageHandlers;

namespace Event_Helper.Handlers {
    public static class Player {
        public static void RegisterEvents() {
            ItemHandlers.ChargingJailbird += OnJailbirdUse;

            PlayerHandlers.Verified += OnPlayerVerified;
            PlayerHandlers.UsingMicroHIDEnergy += OnMicroEnergyDrain;
            PlayerHandlers.Shot += OnWeaponFire;
            PlayerHandlers.DryfiringWeapon += OnWeaponDryFire;
            PlayerHandlers.ReloadingWeapon += OnWeaponReload;
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

            PlayerHandlers.Verified -= OnPlayerVerified;
            PlayerHandlers.UsingMicroHIDEnergy -= OnMicroEnergyDrain;
            PlayerHandlers.Shot -= OnWeaponFire;
            PlayerHandlers.DryfiringWeapon -= OnWeaponDryFire;
            PlayerHandlers.ReloadingWeapon -= OnWeaponReload;
            PlayerHandlers.DroppingAmmo -= OnAmmoDrop;
            PlayerHandlers.Spawned -= OnSpawn;
            PlayerHandlers.TriggeringTesla -= OnTeslaGateActivate;
            PlayerHandlers.DamagingDoor -= OnDoorDamage;
            PlayerHandlers.InteractingDoor -= OnDoorInteract;
            PlayerHandlers.Dying -= OnPlayerDeath;
            PlayerHandlers.Handcuffing -= OnPlayerDetained;
            PlayerHandlers.PickingUpItem -= OnPickUpItem;
        }

        private static void OnPlayerVerified(VerifiedEventArgs ev) {
            var unablePickupList = Plugin.Instance.ItemUnableToPickUp.Where(i => i.Value.affectsEveryone == true);
            foreach (var i in unablePickupList) {
                i.Value.affectedPlayers.Add(ev.Player);
            }
        }

        private static void OnWeaponFire(ShotEventArgs ev) {
            // Checks if players should have infinite ammo without reloading //
            if (Plugin.Instance.IsInfInGunAmmoEnabled) {
                ev.Firearm.PrimaryMagazine.Ammo = ev.Firearm.PrimaryMagazine.MaxAmmo;
                return;
            } else if (Plugin.Instance.IsInfAmmoEnabled && ev.Item.Type == ItemType.ParticleDisruptor) {
                ev.Firearm.PrimaryMagazine.Ammo = ev.Firearm.PrimaryMagazine.MaxAmmo;
            }

            // Checks if players should have infinite ammo //
            if (Plugin.Instance.IsInfAmmoEnabled)
                ev.Player.SetAmmo(ev.Firearm.AmmoType, 1);
        }
        private static void OnWeaponDryFire(DryfiringWeaponEventArgs ev) {
            // Checks if players should have infinite ammo without reloading //
            if (Plugin.Instance.IsInfInGunAmmoEnabled) {
                ev.Firearm.PrimaryMagazine.Ammo = ev.Firearm.PrimaryMagazine.MaxAmmo;
                return;
            } else if (Plugin.Instance.IsInfAmmoEnabled && ev.Item.Type == ItemType.ParticleDisruptor) {
                ev.Firearm.PrimaryMagazine.Ammo = ev.Firearm.PrimaryMagazine.MaxAmmo;
            }

            // Checks if players should have infinite ammo //
            if (Plugin.Instance.IsInfAmmoEnabled)
                ev.Player.SetAmmo(ev.Firearm.AmmoType, 1);
        }
        private static void OnJailbirdUse(ChargingJailbirdEventArgs ev) {
            if (Plugin.Instance.IsInfAmmoEnabled || Plugin.Instance.IsInfInGunAmmoEnabled) {
                ev.Jailbird.TotalCharges = 0;
            }
        }
        private static void OnMicroEnergyDrain(UsingMicroHIDEnergyEventArgs ev) {
            if (Plugin.Instance.Config.infiniteMicro && (Plugin.Instance.IsInfAmmoEnabled || Plugin.Instance.IsInfInGunAmmoEnabled)) {
                ev.MicroHID.Energy = 100;
                ev.Drain = 0;
            }
        }
        private static void OnWeaponReload(ReloadingWeaponEventArgs ev) {
            if (Plugin.Instance.IsInfAmmoEnabled) {
                // Revolvers set ammo to 1 less, so that's why it's 2 here //
                int addition = (ev.Firearm.Type == ItemType.GunRevolver) ? 2 : 1;
                ev.Player.SetAmmo(ev.Firearm.AmmoType, (ushort)(ev.Firearm.MaxMagazineAmmo - ev.Firearm.MagazineAmmo + addition));
            }
        }

        private static void OnAmmoDrop(DroppingAmmoEventArgs ev) {
            // Disallows players from dropping ammo if infinite ammo is enabled //
            if (Plugin.Instance.IsInfAmmoEnabled) {
                ev.IsAllowed = false;
                return;
            }
        }

        private static void OnSpawn(SpawnedEventArgs ev) {
            // Clears the inventory of the player if they shouldn't spawn with items //
            if (!Plugin.Instance.DoPlayersSpawnWithItems) {
                if (Plugin.Instance.AffectOnlyClassD) {
                    if (ev.Player.Role == RoleTypeId.ClassD) {
                        ev.Player.ClearInventory();
                    }
                } else {
                    ev.Player.ClearInventory();
                }
            }

            // Checks if an item should be given, then gives and force equips the item //
            if (Plugin.Instance.AreItemsBeingGivenOnWave && Plugin.Instance.ItemsOnlyOnWaves) {
                Log.Debug("Items are being given out on waves from the command \"giveitemonspawn\"");
                Log.Debug($"The item being given is {Plugin.Instance.ItemsBeingGiven}");

                IEnumerable<ExiledPlayer> players = ExiledPlayer.Dictionary.Values;
                foreach (ExiledPlayer p in players) {
                    Item i = p.AddItem(Plugin.Instance.ItemsBeingGiven);
                    p.CurrentItem = i;
                }
            }

            // Checks if an effect should be gien, gives it, then adds the amount you wanted to add //
            if (Plugin.Instance.AreEffectsBeingGivenOnSpawn && !Plugin.Instance.EffectsOnlyOnWaves) {
                Log.Debug("Effects are being given out on waves from the command \"giveitemonspawn\"");

                // Gives the requested effect
                IEnumerable<ExiledPlayer> players = ExiledPlayer.Dictionary.Values;
                foreach (ExiledPlayer p in players) {
                    foreach (string effectName in Plugin.Instance.EffectNames) {
                        p.EnableEffect(effectName, Plugin.Instance.EffectIntensity, Plugin.Instance.EffectDuration);
                    }
                }
            }
        }

        private static void OnTeslaGateActivate(TriggeringTeslaEventArgs ev) {
            // Checks if teslas should be triggered //
            if (!Plugin.Instance.AreTeslasTriggering) {
                ev.IsTriggerable = false;
            }
        }

        private static void OnDoorDamage(DamagingDoorEventArgs ev) {
            // Checks if doors should break //
            if (!Plugin.Instance.DoDoorsBreak) {
                ev.IsAllowed = false;
            }
        }

        private static void OnDoorInteract(InteractingDoorEventArgs ev) {
            // Checks if the player should be locking the door //
            if (Plugin.Instance.PlayersThatLockDoors.Contains(ev.Player)) {
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

        private static void OnPlayerDeath(DyingEventArgs ev) {
            if (Plugin.Instance.Config.TeslaVaporize && ev.DamageHandler.Type == DamageType.Tesla)
                ev.Player.Vaporize();
        }

        private static void OnPlayerDetained(HandcuffingEventArgs ev) {
            if ((!Plugin.Instance.Config.GodModePlayersGetDetained && ev.Target.IsGodModeEnabled) ||
                (!Plugin.Instance.Config.BypassPlayersGetDetained && ev.Target.IsBypassModeEnabled)) {
                ev.IsAllowed = false;
            }
        }

        private static void OnPickUpItem(PickingUpItemEventArgs ev) {
            if (Plugin.Instance.ItemUnableToPickUp.TryGetValue(ev.Pickup.Type, out var playerList)) {
                if (playerList.affectedPlayers.Contains(ev.Player))
                    ev.IsAllowed = false;
            }
        }
    }
}
