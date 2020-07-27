using Exiled;
using Exiled.API;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Enums;
using System.Text.RegularExpressions;
using Exiled.Events.EventArgs;
using System.Text;
using System.Collections.Generic;

namespace NoNickAds
{
    public class EventHandlers
    {
        public Plugin plugin;

        private readonly Regex regexSmartLinkReplacer = new Regex(@"\s*((http://)|(https://))?\s*(www(\.|,))?\s*\W*[*\-\w*]*(((\W+(com|net|xyz|pw))+)|((\.|,)(website|online|travel|moscow|москва|studio|онлайн|design|money|press|media|store|space|parts|cloud|house|info|name|site|zone|team|show|shop|host|tech|club|сайт|plus|mobi|live|life|luxe|work|blog|best|guru|fail|farm|com|net|org|run|biz|xyz|рус|top|ltd|pro|fun|moe|art|bar|vip|орг|xxx|bet|wtf|de|uk|cn|nl|eu|ru|pw|me|su|io|ua|by|tk|рф|kz|co|fm|gg|tv|be|ly|ai|cc|pl)))(/[*\w\-%&=\?\+\._*]*)*/*\W*\s*", RegexOptions.IgnoreCase);

        public EventHandlers(Plugin plugin) => this.plugin = plugin;

        public void OnPlayerJoin(JoinedEventArgs ev)
        {
            if (!plugin.Config.IsEnabled)
            {
                Log.Debug("Plugin disabled, skipping...", plugin.Config.DebugMode);
                return;
            }
            if (ev.Player == null)
            {
                Log.Debug("Player is null, skipping...", plugin.Config.DebugMode);
                return;
            }
            string nick = ev.Player.Nickname;
            if (plugin.Config.WhitelistedPlayers.Contains(ev.Player.UserId))
            {
                Log.Debug($"Player \"{nick}\" is in whitelist, skipping...", plugin.Config.DebugMode);
                return;
            }
            if (ev.Player.ReferenceHub.serverRoles.RaEverywhere)
            {
                Log.Debug($"Player \"{nick}\" is a global moderator, skipping...", plugin.Config.DebugMode);
                return;
            }
            Log.Debug($"Nick is {nick}", plugin.Config.DebugMode);
            string newNick = nick;
            if (plugin.Config.UseUnicodeNormalization)
            {
                Log.Debug($"Normalizing the nick with {plugin.Config.currentNormalizationForm}...", plugin.Config.DebugMode);
                newNick = newNick.Normalize(plugin.Config.currentNormalizationForm);
            }
            bool issueBan = false;
            HashSet<string> wordsArr = new HashSet<string>();
            Log.Debug("Looking for banned words in the nick...", plugin.Config.DebugMode);
            for (int i = 0; i < plugin.Config.BannedWords.Length; i++)
            {
                Regex regex = plugin.Config.bannedWordsRegices[i];
                if (regex.IsMatch(newNick))
                {
                    string word = regex.ToString();
                    issueBan = true;
                    Log.Debug($"Found banned word \"{word}\" [Regex: \"{regex.ToString()}\"]", plugin.Config.DebugMode);
                    wordsArr.Add(word);
                }
            }
            if (issueBan)
            {
                string words = string.Join(", ", wordsArr);
                if (ev.Player != null)
                {
                    Log.Info($"Player {ev.Player.Nickname} has been banned for {plugin.Config.BanDurationInMinutes} minutes because his nick contains banned word(s): {words}");
                    ev.Player.Ban(plugin.Config.BanDurationInMinutes, (plugin.Config.BanDurationInMinutes <= 0 ? "[KICK BY SERVER PLUGIN] " : "[BAN BY SERVER PLUGIN] ") + plugin.Config.BanMsgWhenNickContainsBannedWord.Replace("%words%", words));
                }
                return;
            }
            Log.Debug("Looking for ads in the nick...", plugin.Config.DebugMode);
            for (int i = 0; i < plugin.Config.Ads.Length; i++)
            {
                Regex regex = plugin.Config.adsRegices[i];
                if (plugin.Config.DebugMode)
                {
                    foreach (Match match in regex.Matches(newNick))
                    {
                        Log.Debug($"Found ad \"{match.Value}\", replacing with \"{plugin.Config.ReplacementText}\"... [Regex: \"{regex.ToString()}\"]", plugin.Config.DebugMode);
                    }
                }
                newNick = regex.Replace(newNick, plugin.Config.ReplacementText);
            }
            Log.Debug("Looking for links in the nick...", plugin.Config.DebugMode);
            if (plugin.Config.UseSmartLinkReplacer)
            {
                if (plugin.Config.DebugMode)
                {
                    foreach (Match match in regexSmartLinkReplacer.Matches(newNick))
                    {
                        Log.Debug($"Found link \"{match.Length}\", replacing with \"{plugin.Config.ReplacementText}\"...", plugin.Config.DebugMode);
                    }
                }
                newNick = regexSmartLinkReplacer.Replace(newNick, plugin.Config.ReplacementText);
            }
            if (nick == newNick || (plugin.Config.UseUnicodeNormalization && nick.Normalize(plugin.Config.currentNormalizationForm) == newNick))
            {
                Log.Debug($"Nick hasn't changed", plugin.Config.DebugMode);
            }
            else
            {
                SetNick(ev.Player, newNick);
            }
        }

        public void WaitingForPlayers()
        {
            Configs.PrepareThings();
        }

        private void SetNick(Player player, string newNick)
        {
            if (player != null)
            {
                if (string.IsNullOrWhiteSpace(newNick))
                {
                    Log.Info($"Player {player.Nickname} has been kicked because his nick is empty without ad. Something is wrong? Enable debug_mode");
                    player.Disconnect("[KICK BY SERVER PLUGIN] " + plugin.Config.KickMsgWhenNickIsEmpty);
                }
                else
                {
                    Log.Debug($"Setting up a new nickname: \"{newNick}\"...", plugin.Config.DebugMode);
                    player.Nickname = newNick;
                }
            }
        }
    }
}