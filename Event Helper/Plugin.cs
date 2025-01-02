using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerHandlers = Exiled.Events.Handlers.Player;
using ItemHandlers = Exiled.Events.Handlers.Item;
using ServerHandlers = Exiled.Events.Handlers.Server;

namespace Event_Helper {
    public class Plugin : Plugin<Config> {
        public override string Author { get; } = "SCP-207";
        public override string Name { get; } = "Event Helper";
        public override string Prefix { get; } = "EH";
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public override Version RequiredExiledVersion { get; } = new(9, 2, 1);
        public override Version Version { get; } = new(3, 4, 1);

        public static List<string> commandList { get; private set; } = new();

        public static bool isInfAmmoEnabled = false;

        public static bool isInfInGunAmmoEnabled = false;

        public static bool areSpawnWavesEnabled = true;

        public static bool areItemsBeingGivenOnWave = false;
        public static ItemType itemsBeingGiven;

        public static bool areEffectsBeingGivenOnSpawn = false;
        public static List<string> effectNames { get; } = new();
        public static int effectDuration;
        public static byte effectIntensity;
        public static Dictionary<string, byte> effectIntensityAdditionOverTime = new();

        public static bool areTeslasTriggering = true;

        public static bool doPlayersSpawnWithItems = true;
        public static bool affectsOnlyClassD = false;

        public static bool doDoorsBreak = true;

        public static bool doWindowsBreak = true;
        public static Dictionary<Window, float> windowHealthList { get; } = new();

        public static List<Player> lockDoors { get; } = new();

        public static Dictionary<ItemType, List<Player>> itemUnableToPickUp { get; } = new();

        private Handlers.Player player;
        private Handlers.Server server;

        public override void OnEnabled() {
            RegisterCommands();
            GetCommands(true);

            base.OnEnabled();
        }

        public override void OnDisabled() {
            UnregisterCommands();
            GetCommands(false);

            base.OnDisabled();
        }

        private void RegisterCommands() {
            player = new(this);
            server = new();

            ItemHandlers.ChargingJailbird += player.OnJailbirdUse;
            PlayerHandlers.UsingMicroHIDEnergy += player.OnMicroEnergyDrain;
            PlayerHandlers.Shot += player.OnWeaponFire;
            PlayerHandlers.DryfiringWeapon += player.OnWeaponDryFire;
            PlayerHandlers.DroppingAmmo += player.OnAmmoDrop;
            PlayerHandlers.Spawned += player.OnSpawn;
            PlayerHandlers.TriggeringTesla += player.OnTeslaGateActivate;
            PlayerHandlers.DamagingDoor += player.OnDoorDamage;
            PlayerHandlers.InteractingDoor += player.OnDoorInteract;
            PlayerHandlers.Dying += player.OnPlayerDeath;
            PlayerHandlers.Handcuffing += player.OnPlayerDetained;
            PlayerHandlers.PickingUpItem += player.OnPickUpItem;

            ServerHandlers.RespawningTeam += server.OnWaveSpawning;
            ServerHandlers.RespawnedTeam += server.OnWaveSpawn;
            ServerHandlers.RoundEnded += server.OnRoundEnd;
        }

        private void UnregisterCommands() {
            ItemHandlers.ChargingJailbird -= player.OnJailbirdUse;
            PlayerHandlers.UsingMicroHIDEnergy -= player.OnMicroEnergyDrain;
            PlayerHandlers.Shot -= player.OnWeaponFire;
            PlayerHandlers.DryfiringWeapon -= player.OnWeaponDryFire;
            PlayerHandlers.DroppingAmmo -= player.OnAmmoDrop;
            PlayerHandlers.Spawned -= player.OnSpawn;
            PlayerHandlers.TriggeringTesla -= player.OnTeslaGateActivate;
            PlayerHandlers.DamagingDoor -= player.OnDoorDamage;
            PlayerHandlers.InteractingDoor -= player.OnDoorInteract;
            PlayerHandlers.Dying -= player.OnPlayerDeath;
            PlayerHandlers.Handcuffing -= player.OnPlayerDetained;
            PlayerHandlers.PickingUpItem -= player.OnPickUpItem;

            ServerHandlers.RespawningTeam -= server.OnWaveSpawning;
            ServerHandlers.RespawnedTeam -= server.OnWaveSpawn;
            ServerHandlers.RoundEnded -= server.OnRoundEnd;

            player = null;
            server = null;
        }

        private void GetCommands(bool enabled) {
            commandList = new();

            if (enabled) {
                commandList.Add("amountofdroppeditems");
                commandList.Add("disablepickups");
                commandList.Add("doorsbreaking");
                commandList.Add("ehreset");
                commandList.Add("giveeffectonspawn");
                commandList.Add("giveitemonwave");
                commandList.Add("infammo");
                commandList.Add("infammoingun");
                commandList.Add("lockingdoors");
                commandList.Add("spawningwithitem");
                commandList.Add("stopteslas");
                commandList.Add("wavesenabled");
                commandList.Add("windowsbreaking");
            }
        }

        public static void ResetCommands() {
            isInfAmmoEnabled = false;

            isInfInGunAmmoEnabled = false;

            areSpawnWavesEnabled = true;

            areItemsBeingGivenOnWave = false;

            areEffectsBeingGivenOnSpawn = false;
            effectNames.Clear();

            areTeslasTriggering = true;

            doPlayersSpawnWithItems = true;
            affectsOnlyClassD = false;

            doDoorsBreak = true;

            doWindowsBreak = true;
            windowHealthList.Clear();

            lockDoors.Clear();

            itemUnableToPickUp.Clear();
        }
    }
}
