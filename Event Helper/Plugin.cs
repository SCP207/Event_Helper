using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerHandlers = Exiled.Events.Handlers.Player;
using ServerHandlers = Exiled.Events.Handlers.Server;

namespace Event_Helper {
    public class Plugin : Plugin<Config> {
        public static Plugin Instance { get; private set; }
        public static Config ConfigInstance { get; private set; }

        public override string Author { get; } = "SCP-207";

        public override string Name { get; } = "Event Helpers";

        public override string Prefix { get; } = "EH";

        public override PluginPriority Priority { get; } = PluginPriority.Default;

        public override Version RequiredExiledVersion { get; } = new Version(8, 7, 2);

        public override Version Version { get; } = new Version(1, 0, 0);

        public string playerIdList;

        public bool isInfAmmoEnabled = false;

        public bool areSpawnWavesEnabled = true;

        public bool areItemsBeingGivenOnWave = false;
        public int itemsBeingGiven;

        public bool areEffectsBeingGivenOnSpawn = false;
        public string effect;
        public int duration;
        public int intensity;
        public int additionOverTime;

        public bool areTeslasTriggering = true;

        private Handlers.Player player;
        private Handlers.Server server;

        public override void OnEnabled() {
            RegisterCommands();

            base.OnEnabled();
        }

        public override void OnDisabled() {
            UnregisterCommands();

            base.OnDisabled();
        }

        public override void OnReloaded() {
            base.OnReloaded();
        }
        private void RegisterCommands() {
            Instance = this;
            ConfigInstance = this.Config;

            player = new Handlers.Player();
            server = new Handlers.Server();

            PlayerHandlers.ReloadingWeapon += player.OnWeaponReload;
            PlayerHandlers.Verified += player.OnVerified;
            PlayerHandlers.TriggeringTesla += player.OnTeslaGateActivate;

            ServerHandlers.RespawningTeam += server.OnWaveSpawn;
            ServerHandlers.EndingRound += server.OnRoundEnding;
        }

        private void UnregisterCommands() {
            PlayerHandlers.ReloadingWeapon -= player.OnWeaponReload;
            PlayerHandlers.Verified -= player.OnVerified;
            PlayerHandlers.TriggeringTesla -= player.OnTeslaGateActivate;

            ServerHandlers.RespawningTeam -= server.OnWaveSpawn;
            ServerHandlers.EndingRound -= server.OnRoundEnding;

            player = null;
            server = null;

            ConfigInstance = null;
            Instance = null;
        }
    }
}
