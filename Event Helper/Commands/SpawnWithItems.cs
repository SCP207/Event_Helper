using CommandSystem;
using System;
using Exiled.Permissions.Extensions;
using Event_Helper;
using Exiled.Events.Handlers;
using Exiled.API.Features;

namespace Event_Helper.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class SpawnWithItems : ICommand, IUsageProvider {
    private int _itemID;
    private const int ITEM_ID_MAX = 59;
    private bool _onlySpawnWaves;

    public string Command => "giveitemonwave";
    public string[] Aliases => ["gis", "spawngive", "giveitemonspawn"];
    public string Description => "Gives everyone an item when they spawn, use -1 to disable";
    public string[] Usage => ["Item ID", "Only spawn waves"];

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
        if (!int.TryParse(arguments.At(0), out _itemID)) {
            response = $"Invalid value: {arguments.At(0)}";
            return false;
        }
        if (!bool.TryParse(arguments.At(1), out _onlySpawnWaves)) {
            response = $"Invalid value: {arguments.At(1)}";
            return false;
        }

        Plugin.Instance.ItemsOnlyOnWaves = _onlySpawnWaves;

        // Checks if the item is a valid item //
        if (_itemID >= 0 || _itemID <= ITEM_ID_MAX) {
            Plugin.Instance.AreItemsBeingGivenOnWave = true;
            Plugin.Instance.ItemsBeingGiven = (ItemType)_itemID;
            string onlyWavesMessage = (_onlySpawnWaves) ? "spawn wave" : "spawn";

            Log.Debug($"Every {onlyWavesMessage} will give item {(ItemType)_itemID}");
            response = $"Done! Every {onlyWavesMessage} will give item {(ItemType)_itemID}";
        } else {
            Plugin.Instance.AreItemsBeingGivenOnWave = false;

            Log.Debug($"Every spawn will not give items");
            response = $"Done! Every spawn will not give items";
        }

        return true;
    }
}
