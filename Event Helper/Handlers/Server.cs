using Exiled.Events.EventArgs.Server;
using System.Threading.Tasks;
using Exiled.API.Features;
using PluginAPI.Events;

namespace Event_Helper.Handlers {
    public class Server {
        private string raCommand;

        public async void OnWaveSpawn(RespawningTeamEventArgs ev) {
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
                raCommand = $"/give {Plugin.playerIdList} {Plugin.itemsBeingGiven}";
                ExecuteCommand(raCommand); 
                raCommand = $"/fequip {Plugin.playerIdList} {Plugin.itemsBeingGiven}";
                await ExecuteCommand(raCommand);
            }

            // Checks if an effect should be gien, gives it, then adds the amount you wanted to add
            if (Plugin.areEffectsBeingGivenOnSpawn) {
                Log.Debug("Effects are being given out on waves from the command \"giveitemonspawn\"");
                raCommand = $"/pfx {Plugin.effect} {Plugin.intensity} {Plugin.duration} {Plugin.playerIdList}";
                ExecuteCommand(raCommand);
                Plugin.intensity += Plugin.additionOverTime;
            }
        }

        public void OnRoundEnding(EndingRoundEventArgs ev) {
            // Clears the ID list when the round ends
            Plugin.playerIdList = "";
            Log.Debug($"The round ended\nEvent Helpers now has the player list of: {Plugin.playerIdList}");
        }

        private async Task ExecuteCommand(string command) {
            // Executes a command using the Server Console
            await Task.Delay(1000);
            Log.Debug($"Event Helpers is running the command \"{command}\"");
            ServerConsole.EnterCommand(command);
        }
    }
}
