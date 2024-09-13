using Exiled.Events.EventArgs.Server;
using System.Threading.Tasks;
using Exiled.API.Features;
using System.Collections.Generic;
using Players = Exiled.API.Features.Player;

namespace Event_Helper.Handlers {
    public class Server {
        public void OnWaveSpawn(RespawningTeamEventArgs ev) {
            // Checks if spawn waves are enabled, doesn't run the rest if they are disabled
            if (!Plugin.areSpawnWavesEnabled) {
                Log.Debug("Spawn waves are not enabled from the command \"wavesenabled\"");
                ev.IsAllowed = false;
                return;
            }

            Log.Debug("Spawn waves are enabled from the command \"wavesenabled\"");
            ev.IsAllowed = true;

            // Checks if an item should be given, then gives and force equips the item
            if (Plugin.areItemsBeingGivenOnWave) {
                Log.Debug("Items are being given out on waves from the command \"giveitemonspawn\"");
                Log.Debug($"The item being given is {Plugin.itemsBeingGiven}");
                GiveItems(); 
            }

            // Checks if an effect should be gien, gives it, then adds the amount you wanted to add
            if (Plugin.areEffectsBeingGivenOnSpawn) {
                Log.Debug("Effects are being given out on waves from the command \"giveitemonspawn\"");
                GiveEffects();
            }
        }

        private async Task GiveItems() {
            await Task.Delay(1000);

            // Gives the requested item
            IEnumerable<Players> players = Players.Dictionary.Values;
            ItemType item = (ItemType)Plugin.itemsBeingGiven;
            foreach (Players p in players) {
                p.CurrentItem = p.AddItem(item);
            }
        }
        private async Task GiveEffects() {
            await Task.Delay(1000);

            // Gives the requested effect
            IEnumerable<Players> players = Players.Dictionary.Values;
            foreach (Players p in players) {
                foreach (string effectName in Plugin.effectNames) {
                    p.EnableEffect(effectName, Plugin.effectIntensity, Plugin.effectDuration);
                }
            }

            // Adds the "additionOverTime" value to the intensity
            if (Plugin.effectIntensity + Plugin.effectIntensityAdditionOverTime <= 255) {
                Plugin.effectIntensity += Plugin.effectIntensityAdditionOverTime;
            } else {
                Plugin.effectIntensity = 255;
            }
        }
    }
}
