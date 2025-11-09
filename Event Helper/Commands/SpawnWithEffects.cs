using CommandSystem;
using System;
using Exiled.Permissions.Extensions;
using Event_Helper;
using Exiled.API.Features;

namespace Event_Helper.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal class SpawnWithEffects : ICommand, IUsageProvider {
    private string _effect;
    private float _duration;
    private byte _intensity, _additionOverTime;
    private bool _onlySpawnWaves;

    public string Command => "giveeffectonspawn";
    public string[] Aliases => ["ges", "spawneffect"];
    public string Description => "Gives everyone an effect when the spawn in";
    public string[] Usage => ["Effect (or false)", "Duration", "Intensity", "How much to add over time", "Only on spawn waves"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        if (!sender.CheckPermission("eh.wavegiveeffects")) {
            response = "You don't have permission to run this command";
            return false;
        }
        if (arguments.Count < 1) {
            response = "Usage: giveeffectonspawn (Effect [or false]) (Duration [0 for none]) (Intensity [255 max]) (How much to add over time [0 for none]) (Only on spawn waves)";
            return false;
        }
        if (arguments.Count != 5 && arguments.Count != 1) {
            response = "You have too many or too little arguments\nUsage: giveeffectonspawn (Effect [or false]) (Duration [0 for none]) (Intensity [255 max]) (How much to add over time [0 for none]) (Only on spawn waves)";
            return false;
        }
        if (arguments.At(0) == "false") {
            Plugin.Instance.AreEffectsBeingGivenOnSpawn = false;
            response = $"Done! Every spawn wave will not give effects";
            return true;
        }
        if (!float.TryParse(arguments.At(1), out _duration)) {
            response = $"Invalid value: {arguments.At(1)}";
            return false;
        }
        if (!byte.TryParse(arguments.At(2), out _intensity)) {
            response = $"Invalid value: {arguments.At(2)}";
            return false;
        }
        if (!byte.TryParse(arguments.At(3), out _additionOverTime)) {
            response = $"Invalid value: {arguments.At(3)}";
            return false;
        }
        if (!bool.TryParse(arguments.At(4), out _onlySpawnWaves)) {
            response = $"Invalid value: {arguments.At(4)}";
            return false;
        }

        _effect = arguments.At(0);

        Plugin.Instance.AreEffectsBeingGivenOnSpawn = true;
        Plugin.Instance.EffectNames.Add(_effect);
        Plugin.Instance.EffectDuration = _duration;
        Plugin.Instance.EffectIntensity = _intensity;
        Plugin.Instance.EffectIntensityAdditionOverTime.Add(_effect, _additionOverTime);
        Plugin.Instance.EffectsOnlyOnWaves = _onlySpawnWaves;

        string onlyWavesMessage = (_onlySpawnWaves) ? "spawn wave" : "spawn";

        Log.Debug($"Every {onlyWavesMessage} will give the effect {_effect} for {_duration} seconds with intensity {_intensity}");
        response = $"Done! Every {onlyWavesMessage} will give the effect {_effect} for {_duration} seconds with intensity {_intensity}";
        return true;
    }
}
