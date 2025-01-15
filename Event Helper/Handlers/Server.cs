using Exiled.Events.EventArgs.Server;
using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features.Items;

using ServerHandlers = Exiled.Events.Handlers.Server;

using EPlayer = Exiled.API.Features.Player;

namespace Event_Helper.Handlers {
    public static class Server {
        public static void RegisterEvents() {
            ServerHandlers.RespawnedTeam += OnWaveSpawn;
            ServerHandlers.RoundStarted += OnRoundStart;
        }

        public static void UnregisterEvents() {
            ServerHandlers.RespawnedTeam -= OnWaveSpawn;
            ServerHandlers.RoundStarted -= OnRoundStart;
        }

        private static void OnWaveSpawn(RespawnedTeamEventArgs ev) {
            // Checks if an item should be given, then gives and force equips the item //
            if (Plugin.Instance.areItemsBeingGivenOnWave && Plugin.Instance.itemsOnlyOnWaves) {
                Log.Debug("Items are being given out on waves from the command \"giveitemonspawn\"");
                Log.Debug($"The item being given is {Plugin.Instance.itemsBeingGiven}");

                IEnumerable<EPlayer> players = EPlayer.Dictionary.Values;
                foreach (EPlayer p in players) {
                    Item i = p.AddItem(Plugin.Instance.itemsBeingGiven);
                    if (ev.Players.Contains(p) || p.IsScp) {
                        p.CurrentItem = i;
                    }
                }
            }

            // Checks if an effect should be gien, gives it, then adds the amount you wanted to add //
            if (Plugin.Instance.areEffectsBeingGivenOnSpawn) {
                if (Plugin.Instance.effectsOnlyOnWaves) {
                    Log.Debug("Effects are being given out on waves from the command \"giveitemonspawn\"");

                    // Gives the requested effect
                    IEnumerable<EPlayer> players = EPlayer.Dictionary.Values;
                    foreach (EPlayer p in players) {
                        foreach (string effectName in Plugin.Instance.effectNames) {
                            p.EnableEffect(effectName, Plugin.Instance.effectIntensity, Plugin.Instance.effectDuration);
                        }
                    }
                }

                // Adds the "effectIntensityAdditionOverTime" value to the intensity //
                foreach (string effect in Plugin.Instance.effectIntensityAdditionOverTime.Keys) {
                    Plugin.Instance.effectIntensityAdditionOverTime.TryGetValue(effect, out byte addition);
                    if (Plugin.Instance.effectIntensity + addition >= 255) {
                        Plugin.Instance.effectIntensity = 255;
                    } else if (Plugin.Instance.effectIntensity + addition <= 0) {
                        Plugin.Instance.effectIntensity = 0;
                    } else {
                        Plugin.Instance.effectIntensity += addition;
                    }
                }
            }
        }

        private static void OnRoundStart() => Plugin.Instance.ResetCommands();
    }
}
