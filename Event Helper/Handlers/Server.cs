using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Server;
using System.Collections.Generic;
using System.Linq;

using ServerHandlers = Exiled.Events.Handlers.Server;

using ExiledPlayer = Exiled.API.Features.Player;

namespace Event_Helper.Handlers;

public static class Server {
    public static void RegisterEvents() {
        ServerHandlers.RespawnedTeam += OnWaveSpawn;
        ServerHandlers.WaitingForPlayers += OnWaitingForPlayers;
    }

    public static void UnregisterEvents() {
        ServerHandlers.RespawnedTeam -= OnWaveSpawn;
        ServerHandlers.WaitingForPlayers -= OnWaitingForPlayers;
    }

    private static void OnWaveSpawn(RespawnedTeamEventArgs ev) {
        // Checks if an item should be given, then gives and force equips the item //
        if (Plugin.Instance.AreItemsBeingGivenOnWave && Plugin.Instance.ItemsOnlyOnWaves) {
            Log.Debug("Items are being given out on waves from the command \"giveitemonspawn\"");
            Log.Debug($"The item being given is {Plugin.Instance.ItemsBeingGiven}");

            var players = ExiledPlayer.Dictionary.Values;
            foreach (ExiledPlayer p in players) {
                Item i = p.AddItem(Plugin.Instance.ItemsBeingGiven);
                if (ev.Players.Contains(p) || p.IsScp) {
                    p.CurrentItem = i;
                }
            }
        }

        // Checks if an effect should be gien, gives it, then adds the amount you wanted to add //
        if (Plugin.Instance.AreEffectsBeingGivenOnSpawn) {
            if (Plugin.Instance.EffectsOnlyOnWaves) {
                Log.Debug("Effects are being given out on waves from the command \"giveitemonspawn\"");

                // Gives the requested effect
                IEnumerable<ExiledPlayer> players = ExiledPlayer.Dictionary.Values;
                foreach (ExiledPlayer p in players) {
                    foreach (string effectName in Plugin.Instance.EffectNames) {
                        p.EnableEffect(effectName, Plugin.Instance.EffectIntensity, Plugin.Instance.EffectDuration);
                    }
                }
            }

            // Adds the "effectIntensityAdditionOverTime" value to the intensity //
            foreach (string effect in Plugin.Instance.EffectIntensityAdditionOverTime.Keys) {
                Plugin.Instance.EffectIntensityAdditionOverTime.TryGetValue(effect, out byte addition);
                if (Plugin.Instance.EffectIntensity + addition >= 255) {
                    Plugin.Instance.EffectIntensity = 255;
                } else if (Plugin.Instance.EffectIntensity + addition <= 0) {
                    Plugin.Instance.EffectIntensity = 0;
                } else {
                    Plugin.Instance.EffectIntensity += addition;
                }
            }
        }
    }

    private static void OnWaitingForPlayers() =>
        Plugin.Instance.ResetCommands();
}
