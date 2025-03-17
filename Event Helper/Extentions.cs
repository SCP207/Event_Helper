using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Event_Helper {
    public static class Extensions {
        public static string Log(this IEnumerable<Player> players) => string.Join("\n - ", players.Select(x => $"{x.Nickname}({x.Id})"));
    }
}
