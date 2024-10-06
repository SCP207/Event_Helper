using Exiled.Events.EventArgs.Server;
using System.Threading.Tasks;
using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features.Items;
using Players = Exiled.API.Features.Player;

namespace Event_Helper.Handlers {
    public class Server {
        public void OnWaveSpawning(RespawningTeamEventArgs ev) {
            // Checks if spawn waves are enabled, doesn't run the rest if they are disabled
            if (!Plugin.areSpawnWavesEnabled) {
                Log.Debug("Spawn waves are not enabled from the command \"wavesenabled\"");
                ev.IsAllowed = false;
                return;
            }
            Log.Debug("Spawn waves are enabled from the command \"wavesenabled\"");
        }
        public void OnWaveSpawn(RespawnedTeamEventArgs ev) {
            // Checks if an item should be given, then gives and force equips the item
            if (Plugin.areItemsBeingGivenOnWave) {
                Log.Debug("Items are being given out on waves from the command \"giveitemonspawn\"");
                Log.Debug($"The item being given is {Plugin.itemsBeingGiven}");

                IEnumerable<Players> players = Players.Dictionary.Values;
                foreach (Players p in players) {
                    Item i = p.AddItem(Plugin.itemsBeingGiven);
                    if (ev.Players.Contains(p) || p.IsScp) {
                        p.CurrentItem = i;
                    }
                }
            }

            // Checks if an effect should be gien, gives it, then adds the amount you wanted to add
            if (Plugin.areEffectsBeingGivenOnSpawn) {
                Log.Debug("Effects are being given out on waves from the command \"giveitemonspawn\"");

                // Gives the requested effect
                IEnumerable<Players> players = Players.Dictionary.Values;
                foreach (Players p in players) {
                    foreach (string effectName in Plugin.effectNames) {
                        p.EnableEffect(effectName, Plugin.effectIntensity, Plugin.effectDuration);
                    }
                }

                // Adds the "effectIntensityAdditionOverTime" value to the intensity
                foreach (string effect in Plugin.effectIntensityAdditionOverTime.Keys) {
                    Plugin.effectIntensityAdditionOverTime.TryGetValue(effect, out byte addition);
                    if (Plugin.effectIntensity + addition >= 255) {
                        Plugin.effectIntensity = 255;
                    } else if (Plugin.effectIntensity + addition <= 0) {
                        Plugin.effectIntensity = 0;
                    } else {
                        Plugin.effectIntensity += addition;
                    }
                }
            }
        }

        public void OnRoundEnd(RoundEndedEventArgs ev) {
            Plugin.ResetCommands();
        }
    }
}
