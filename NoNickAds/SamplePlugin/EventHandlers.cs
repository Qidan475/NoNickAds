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
        public void OnPlayerJoin(JoinedEventArgs ev)
        {
            if (!Plugin.Instance.Config.IsEnabled)
            {
                Log.Debug("Plugin disabled, skipping...", Plugin.Instance.Config.DebugMode);
                return;
            }
            if (ev.Player == null)
            {
                Log.Debug("Player is null, skipping...", Plugin.Instance.Config.DebugMode);
                return;
            }
            string nick = ev.Player.Nickname;
            if (Plugin.Instance.Config.WhitelistedPlayers.Contains(ev.Player.UserId))
            {
                Log.Debug($"Player \"{nick}\" is in whitelist, skipping...", Plugin.Instance.Config.DebugMode);
                return;
            }
            if (ev.Player.ReferenceHub.serverRoles.RaEverywhere)
            {
                Log.Debug($"Player \"{nick}\" is a global moderator, skipping...", Plugin.Instance.Config.DebugMode);
                return;
            }
            Log.Debug($"Nick is {nick}", Plugin.Instance.Config.DebugMode);
            string newNick = nick;
            if (Plugin.Instance.Config.UseUnicodeNormalization)
            {
                Log.Debug($"Normalizing the nick with {Plugin.Instance.Config.currentNormalizationForm}...", Plugin.Instance.Config.DebugMode);
                newNick = newNick.Normalize(Plugin.Instance.Config.currentNormalizationForm);
            }
            Log.Debug("Looking for banned words in the nick...", Plugin.Instance.Config.DebugMode);
            if (Misc.FindBannedWords(newNick, out List<string> badWords))
            {
                if (Plugin.Instance.Config.DebugMode)
                {
                    foreach (var word in badWords)
                    {
                        Log.Debug($"Found banned word \"{word}\"", true);
                    }
                }
                string words = string.Join(", ", badWords);
                if (ev.Player != null)
                {
                    Log.Info($"Player {ev.Player.Nickname} has been banned for {Plugin.Instance.Config.BanDurationInMinutes} minutes because his nick contains banned word(s): {words}");
                    ev.Player.Ban(Plugin.Instance.Config.BanDurationInMinutes, (Plugin.Instance.Config.BanDurationInMinutes <= 0 ? "[KICK BY SERVER PLUGIN] " : "[BAN BY SERVER PLUGIN] ") + Plugin.Instance.Config.BanMsgWhenNickContainsBannedWord.Replace("%words%", words));
                }
                return;
            }
            Log.Debug("Looking for ads in the nick...", Plugin.Instance.Config.DebugMode);
            if (Misc.FindAds(ref newNick, out List<string> ads))
            {
                if (Plugin.Instance.Config.DebugMode)
                {
                    foreach (var ad in ads)
                    {
                        Log.Debug($"Found ad \"{ad}\", replacing with \"{Plugin.Instance.Config.ReplacementText}\"...", true);
                    }
                }
            }
            Log.Debug("Looking for links in the nick...", Plugin.Instance.Config.DebugMode);
            if (Misc.FindLinks(ref newNick, out List<string> links))
            {
                if (Plugin.Instance.Config.DebugMode)
                {
                    foreach (var link in links)
                    {
                        Log.Debug($"Found link \"{link}\", replacing with \"{Plugin.Instance.Config.ReplacementText}\"...", true);
                    }
                }
            }
            if (nick == newNick || (Plugin.Instance.Config.UseUnicodeNormalization && nick.Normalize(Plugin.Instance.Config.currentNormalizationForm) == newNick))
            {
                Log.Debug($"Nick hasn't changed", Plugin.Instance.Config.DebugMode);
            }
            else
            {
                Misc.SetNick(ev.Player, newNick);
            }
        }

        public void WaitingForPlayers()
        {
            Configs.PrepareThings();
        }
    }
}