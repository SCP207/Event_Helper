using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Helper.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class GiveAll : ICommand, IUsageProvider {
        public string Command { get; } = "giveall";

        public string[] Aliases { get; } = Array.Empty<string>();

        public string Description { get; } = "Gives items to all players";

        public string[] Usage { get; } = { "Items" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission(PlayerPermissions.GivingItems)) {
                response = "You don't have the permission to use this command";
                return false;
            }

            if (arguments.Count == 0) {
                response = "Usage: giveall [Items]";
                return false;
            }
            if (arguments.Count != 1) {
                response = "Incorrect amount of arguments: Usage: giveall [Items]";
                return false;
            }

            var itemStrings = arguments.At(0).Split('.');
            List<ItemType> items = new();
            foreach (var itemString in itemStrings) {
                if (!Enum.TryParse(itemString, false, out ItemType itemType)) {
                    response = $"One of your items was not an item: {arguments.At(0)}";
                    return false;
                }
                items.Add(itemType);
            }

            foreach (var player in Player.List)
                player.AddItem(items);

            Log.Debug($"All players were given items: {arguments.At(0)}");
            response = "Done! Gave all players items";
            return true;
        }
    }
}
