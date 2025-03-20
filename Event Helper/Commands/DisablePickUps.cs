using CommandSystem;
using Event_Helper;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Permissions.Extensions;
using HarmonyLib;
using InventorySystem.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using static PlayerRoles.Spectating.SpectatableModuleBase;

namespace Event_Helper.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class DisablePickUps : ICommand, IUsageProvider {
        public string Command { get; } = "disablepickups";
        public string[] Aliases { get; } = { "dpu", "dp" };
        public string Description { get; } = "Like bypass, but allows a player to lock a door";
        public string[] Usage { get; } = { "Add / Remove", "Item IDs", "%player%" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission("eh.pickups")) {
                response = "You don't have permission to run this command";
                return false;
            }
            if (arguments.Count == 0) {
                response = "Usage: disablepickups [Add / Remove] [Item ID] [%player%]";
                return false;
            }
            if (arguments.Count != 3) {
                response = "You have too many or too little arguments\nUsage: disablepickups [Add / Remove] [Item ID] [%player%]";
                return false;
            }

            string isAdded = (arguments.At(0) == "remove") ? "removed from" : "added to";

            List<ItemType> items = new List<ItemType>();
            if (arguments.At(1) == "*" || arguments.At(1) == "all") {
                for (int index = 0; index <= 54; index++) {
                    items.Add((ItemType)index);
                }

                response = $"Done! Players were {isAdded} DisablePickUps\nItems: All";
            } else {
                string[] itemStrings = arguments.At(1).Split('.');
                foreach (string i in itemStrings) {
                    if (!int.TryParse(i, out var itemId)) {
                        response = $"Invalid value: {arguments.At(0)}";
                        return false;
                    }
                    if (itemId >= 0 && itemId <= 54)
                        items.Add((ItemType)itemId);
                }

                response = $"Done! Players were {isAdded} DisablePickUps\nItems: {items.Log()}";
            }

            bool affectsEveryone = false;
            IEnumerable<Player> players;
            if (arguments.At(2) == "*" || arguments.At(2) == "all") {
                affectsEveryone = true;
                players = Player.Dictionary.Values;
                response += "\nPlayers: All";
            } else {
                players = Player.GetProcessedData(arguments, 2);
                response += $"\nPlayers: {players.Log()}";
            }

            List<Player> playerList = new();
            foreach (ItemType i in items) {
                if (arguments.At(0) == "add") {
                    if (Plugin.Instance.itemUnableToPickUp.ContainsKey(i)) {
                        Plugin.Instance.itemUnableToPickUp.TryGetValue(i, out var playersList);
                        playerList = playersList.affectedPlayers;
                    }
                    playerList.AddRange(players);

                    foreach (var p in players) {
                        foreach (var item in p.Items.Where(item => item.Type == i)) {
                            p.DropItem(item);
                        }
                    }
                } else if (arguments.At(0) == "remove") {
                    if (Plugin.Instance.itemUnableToPickUp.ContainsKey(i)) {
                        Plugin.Instance.itemUnableToPickUp.TryGetValue(i, out var playersList);
                        playerList = playersList.affectedPlayers;
                        playerList.RemoveAll(p => players.Contains(p));
                    } else {
                        playerList = new();
                    }
                } else {
                    response = $"Invalid value: {arguments.At(0)}";
                    return false;
                }

                playerList = playerList.Distinct().ToList();

                Plugin.Instance.itemUnableToPickUp.Remove(i);
                if (playerList.Count > 0)
                    Plugin.Instance.itemUnableToPickUp.Add(i, (playerList, affectsEveryone));
            }

            Log.Debug($"Players can no longer pick up items\nItems: {items.Log()}\nPlayers: {players.Log()}");
            return true;
        }
    }
}
