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
using static PlayerRoles.Spectating.SpectatableModuleBase;

namespace Event_Give_Items.Commands {
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
            /**
            if (!int.TryParse(arguments.At(1), out itemId) && (arguments.At(1) != "*" || arguments.At(1) != "all")) {
                response = $"Invalid value: {arguments.At(0)}";
                return false;
            }
            **/

            string isAdded = "added to";
            if (arguments.At(0) == "remove") {
                isAdded = "removed from";
            }

            IEnumerable<Player> players;
            if (arguments.At(1) == "*" || arguments.At(1) == "all") {
                players = Player.Dictionary.Values;
                response = $"Done! Players were {isAdded} DisablePickUps\nPlayers: All";
            } else {
                players = Player.GetProcessedData(arguments, 2);
                response = $"Done! Players were {isAdded} DisablePickUps\nPlayers: {Extensions.LogPlayers(players)}";
            }

            List<ItemType> items = new List<ItemType>();
            if (arguments.At(1) == "*" || arguments.At(1) == "all") {
                for (int index = 0; index <= 54; index++) {
                    items.Add((ItemType)index);
                }
            } else {
                string[] itemStrings = arguments.At(1).Split('.');
                foreach (string i in itemStrings) {
                    if (!int.TryParse(i, out var itemId)) {
                        response = $"Invalid value: {arguments.At(0)}";
                        return false;
                    }
                    items.Add((ItemType)itemId);
                }
            }

            foreach (Player p in players) {
                List<ItemType> itemsList = new List<ItemType>();
                foreach (ItemType i in items) {
                    if (arguments.At(0) == "add") {
                        if (Plugin.itemUnableToPickUp.ContainsKey(p)) {
                            Plugin.itemUnableToPickUp.TryGetValue(p, out itemsList);
                        }
                        if (!itemsList.Contains(i)) {
                            itemsList.Add(i);

                            List<Item> playerItems = new List<Item>(p.Items);
                            foreach (Item item in playerItems) {
                                if (item.Type == i) {
                                    p.DropItem(item);
                                }
                            }
                        }
                    } else if (arguments.At(0) == "remove") {
                        if (Plugin.itemUnableToPickUp.ContainsKey(p)) {
                            Plugin.itemUnableToPickUp.TryGetValue(p, out itemsList);
                            itemsList.Remove(i);
                        }
                    } else {
                        response = $"Invalid value: {arguments.At(0)}";
                        return false;
                    }
                }
                Plugin.itemUnableToPickUp.Remove(p);
                Plugin.itemUnableToPickUp.Add(p, itemsList);
            }

            Log.Debug($"Players can no longer pick up items\nPlayers: {Extensions.LogPlayers(players)}");
            return true;
        }
    }
}
