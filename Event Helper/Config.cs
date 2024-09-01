using Exiled.API.Interfaces;
using System.ComponentModel;

namespace Event_Helper {
    public class Config : IConfig {
        [Description("Is this plugin enabled?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Is debug mode enabled?")]
        public bool Debug { get; set; } = false;
    }
}
