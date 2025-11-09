using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using System;
using System.Linq;

namespace Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class TransferItem : ICommand, IUsageProvider {
    public string Command => "transferitem";

    public string[] Aliases => [ "transferhelditem", "transfer", "itemtransfer" ];

    public string Description => "Transfers a players held item (or an item is a slot of your choice) to another player";

    public string[] Usage => [ "From", "To", "Slot Number (optional)" ];

    public string UsageMessage => $"Usage: {Command} [{string.Join("] [", Usage)}]";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        if (arguments.Count == 0) {
            response = UsageMessage;
            return false;
        }

        if (arguments.Count != 2 && arguments.Count != 3) {
            response = $"You have too many or too little arguments\n{UsageMessage}";
            return false;
        }

        if (!Player.TryGet(arguments.At(0), out var from)) {
            response = $"Could not find player: {arguments.At(0)}";
            return false;
        }
        if (!Player.TryGet(arguments.At(1), out var to)) {
            response = $"Could not find player: {arguments.At(1)}";
            return false;
        }

        Item transferItem = from.CurrentItem;
        if (arguments.Count == 3 && !int.TryParse(arguments.At(2), out var slot) && slot >= 0 && slot < 8)
            transferItem = from.Items.ElementAt(slot);

        if (transferItem is null) {
            response = $"Item not found on player {from.Nickname}";
            return false;
        }

        var pickup = from.DropItem(transferItem);
        var item = to.AddItem(pickup);
        if (!to.HasItem(item))
            to.AddItem(pickup);

        pickup.Destroy();

        response = "Done! Item successfully transfered";
        return true;
    }
}
