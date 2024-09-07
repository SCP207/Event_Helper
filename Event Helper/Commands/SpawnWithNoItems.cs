using CommandSystem;
using System;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;
using Event_Helper;

namespace Event_Give_Items.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SpawnWithNoItems : ICommand, IUsageProvider {
        private bool onlyClassD;

        public string Command { get; } = "spawningwithitem";
        public string[] Aliases { get; } = { "swi", "itemspawn" };
        public string Description { get; } = "Doesn't allow players to spawn with items if set to true (toggle)";
        public string[] Usage { get; } = { "Only affects class D" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (!sender.CheckPermission("eh.infammo")) {
                response = "You don't have permission to run this command";
                return false;
            }
            if (arguments.Count < 1) {
                response = "Usage: spawningwithitem [True / False]";
                return false;
            }
            if (arguments.Count != 1) {
                response = "You have too many arguments\nUsage: spawningwithitem [True/False]";
                return false;
            }
            if (!bool.TryParse(arguments.At(0), out onlyClassD)) {
                response = $"Invalid value: {arguments.At(0)}";
                return false;
            }

            Plugin.doPlayersSpawnWithItems = !Plugin.doPlayersSpawnWithItems;
            Plugin.affectsOnlyClassD = onlyClassD;

            Log.Debug($"Spawning with items is set to {Plugin.doPlayersSpawnWithItems}");
            response = $"Done! Spawning with items is now {Plugin.doPlayersSpawnWithItems}";
            return true;
        }
    }
}
