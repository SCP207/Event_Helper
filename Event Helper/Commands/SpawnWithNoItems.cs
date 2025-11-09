using CommandSystem;
using System;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;
using Event_Helper;

namespace Event_Helper.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class SpawnWithNoItems : ICommand, IUsageProvider {
    private bool _onlyClassD;

    public string Command => "spawningwithitem";
    public string[] Aliases => [ "swi", "itemspawn" ];
    public string Description => "Doesn't allow players to spawn with items if set to true (toggle)";
    public string[] Usage => [ "Only affects class D" ];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        if (!sender.CheckPermission("eh.noitemspawn")) {
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
        if (!bool.TryParse(arguments.At(0), out _onlyClassD)) {
            response = $"Invalid value: {arguments.At(0)}";
            return false;
        }

        if (Plugin.Instance.AffectOnlyClassD != _onlyClassD) {
            Plugin.Instance.DoPlayersSpawnWithItems = false;
        } else {
            Plugin.Instance.DoPlayersSpawnWithItems = !Plugin.Instance.DoPlayersSpawnWithItems;
        }
        Plugin.Instance.AffectOnlyClassD = _onlyClassD;

        string onlyClassDS = (_onlyClassD) ? "only affects class D" : "doesn't only affect class D";

        Log.Debug($"Spawning with items is set to {Plugin.Instance.DoPlayersSpawnWithItems} and {onlyClassDS}");
        response = $"Done! Spawning with items is now {Plugin.Instance.DoPlayersSpawnWithItems} and {onlyClassDS}";
        return true;
    }
}
