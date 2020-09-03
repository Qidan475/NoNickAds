using Exiled;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;

namespace NoNickAds
{
    public class Configs : IConfig
    {
        internal Regex[] adsRegices = new Regex[0];
        internal Regex[] bannedWordsRegices = new Regex[0];
        internal NormalizationForm currentNormalizationForm = NormalizationForm.FormC;

        [Description("EN: Indicates whether the plugin is enabled or not" +
               "\n  # RU: Включает/выключает плагин")]
        public bool IsEnabled { get; set; } = true;

        [Description("EN: UserIds of players that will be ignored by plugin" +
               "\n  # RU: UserIds игроков, которые будут игнорироваться плагином")]
        public List<string> WhitelistedPlayers { get; set; } = new List<string>();


        [Description("EN: List of ads that will be replaced by ReplacementText. Case insensitive" +
               "\n  # RU: Список рекламы, которая будет заменяться на ReplacementText. Регистр не учитывается")]
        public string[] Ads { get; set; } = new string[0];

        [Description("EN: Enables/disables an algorithm that replaces links in the nick with ReplacementText" +
               "\n  # RU: Включает/выключает алгоритм, который заменяет ссылки в нике на ReplacementText")]
        public bool UseSmartLinkReplacer { get; set; } = true;

        [Description("EN: The text that will replace the ads & links" +
               "\n  # RU: Текст, на который будет заменяться реклама и ссылки")]
        public string ReplacementText { get; set; } = string.Empty;

        [Description("EN: If it is false, an algorithm will be used that tries to cut out the word containing the ad completely" +
               "\n  # RU: Если равно false, то будет использован алгоритм, старающийся вырезать слово, содержащее рекламу, полностью")]
        public bool LiteReplacing { get; set; } = true;



        [Description("EN: If player's nick contains a banned word, then the player will be banned. Case insensitive" +
               "\n  # RU: Если ник игрока содержит запрещённое слово, то игрок будет забанен. Регистр не учитывается")]
        public string[] BannedWords { get; set; } = new string[0];

        [Description("EN: The ban duration in case of a banned word in the nick" +
               "\n  # RU: Длительность бана в случае наличия запрещённого слова в нике")]
        public int BanDurationInMinutes { get; set; } = 4;



        [Description("unicode.org/reports/tr15/")]
        public bool UseUnicodeNormalization { get; set; } = false;

        [Description("FormC, FormD, FormKC, FormKD")]
        public string UnicodeNormalization { get; set; } = NormalizationForm.FormC.ToString();


        public string KickMsgWhenNickIsEmpty { get; set; } = "Looks like your nickname is all about advertising. Change it and re-join to the server.";

        [Description("EN: Variables: %words%" +
               "\n  # RU: Переменные: %words%")]
        public string BanMsgWhenNickContainsBannedWord { get; set; } = "Your nickname contains bad word(s): %words%";


        [Description("EN: The plugin will provide information about what it does." +
               "\n  # RU: Плагин будет предоставлять информацию по поводу того, что он делает")]
        public bool DebugMode { get; set; } = false;

        [Description("EN: Don't touch it. For big brain bois only. Variables: %%word%%. https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference/" +
               "\n  # RU: Лучше не трогай. Переменные: %%word%%. https://docs.microsoft.com/ru-ru/dotnet/standard/base-types/regular-expression-language-quick-reference")]
        public string AdsCustomRegexPattern { get; set; } = string.Empty;

        [Description("EN: Don't touch it. For big brain bois only. Variables: %%word%%. https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference/" +
               "\n  # RU: Лучше не трогай. Переменные: %%word%%. https://docs.microsoft.com/ru-ru/dotnet/standard/base-types/regular-expression-language-quick-reference")]
        public string BannedWordsCustomRegexPattern { get; set; } = string.Empty;

        internal static void PrepareThings()
        {
            if (Plugin.Instance.Config.IsEnabled)
            {
                string[] ads = Plugin.Instance.Config.Ads;
                string[] bannedWords = Plugin.Instance.Config.BannedWords;
                Plugin.Instance.Config.adsRegices = new Regex[ads.Length];
                Plugin.Instance.Config.bannedWordsRegices = new Regex[bannedWords.Length];
                if (Plugin.Instance.Config.UseUnicodeNormalization)
                {
                    if (Enum.TryParse(Plugin.Instance.Config.UnicodeNormalization, out NormalizationForm form))
                    {
                        Plugin.Instance.Config.currentNormalizationForm = form;
                    }
                    else
                    {
                        Log.Warn($"Invalid config \"unicode_normalization\", using default FormC...");
                        Plugin.Instance.Config.currentNormalizationForm = NormalizationForm.FormC;
                    }
                }
                int successfulAdsRegices = 0;
                int successfulBannedWordsRegices = 0;
                string pattern = string.Empty;
                try
                {
                    if (!string.IsNullOrWhiteSpace(Plugin.Instance.Config.AdsCustomRegexPattern))
                    {
                        for (int i = 0; i < ads.Length; i++)
                        {
                            string word = Regex.Escape(ads[i]);
                            if (!pattern.Contains("%%word%%"))
                            {
                                Log.Warn("Probably broken custom regex pattern");
                            }
                            pattern = Plugin.Instance.Config.AdsCustomRegexPattern.Replace("%%word%%", word);
                            Plugin.Instance.Config.adsRegices[i] = new Regex(pattern, RegexOptions.IgnoreCase);
                            successfulAdsRegices++;
                        }
                    }
                    else if (Plugin.Instance.Config.LiteReplacing)
                    {
                        for (int i = 0; i < ads.Length; i++)
                        {
                            string word = Regex.Escape(ads[i]);
                            pattern = word;
                            Plugin.Instance.Config.adsRegices[i] = new Regex(pattern, RegexOptions.IgnoreCase);
                            successfulAdsRegices++;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ads.Length; i++)
                        {
                            string word = Regex.Escape(ads[i]);
                            pattern = $@"\s*\W*{word}\w*\W*\s*";
                            Plugin.Instance.Config.adsRegices[i] = new Regex(pattern, RegexOptions.IgnoreCase);
                            successfulAdsRegices++;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(Plugin.Instance.Config.BannedWordsCustomRegexPattern))
                    {
                        for (int i = 0; i < bannedWords.Length; i++)
                        {
                            string word = Regex.Escape(bannedWords[i]);
                            if (!pattern.Contains("%%word%%"))
                            {
                                Log.Warn("Probably broken custom regex pattern");
                            }
                            pattern = Plugin.Instance.Config.BannedWordsCustomRegexPattern.Replace("%%word%%", word);
                            Plugin.Instance.Config.bannedWordsRegices[i] = new Regex(pattern, RegexOptions.IgnoreCase);
                            successfulBannedWordsRegices++;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < bannedWords.Length; i++)
                        {
                            string word = Regex.Escape(bannedWords[i]);
                            pattern = word;
                            Plugin.Instance.Config.bannedWordsRegices[i] = new Regex(pattern, RegexOptions.IgnoreCase);
                            successfulBannedWordsRegices++;
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"Something's wrong. Ping {Plugin.Instance.Author} in Exiled's #plugin-discussions and send this:" +
                              $"{Environment.NewLine}=====START=====" +
                              $"{Environment.NewLine}Error: ({e.InnerException}) {e.Message}" +
                              $"{Environment.NewLine}Stack Trace: {e.StackTrace}" +
                              $"{Environment.NewLine}Source: {e.Source}" +
                              $"{Environment.NewLine}Method: {e.TargetSite}" +
                              $"{Environment.NewLine}+++++" +
                              $"{Environment.NewLine}Extra details" +
                              $"{Environment.NewLine}Plugin version: {Plugin.Instance.Version.Major}.{Plugin.Instance.Version.Minor}.{Plugin.Instance.Version.Build}" +
                              $"{Environment.NewLine}ads_custom_regex: \"{Plugin.Instance.Config.AdsCustomRegexPattern}\"" +
                              $"{Environment.NewLine}ads: \"{string.Join(",", ads)}\"" +
                              $"{Environment.NewLine}ads lenght: {ads.Length}" +
                              $"{Environment.NewLine}Successful ads regices: {successfulAdsRegices}" +
                              $"{Environment.NewLine}bannedwords_custom_regex: \"{Plugin.Instance.Config.BannedWordsCustomRegexPattern}\"" +
                              $"{Environment.NewLine}banned words: \"{string.Join(",", bannedWords)}\"" +
                              $"{Environment.NewLine}banned words lenght: \"{bannedWords.Length}\"" +
                              $"{Environment.NewLine}Successful banned words regices: {successfulBannedWordsRegices}" +
                              $"{Environment.NewLine}Regex pattern: \"{pattern}\"" +
                              $"{Environment.NewLine}=====END=====");
                }
            }
        }
    }
}
