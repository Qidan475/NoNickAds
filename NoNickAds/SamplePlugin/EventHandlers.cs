using EXILED;
using EXILED.Extensions;
using System.Text.RegularExpressions;

namespace NoNickAds
{
    public class EventHandlers
	{
		public Plugin plugin;

        private readonly Regex regexSmartSiteReplacer = new Regex(@"((((\s*)\w*)\.)|(\s*)?.)?(\w*)(\s*)?\.(\s*)?(com|org|ru|net|info|biz|by|de|uk|nl|tk|su|cn|xyz|online|site|zone|money|run|top|team|πτ|ltd|media|show|pw|name|eu|me|store|shop|pro|space|website|fun|host|tech)(\s*)?(/*)?");

		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
            if (Configs.disabled || ev.Player.serverRoles.Staff || Configs.whitelist.Contains(ev.Player.GetUserId()))
            {
                return;
            }

            string nick = ev.Player.GetNickname();
            if (Configs.useUnicodeNormalization)
            {
                nick = nick.Normalize(Configs.currentNormalizationForm);
            }
            if (Configs.useSmartSiteReplacer)
            {
                if (Configs.mode == 1)
                {
                    nick = regexSmartSiteReplacer.Replace(nick, Configs.text);
                }
                else
                {
                    MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
                    if (matches.Count > 0)
                    {
                        string words = string.Empty;
                        foreach (Match match in matches)
                        {
                            words = $"{(string.IsNullOrEmpty(words) ? "" : ", ")}{match.Value}";
                        }
                        if (ev.Player != null)
                        {
                            ev.Player.BanPlayer(Configs.banDuration, (Configs.banDuration <= 0 ? "[KICK BY SERVER PLUGIN] " : "[BAN BY SERVER PLUGIN] ") + Configs.text.Replace("%words%", words));
                            return;
                        }
                    }
                }
            }
            for (int i = 0; i < Configs.blacklistCount; i++)
            {
                Regex regex = Configs.regices[i];
                if (Configs.mode == 1)
                {
                    nick = regex.Replace(nick, Configs.text);
                }
                else
                {
                    MatchCollection matches = regex.Matches(nick);
                    if (matches.Count > 0)
                    {
                        string words = string.Empty;
                        foreach (Match match in matches)
                        {
                            words = $"{(string.IsNullOrEmpty(words) ? "" : ", ")}{match.Value}";
                        }
                        if (ev.Player != null)
                        {
                            ev.Player.BanPlayer(Configs.banDuration, (Configs.banDuration <= 0 ? "[KICK BY SERVER PLUGIN] " : "[BAN BY SERVER PLUGIN] ") + Configs.text.Replace("%words%", words));
                            return;
                        }
                    }
                }
            }

            SetNick(ev.Player, nick);
        }

		public void WaitingForPlayers()
		{
			Configs.Reload();
		}

        private void SetNick(ReferenceHub player, string newNick)
        {
            if (player != null)
            {
                if (string.IsNullOrWhiteSpace(newNick))
                {
                    player.Disconnect("[KICK BY SERVER PLUGIN] Looks like your nickname is all about advertising. Change it and join to the server.");
                    return;
                }
                player.nicknameSync.Network_myNickSync = newNick;
            }
        }
	}
}