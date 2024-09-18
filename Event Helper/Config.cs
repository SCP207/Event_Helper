using Exiled.API.Interfaces;
using System.ComponentModel;

namespace Event_Helper {
    public class Config : IConfig {
        [Description("Is this plugin enabled?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Is debug mode enabled?")]
        public bool Debug { get; set; } = false;

        [Description("Do Tesla Gates vaporize")]
        public bool TeslaVaporize { get; set; } = false;

        [Description("Can players with God Mode get detained")]
        public bool GodModePlayersGetDetained { get; set; } = false;

        [Description("Can players with Bypass get detained")]
        public bool BypassPlayersGetDetained { get; set; } = false;
    }
}
