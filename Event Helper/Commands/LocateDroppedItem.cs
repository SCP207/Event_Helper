using CommandSystem;
using Event_Helper;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;

namespace Event_Helper.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class AmountOfDroppedItems : ICommand, IUsageProvider {
        int itemId, amount;

        public string Command { get; } = "amountofdroppeditems";
        public string[] Aliases { get; } = { "aodi", "adi", "locateitems", "locatepickup", "lp" };
        public string Description { get; } = "Like bypass, but allows a player to lock a door";
        public string[] Usage { get; } = { "Item ID" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission("eh.locateitems")) {
                response = "You don't have permission to run this command";
                return false;
            }
            if (arguments.Count == 0) {
                response = "Usage: amountofdroppeditems [Item ID]";
                return false;
            }
            if (arguments.Count != 1) {
                response = "You have too many arguments\nUsage: amountofdroppeditems [Item ID]";
                return false;
            }
            if (!int.TryParse(arguments.At(0), out itemId)) {
                response = $"Invalid value: {arguments.At(0)}";
                return false;
            }

            ItemType item = (ItemType)itemId;

            IEnumerable<Pickup> pickups = Pickup.List;
            foreach (Pickup p in pickups) {
                if (p.Type == item) {
                    amount++;
                }
            }

            Log.Debug($"There are {amount} {item}s");
            response = $"Done! There are {amount} {item}s";
            return true;
        }
    }
}
