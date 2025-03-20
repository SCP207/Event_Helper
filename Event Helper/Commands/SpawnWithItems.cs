using CommandSystem;
using System;
using Exiled.Permissions.Extensions;
using Event_Helper;
using Exiled.Events.Handlers;
using Exiled.API.Features;

namespace Event_Helper.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SpawnWithItems : ICommand, IUsageProvider {
        private int itemID;
        private const int itemIDMax = 59;
        private bool onlySpawnWaves;

        public string Command { get; } = "giveitemonwave";
        public string[] Aliases { get; } = { "gis", "spawngive", "giveitemonspawn" };
        public string Description { get; } = "Gives everyone an item when they spawn, use -1 to disable";
        public string[] Usage { get; } = { "Item ID", "Only spawn waves" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission("eh.wavegiveitems")) {
                response = "You don't have permission to run this command";
                return false;
            }
            if (arguments.Count < 1) {
                response = "Usage: giveitemonwave (Item ID) (Only spawn waves)";
                return false;
            }
            if (arguments.Count != 2) {
                response = "You have too many arguments\nUsage: giveitemonwave (Item ID) (Only spawn waves)";
                return false;
            }
            if (!int.TryParse(arguments.At(0), out itemID)) {
                response = $"Invalid value: {arguments.At(0)}";
                return false;
            }
            if (!bool.TryParse(arguments.At(1), out onlySpawnWaves)) {
                response = $"Invalid value: {arguments.At(1)}";
                return false;
            }

            Plugin.Instance.itemsOnlyOnWaves = onlySpawnWaves;

            // Checks if the item is a valid item //
            if (itemID >= 0 || itemID <= itemIDMax) {
                Plugin.Instance.areItemsBeingGivenOnWave = true;
                Plugin.Instance.itemsBeingGiven = (ItemType)itemID;
                string onlyWavesMessage = (onlySpawnWaves) ? "spawn wave" : "spawn";

                Log.Debug($"Every {onlyWavesMessage} will give item {(ItemType)itemID}");
                response = $"Done! Every {onlyWavesMessage} will give item {(ItemType)itemID}";
            } else {
                Plugin.Instance.areItemsBeingGivenOnWave = false;

                Log.Debug($"Every spawn will not give items");
                response = $"Done! Every spawn will not give items";
            }

            return true;
        }
    }
}
