using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using System.Collections.ObjectModel;
using Event_Helper.Commands;
using Exiled.API.Features.Waves;

namespace Event_Helper {
    public class Plugin : Plugin<Config> {
        public override string Author { get; } = "SCP-207";
        public override string Name { get; } = "Event Helper";
        public override string Prefix { get; } = "EH";
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public override Version RequiredExiledVersion { get; } = new(9, 6, 0);
        public override Version Version { get; } = new(3, 5, 5);

        public static Plugin Instance { get; private set; }

        /*
        public ReadOnlyCollection<string> CommandList { get; } = new(
            new List<string> {
            "amountofdroppeditems",
            "disablepickups",
            "doorsbreaking",
            "ehreset",
            "giveeffectonspawn",
            "giveitemonwave",
            "infammo",
            "infammoingun",
            "lockingdoors",
            "spawningwithitem",
            "stopteslas",
            "wavesenabled",
            "windowsbreaking"
        });
        */

        public bool IsInfAmmoEnabled { get; set; } = false;

        public bool IsInfInGunAmmoEnabled { get; set; } = false;

        public bool AreSpawnWavesEnabled { get; set; } = true;

        public bool AreItemsBeingGivenOnWave { get; set; } = false;
        public ItemType ItemsBeingGiven { get; set; } = ItemType.None;
        public bool ItemsOnlyOnWaves { get; set; } = true;

        public bool AreEffectsBeingGivenOnSpawn { get; set; } = false;
        public List<string> EffectNames { get; } = new();
        public float EffectDuration { get; set; } = 0;
        public byte EffectIntensity { get; set; } = 0;
        public Dictionary<string, byte> EffectIntensityAdditionOverTime { get; } = new();
        public bool EffectsOnlyOnWaves { get; set; } = true;

        public bool AreTeslasTriggering { get; set; } = true;

        public bool DoPlayersSpawnWithItems { get; set; } = true;
        public bool AffectOnlyClassD { get; set; } = false;

        public bool DoDoorsBreak { get; set; } = true;

        public bool DoWindowsBreak { get; set; } = true;
        public Dictionary<Window, float> WindowHealthList { get; } = new();

        public List<Player> PlayersThatLockDoors { get; } = new();

        public Dictionary<ItemType, (List<Player> affectedPlayers, bool affectsEveryone)> ItemUnableToPickUp { get; } = new();

        public override void OnEnabled() {
            Instance = this;
            RegisterCommands();

            base.OnEnabled();
        }

        public override void OnDisabled() {
            Instance = null;
            UnregisterCommands();

            base.OnDisabled();
        }

        private void RegisterCommands() {
            Handlers.Player.RegisterEvents();
            Handlers.Server.RegisterEvents();
        }

        private void UnregisterCommands() {
            Handlers.Player.UnregisterEvents();
            Handlers.Server.UnregisterEvents();
        }

        public void ResetCommands() {
            IsInfAmmoEnabled = false;

            IsInfInGunAmmoEnabled = false;

            AreSpawnWavesEnabled = true;
            WaveTimer.GetWaveTimers().ForEach(w => w.Unpause());

            AreItemsBeingGivenOnWave = false;

            AreEffectsBeingGivenOnSpawn = false;
            EffectNames.Clear();

            AreTeslasTriggering = true;

            DoPlayersSpawnWithItems = true;
            AffectOnlyClassD = false;

            DoDoorsBreak = true;

            DoWindowsBreak = true;
            WindowHealthList.Clear();

            PlayersThatLockDoors.Clear();

            ItemUnableToPickUp.Clear();

            Log.Debug("Commands have been reset");
        }
    }
}
