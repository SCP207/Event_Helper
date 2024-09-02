﻿using CommandSystem;
using System;
using Exiled.Permissions.Extensions;
using Event_Helper;

namespace Event_Give_Items.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Waves : ICommand {
        public string Command { get; } = "wavesenabled";

        public string[] Aliases { get; } = { "we", "enablewaves", "wavesoff" };

        public string Description { get; } = "Turns on and off waves (Toggle)";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission("eh.wavesenabled")) {
                response = "You don't have permission to run this command";
                return false;
            }
            if (arguments.Count != 0) {
                response = "You have too many arguments\nUsage: wavesenabled";
                return false;
            }

            Plugin.areSpawnWavesEnabled = !Plugin.areSpawnWavesEnabled;

            string spawnWaves;
            if (Plugin.areSpawnWavesEnabled) {
                spawnWaves = "enabled";
            } else {
                spawnWaves = "disabled";
            }

            response = $"Done! Spawn waves are now {spawnWaves}";
            return true;
        }
    }
}
