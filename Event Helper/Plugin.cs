using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerHandlers = Exiled.Events.Handlers.Player;
using ServerHandlers = Exiled.Events.Handlers.Server;

namespace Event_Helper {
    public class Plugin : Plugin<Config> {
        public override string Author { get; } = "SCP-207";

        public override string Name { get; } = "Event Helpers";

        public override string Prefix { get; } = "EH";

        public override PluginPriority Priority { get; } = PluginPriority.Default;

        public override Version RequiredExiledVersion { get; } = new Version(8, 7, 2);

        public override Version Version { get; } = new Version(1, 1, 1);

        public static string playerIdList;

        public static bool isInfAmmoEnabled = false;

        public static bool areSpawnWavesEnabled = true;

        public static bool areItemsBeingGivenOnWave = false;
        public static int itemsBeingGiven;

        public static bool areEffectsBeingGivenOnSpawn = false;
        public static string effect;
        public static int duration;
        public static int intensity;
        public static int additionOverTime;

        public static bool areTeslasTriggering = true;

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

        private void RegisterCommands() {
            player = new Handlers.Player();
            server = new Handlers.Server();

            PlayerHandlers.ReloadingWeapon += player.OnWeaponReload;
            PlayerHandlers.DroppingAmmo += player.OnAmmoDrop;
            PlayerHandlers.Spawned += player.OnSpawn;
            PlayerHandlers.Verified += player.OnVerified;
            PlayerHandlers.TriggeringTesla += player.OnTeslaGateActivate;

            ServerHandlers.RespawningTeam += server.OnWaveSpawn;
            ServerHandlers.EndingRound += server.OnRoundEnding;
        }

        private void UnregisterCommands() {
            PlayerHandlers.ReloadingWeapon -= player.OnWeaponReload;
            PlayerHandlers.DroppingAmmo -= player.OnAmmoDrop;
            PlayerHandlers.Spawned -= player.OnSpawn;
            PlayerHandlers.Verified -= player.OnVerified;
            PlayerHandlers.TriggeringTesla -= player.OnTeslaGateActivate;

            ServerHandlers.RespawningTeam -= server.OnWaveSpawn;
            ServerHandlers.EndingRound -= server.OnRoundEnding;

            player = null;
            server = null;
        }
    }
}
