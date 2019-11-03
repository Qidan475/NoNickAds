using Smod2.EventHandlers;
using Smod2.Events;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NoNickAds
{
    internal class EventHandlers : IEventHandlerNicknameSet, IEventHandlerWaitingForPlayers
    {
        private readonly NoNickAds plugin;

        List<string> blacklist;

        List<string> whitelist;

        int mode;

        public EventHandlers(NoNickAds plugin)
        {
            this.plugin = plugin;
        }

        public void OnNicknameSet(PlayerNicknameSetEvent ev)
        {
            if (!plugin.Disabled && whitelist != null && !whitelist.Contains(ev.Player.SteamId) && !ev.Player.GetAuthToken().Contains("Bypass bans: YES"))
            {
                foreach (string word in blacklist)
                {
                    if (mode == 1)
                    {
                        Regex regex = new Regex($"{word}(\\w*)", RegexOptions.IgnoreCase);
                        MatchCollection matches = regex.Matches(ev.Nickname);
                        if (matches.Count > 0)
                        {
                            foreach (Match match in matches)
                            {
                                if (plugin.SelectiveDeletion)
                                {
                                    ev.Nickname = ev.Nickname.Remove(match.Index, word.Length);
                                }
                                else
                                {
                                    ev.Nickname = ev.Nickname.Replace(match.ToString(), string.Empty);
                                }
                            }
                        }
                    }
                    else if (ev.Nickname.Contains(word))
                    {
                        ev.Player.Ban(plugin.Duration, (plugin.Duration <= 0 ? "[Kick by server plugin]" : "[Ban by server plugin]") + plugin.Text.Replace("%word%", word));
                    }
                }
                if (string.IsNullOrWhiteSpace(ev.Nickname))
                {
                    ev.Player.Disconnect("[Kick by server plugin] Looks like your nickname is all about advertising. Change it and then re-join to the server.");
                }
            }
        }

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            ReloadConfigs();
        }

        public void ReloadConfigs()
        {
            this.blacklist = new List<string>(plugin.BlacklistedWords);
            this.whitelist = new List<string>(plugin.Whitelist);
            this.mode = plugin.Mode;
            if (mode > 2) { mode = 2; }
            if (mode < 1) { mode = 1; }
        }
    }
}
