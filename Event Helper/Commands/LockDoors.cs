﻿using CommandSystem;
using Event_Helper;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;

namespace Event_Give_Items.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class LockDoors : ICommand, IUsageProvider {
        public string Command { get; } = "lockdoors";
        public string[] Aliases { get; } = { "doorlocking", "dl" };
        public string Description { get; } = "Like bypass, but allows a player to lock a door";
        public string[] Usage { get; } = { "Add / Remove", "%player%"};

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission("eh.lockdoors")) {
                response = "You don't have permission to run this command";
                return false;
            }
            if (arguments.Count == 0) {
                response = "Usage: lockdoors [Add / Remove] [%player%]";
                return false;
            }
            if (arguments.Count != 2) {
                response = "You have too many or too little arguments\nUsage: lockdoors [Add / Remove] [%player%]";
                return false;
            }

            IEnumerable<Player> players;
            if (arguments.At(1) == "*" || arguments.At(1) == "all") {
                players = Player.Dictionary.Values;
                if (arguments.At(0) == "remove") {
                    Plugin.lockDoors.Clear();
                } else if (arguments.At(0) == "add") {
                    Plugin.lockDoors.AddRange(players);
                } else {
                    response = $"Invalid value: {arguments.At(0)}";
                    return false;
                }
            } else {
                players = Player.GetProcessedData(arguments);
                foreach (Player p in players) {
                    if (arguments.At(0) == "remove") {
                        Plugin.lockDoors.Remove(p);
                    } else if (arguments.At(0) == "add") {
                        Plugin.lockDoors.Add(p);
                    } else {
                        response = $"Invalid value: {arguments.At(0)}";
                        return false;
                    }
                }
            }

            string isAdded = "added to";
            if (arguments.At(0) == "remove") {
                isAdded = "removed from";
            }

            Log.Debug($"Players {isAdded} lock doors");
            response = $"Done! Players were {isAdded} LockDoors";
            Plugin.lockDoors.RemoveRange(0, Plugin.lockDoors.Count);
            Log.Debug($"Players are no longer able to lock doors");
            response = $"Done! All players have been removed from LockDoors";

            return true;
        }
    }
}
