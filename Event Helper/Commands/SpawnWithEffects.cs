using CommandSystem;
using System;
using Exiled.Permissions.Extensions;
using Event_Helper;
using Exiled.API.Features;

namespace Event_Helper.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class SpawnWithEffects : ICommand, IUsageProvider {
        private string effect;
        private int duration;
        private byte intensity, additionOverTime;
        private bool onlySpawnWaves;

        public string Command { get; } = "giveeffectonspawn";
        public string[] Aliases { get; } = { "ges", "spawneffect" };
        public string Description { get; } = "Gives everyone an effect when the spawn in";
        public string[] Usage { get; } = { "Effect (or false)", "Duration", "Intensity", "How much to add over time", "Only on spawn waves" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission("eh.wavegiveeffects")) {
                response = "You don't have permission to run this command";
                return false;
            }
            if (arguments.Count < 1) {
                response = "Usage: giveeffectonspawn (Effect [or false]) (Duration [0 for none]) (Intensity [255 max]) (How much to add over time [0 for none]) (Only on spawn waves)";
                return false;
            }
            if (arguments.Count != 5 && arguments.Count != 1) {
                response = "You have too many or too little arguments\nUsage: giveeffectonspawn (Effect [or false]) (Duration [0 for none]) (Intensity [255 max]) (How much to add over time [0 for none]) (Only on spawn waves)";
                return false;
            }
            if (arguments.At(0) == "false") {
                Plugin.Instance.areEffectsBeingGivenOnSpawn = false;
                response = $"Done! Every spawn wave will not give effects";
                return true;
            }
            if (!int.TryParse(arguments.At(1), out duration)) {
                response = $"Invalid value: {arguments.At(1)}";
                return false;
            }
            if (!byte.TryParse(arguments.At(2), out intensity)) {
                response = $"Invalid value: {arguments.At(2)}";
                return false;
            }
            if (!byte.TryParse(arguments.At(3), out additionOverTime)) {
                response = $"Invalid value: {arguments.At(3)}";
                return false;
            }
            if (!bool.TryParse(arguments.At(4), out onlySpawnWaves)) {
                response = $"Invalid value: {arguments.At(4)}";
                return false;
            }

            effect = arguments.At(0);

            Plugin.Instance.areEffectsBeingGivenOnSpawn = true;
            Plugin.Instance.effectNames.Add(effect);
            Plugin.Instance.effectDuration = duration;
            Plugin.Instance.effectIntensity = intensity;
            Plugin.Instance.effectIntensityAdditionOverTime.Add(effect, additionOverTime);
            Plugin.Instance.effectsOnlyOnWaves = onlySpawnWaves;

            string onlyWavesMessage = (onlySpawnWaves) ? "spawn wave" : "spawn";

            Log.Debug($"Every {onlyWavesMessage} will give the effect {effect} for {duration} seconds with intensity {intensity}");
            response = $"Done! Every {onlyWavesMessage} will give the effect {effect} for {duration} seconds with intensity {intensity}";
            return true;
        }
    }
}
