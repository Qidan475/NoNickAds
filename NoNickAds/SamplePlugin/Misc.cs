using Exiled;
using Exiled.API;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NoNickAds
{
    public static class Misc
    {
        private static readonly Regex regexSmartLinkReplacer = new Regex(@"\s*((http://)|(https://))?\s*(www(\.|,))?\s*\W*[*\-\w*]*(((\W+(com|net|xyz|pw))+)|((\.|,)(website|online|travel|moscow|москва|studio|онлайн|design|money|press|media|store|space|parts|cloud|house|info|name|site|zone|team|show|shop|host|tech|club|сайт|plus|mobi|live|life|luxe|work|blog|best|guru|fail|farm|com|net|org|run|biz|xyz|рус|top|ltd|pro|fun|moe|art|bar|vip|орг|xxx|bet|wtf|de|uk|cn|nl|eu|ru|pw|me|su|io|ua|by|tk|рф|kz|co|fm|gg|tv|be|ly|ai|cc|pl)))(/[*\w\-%&=\?\+\._*]*)*/*\W*\s*", RegexOptions.IgnoreCase);

        internal static void SetNick(Player player, string newNick)
        {
            try
            {
                if (player != null)
                {
                    if (string.IsNullOrWhiteSpace(newNick))
                    {
                        Log.Info($"Player {player.Nickname} has been kicked because his nick is empty without ad. Something is wrong? Enable debug_mode");
                        player.Disconnect("[KICK BY SERVER PLUGIN] " + Plugin.Instance.Config.KickMsgWhenNickIsEmpty);
                    }
                    else
                    {
                        Log.Debug($"Setting up a new nickname: \"{newNick}\"...", Plugin.Instance.Config.DebugMode);
                        player.DisplayNickname = newNick;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Something's wrong. Ping {Plugin.Instance.Author} in Exiled's #plugin-discussions and send this:" +
                          $"{Environment.NewLine}Method: {e.TargetSite}" +
                          $"{Environment.NewLine}Error: ({e.InnerException}) {e.Message}" +
                          $"{Environment.NewLine}Stack trace: {e.StackTrace}");
            }
        }

        internal static bool FindBannedWords(string nick, out List<string> badWords)
        {
            bool issueBan = false;
            badWords = new List<string>();
            for (int i = 0; i < Plugin.Instance.Config.BannedWords.Length; i++)
            {
                Regex regex = Plugin.Instance.Config.bannedWordsRegices[i];
                foreach (Match match in regex.Matches(nick))
                {
                    issueBan = true;
                    string word = match.Value;
                    badWords.Add(word);
                }
            }
            return issueBan;
        }

        internal static bool FindAds(ref string nick, out List<string> ads)
        {
            bool foundSomething = false;
            ads = new List<string>();
            for (int i = 0; i < Plugin.Instance.Config.Ads.Length; i++)
            {
                Regex regex = Plugin.Instance.Config.adsRegices[i];
                foreach (Match match in regex.Matches(nick))
                {
                    foundSomething = true;
                    string ad = match.Value;
                    ads.Add(ad);
                }
                nick = regex.Replace(nick, Plugin.Instance.Config.ReplacementText);
            }
            return foundSomething;
        }

        public static bool FindLinks(ref string nick, out List<string> links)
        {
            bool foundSomething = false;
            links = new List<string>();
            if (Plugin.Instance.Config.UseSmartLinkReplacer)
            {
                foreach (Match match in regexSmartLinkReplacer.Matches(nick))
                {
                    foundSomething = true;
                    string link = match.Value;
                    links.Add(link);
                }
                nick = regexSmartLinkReplacer.Replace(nick, Plugin.Instance.Config.ReplacementText);
            }
            return foundSomething;
        }
    }
}
