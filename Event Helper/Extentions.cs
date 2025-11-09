using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;

namespace Event_Helper;

public static class Extensions {
    public static string Log(this IEnumerable<Player> players) =>
        string.Join("\n - ", players.Select(p => $"{p.Nickname}({p.Id})"));
    public static string Log(this IEnumerable<ItemType> items) =>
        string.Join("\n - ", items.Select(i => $"{i} ({(int)i})"));
}
