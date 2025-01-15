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
        public override Version RequiredExiledVersion { get; } = new(9, 3, 0);
        public override Version Version { get; } = new(3, 5, 1);

        public static Plugin Instance { get; private set; }

        public ReadOnlyCollection<string> commandList { get; } = new(new List<string> {
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

        public bool isInfAmmoEnabled = false;

        public bool isInfInGunAmmoEnabled = false;

        public bool areSpawnWavesEnabled { get; set; } = true;

        public bool areItemsBeingGivenOnWave = false;
        public ItemType itemsBeingGiven;
        public bool itemsOnlyOnWaves;

        public bool areEffectsBeingGivenOnSpawn = false;
        public List<string> effectNames { get; } = new();
        public int effectDuration;
        public byte effectIntensity;
        public Dictionary<string, byte> effectIntensityAdditionOverTime = new();
        public bool effectsOnlyOnWaves;

        public bool areTeslasTriggering = true;

        public bool doPlayersSpawnWithItems = true;
        public bool affectsOnlyClassD = false;

        public bool doDoorsBreak = true;

        public bool doWindowsBreak = true;
        public Dictionary<Window, float> windowHealthList { get; } = new();

        public List<Player> lockDoors { get; } = new();

        public Dictionary<ItemType, List<Player>> itemUnableToPickUp { get; } = new();

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
            isInfAmmoEnabled = false;

            isInfInGunAmmoEnabled = false;

            areSpawnWavesEnabled = true;
            var waves = WaveTimer.GetWaveTimers();
            waves.ForEach(w => {
                w.Unpause();
                Log.Debug("Reset Waves");
            });

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
