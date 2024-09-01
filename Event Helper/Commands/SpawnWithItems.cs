using CommandSystem;
using System;
using Exiled.Permissions.Extensions;
using Event_Helper;

namespace Event_Give_Items.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SpawnWithItems : ICommand, IUsageProvider {
        private int itemID, itemIDMax = 54, itemIDMin = 0;

        public string Command { get; } = "giveitemonwave";

        public string[] Aliases { get; } = { "gis", "spawngive", "giveitemonspawn" };

        public string Description { get; } = "Gives everyone an item when they spawn, use -1 to disable";

        public string[] Usage { get; } = { "Item ID" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission("eh.wavegiveitems")) {
                response = "You don't have permission to run this command";
                return false;
            }
            if (arguments.Count < 1) {
                response = "Usage: giveitemonwave (Item ID)";
                return false;
            }
            if (arguments.Count != 1) {
                response = "You have too many arguments\nUsage: giveitemonwave (Item ID)";
                return false;
            }
            if (!int.TryParse(arguments.At(0), out itemID)) {
                response = $"Invalid value: {arguments.At(0)}";
                return false;
            }

            // Checks if the item is a valid item
            if (itemID == -1) {
                Plugin.Instance.areItemsBeingGivenOnWave = false;
                response = $"Done! Players won't be given items when they spawn";
            } else if (itemID >= itemIDMin || itemID <= itemIDMax) {
                Plugin.Instance.areItemsBeingGivenOnWave = true;
                Plugin.Instance.itemsBeingGiven = itemID;
                response = $"Done! Every spawn wave will give item {itemID}";
            } else {
                response = $"Invalid value: {arguments.At(0)}\nMust be between -1 and 54";
                return false;
            }
            return true;
        }
    }
}
