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
        public override string Name { get; } = "Event Helpers";
        public override string Prefix { get; } = "EH";
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public override Version RequiredExiledVersion { get; } = new Version(8, 11, 0);
        public override Version Version { get; } = new Version(3, 3, 2);
        public static Config config { get; private set; } = new Config();

        public static List<string> commandList = new List<string>();

        public static bool isInfAmmoEnabled = false;

        public static bool isInfInGunAmmoEnabled = false;

        public static bool areSpawnWavesEnabled = true;

        public static bool areItemsBeingGivenOnWave = false;
        public static ItemType itemsBeingGiven;

        public static bool areEffectsBeingGivenOnSpawn = false;
        public static List<string> effectNames { get; } = new List<string>();
        public static int effectDuration;
        public static byte effectIntensity;
        public static byte effectIntensityAdditionOverTime;

        public static bool areTeslasTriggering = true;

        public static bool doPlayersSpawnWithItems = true;
        public static bool affectsOnlyClassD = false;

        public static bool doDoorsBreak = true;

        public static bool doWindowsBreak = true;
        public static Dictionary<Window, float> windowHealthList { get; } = new Dictionary<Window, float>();

        public static List<Player> lockDoors { get; } = new List<Player>();

        public static Dictionary<Player, List<ItemType>> itemUnableToPickUp { get; } = new Dictionary<Player, List<ItemType>>();

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
            config = this.Config;

            player = new Handlers.Player();
            server = new Handlers.Server();

            ItemHandlers.ChargingJailbird += player.OnJailbirdUse;
            PlayerHandlers.Shot += player.OnWeaponFire;
            PlayerHandlers.ReloadingWeapon += player.OnWeaponReload;
            PlayerHandlers.DroppingAmmo += player.OnAmmoDrop;
            PlayerHandlers.Spawned += player.OnSpawn;
            PlayerHandlers.TriggeringTesla += player.OnTeslaGateActivate;
            PlayerHandlers.DamagingDoor += player.OnDoorDamage;
            PlayerHandlers.PlayerDamageWindow += player.OnWindowDamage;
            PlayerHandlers.InteractingDoor += player.OnDoorInteract;
            PlayerHandlers.Dying += player.OnPlayerDeath;
            PlayerHandlers.Handcuffing += player.OnPlayerDetained;
            PlayerHandlers.PickingUpItem += player.OnPickUpItem;

            ServerHandlers.RespawningTeam += server.OnWaveSpawning;
            ServerHandlers.RespawnedTeam += server.OnWaveSpawn;
        }

        private void UnregisterCommands() {
            ItemHandlers.ChargingJailbird -= player.OnJailbirdUse;
            PlayerHandlers.Shot -= player.OnWeaponFire;
            PlayerHandlers.ReloadingWeapon -= player.OnWeaponReload;
            PlayerHandlers.DroppingAmmo -= player.OnAmmoDrop;
            PlayerHandlers.Spawned -= player.OnSpawn;
            PlayerHandlers.TriggeringTesla -= player.OnTeslaGateActivate;
            PlayerHandlers.DamagingDoor -= player.OnDoorDamage;
            PlayerHandlers.PlayerDamageWindow -= player.OnWindowDamage;
            PlayerHandlers.InteractingDoor -= player.OnDoorInteract;
            PlayerHandlers.Dying -= player.OnPlayerDeath;
            PlayerHandlers.Handcuffing -= player.OnPlayerDetained;
            PlayerHandlers.PickingUpItem -= player.OnPickUpItem;

            ServerHandlers.RespawningTeam -= server.OnWaveSpawning;
            ServerHandlers.RespawnedTeam -= server.OnWaveSpawn;

            player = null;
            server = null;

            config = null;
        }

        private void GetCommands(bool enabled) {
            commandList = new List<string>();

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
