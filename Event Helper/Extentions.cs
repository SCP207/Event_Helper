using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Event_Helper {
    public static class Extensions {
        public static string FormatArguments(this ArraySegment<string> sentence, int index) {
            StringBuilder sb = new();
            foreach (string word in sentence.Segment(index)) {
                sb.Append(word);
                sb.Append(" ");
            }
            string msg = sb.ToString();
            return msg;
        }

        public static string LogPlayers(this IEnumerable<Player> players) => string.Join("\n - ", players.Select(x => $"{x.Nickname}({x.Id})"));
    }
}
