﻿using CommandSystem;
using System;
using Exiled.Permissions.Extensions;
using Event_Helper;

namespace Event_Helper.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class SpawnWithEffects : ICommand, IUsageProvider {
        private string effect;
        private int duration;
        private byte intensity, additionOverTime;

        public string Command { get; } = "giveeffectonspawn";
        public string[] Aliases { get; } = { "ges", "spawneffect" };
        public string Description { get; } = "Gives everyone an effect when the spawn in";
        public string[] Usage { get; } = { "Effect (or false)", "Duration", "Intensity", "How much to add over time" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission("eh.wavegiveeffects")) {
                response = "You don't have permission to run this command";
                return false;
            }
            if (arguments.Count < 1) {
                response = "Usage: giveeffectonspawn (Effect [or false]) (Duration [0 for none]) (Intensity [255 max]) (How much to add over time [0 for none])";
                return false;
            }
            if (arguments.Count != 4 && arguments.Count != 1) {
                response = "You have too many or too little arguments\nUsage: giveeffectonspawn (Effect [or false]) (Duration [0 for none]) (Intensity [255 max]) (How much to add over time [0 for none])";
                return false;
            }
            if (arguments.At(0) == "false") {
                Plugin.areEffectsBeingGivenOnSpawn = false;
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

            effect = arguments.At(0);

            Plugin.areEffectsBeingGivenOnSpawn = true;
            Plugin.effectNames.Add(effect);
            Plugin.effectDuration = duration;
            Plugin.effectIntensity = intensity;
            Plugin.effectIntensityAdditionOverTime.Add(effect, additionOverTime);

            response = $"Done! Every spawn wave will give the effect {effect} for {duration} seconds with intensity {intensity}";
            return true;
        }
    }
}
