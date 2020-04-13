using EXILED;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NoNickAds
{
	internal static class Configs
    {
        internal static bool disabled = false;
        internal static int blacklistCount = 0;
        internal static List<string> whitelist = new List<string>();
        internal static byte mode = 1;
        internal static string text = string.Empty;
        internal static int banDuration = 2;
        internal static bool selectiveReplacing = true;
        internal static bool useUnicodeNormalization = true;
        internal static bool useSmartSiteReplacer = true;
        internal static Regex[] regices = new Regex[0];
        internal static NormalizationForm currentNormalizationForm = NormalizationForm.FormC;

        internal static void Reload()
        {
			disabled = EXILED.Plugin.Config.GetBool("nna_disable", false);
			if (!disabled)
			{
				int successfulConfigs = 0;
				string[] blacklist = new string[0];
				string unicodeNormalization = string.Empty;
				string customRegexPattern = string.Empty;
				try
				{
					blacklist = EXILED.Plugin.Config.GetString("nna_blacklisted_words", "").Split(new char[] { ',' }, StringSplitOptions.None);
					successfulConfigs++;
					blacklistCount = blacklist.Length;
					successfulConfigs++;
					whitelist = new List<string>(EXILED.Plugin.Config.GetString("nna_whitelisted_players", "").ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
					successfulConfigs++;
					mode = EXILED.Plugin.Config.GetByte("nna_mode", 1);
					successfulConfigs++;
					text = EXILED.Plugin.Config.GetString("nna_text", string.Empty);
					successfulConfigs++;
					banDuration = EXILED.Plugin.Config.GetInt("nna_ban_duration", 2);
					successfulConfigs++; // successfulConfigs = 6
					selectiveReplacing = EXILED.Plugin.Config.GetBool("nna_selective_replacing", true);
					successfulConfigs++;
					useUnicodeNormalization = EXILED.Plugin.Config.GetBool("nna_use_unicode_normalization", true);
					successfulConfigs++;
					unicodeNormalization = EXILED.Plugin.Config.GetString("nna_unicode_normalization", "FormC").Replace(" ", string.Empty);
					successfulConfigs++;
					customRegexPattern = EXILED.Plugin.Config.GetString("nna_custom_regex", "");
					successfulConfigs++;
					useSmartSiteReplacer = EXILED.Plugin.Config.GetBool("nna_use_smart_site_replacer", true);
					successfulConfigs++;
					regices = new Regex[blacklistCount];
					successfulConfigs++; // successfulConfigs = 12
				}
				catch (Exception e)
				{
					Log.Error($"Something wrong. Ping Dark7eamplar#2683 in #plugin-discussions and send this:" +
							  $"{Environment.NewLine}=====START=====" +
							  $"{Environment.NewLine}Identifier: Plugin::ConfigReload()::Configs" +
							  $"{Environment.NewLine}Error: ({e.InnerException}) {e.Message}" +
							  $"{Environment.NewLine}Stack Trace: {e.StackTrace}" +
							  $"{Environment.NewLine}Source: {e.Source}" +
							  $"{Environment.NewLine}Method: {e.TargetSite}" +
							  $"{Environment.NewLine}+++++" +
							  $"{Environment.NewLine}Extra details" +
							  $"{Environment.NewLine}Plugin version: {Plugin.pluginVersion}" +
							  $"{Environment.NewLine}EXILED version: {EventPlugin.Version.Major}.{EventPlugin.Version.Minor}.{EventPlugin.Version.Patch}" +
							  $"{Environment.NewLine}successfulConfigs: {successfulConfigs}" +
							  $"{Environment.NewLine}=====END=====");
				}
				if (Enum.TryParse(unicodeNormalization, out NormalizationForm form))
				{
					currentNormalizationForm = form;
				}
				else
				{
					Log.Warn($"Problem with config \"nna_unicode_normalization\": can't parse, using default FormC...");
					currentNormalizationForm = NormalizationForm.FormC;
				}
				if (mode > 2 || mode < 1)
				{
					Log.Warn($"Unknown value \"{mode}\" in \"nna_mode\", using default mode 1...");
					mode = 1;
				}
				int successfulRegices = 0;
				string pattern = string.Empty;
				try
				{
					for (int i = 0; i < blacklistCount; i++)
					{
						string word = Regex.Escape(blacklist[i]);
						if (!string.IsNullOrWhiteSpace(customRegexPattern))
						{
							pattern = customRegexPattern.Replace("{word}", word);
						}
						else if (selectiveReplacing || mode == 2)
						{
							pattern = word;
						}
						else
						{
							pattern = $@"(.)?{word}(\w*)";
						}
						regices[i] = new Regex(pattern, RegexOptions.IgnoreCase);
						successfulRegices++;
					}
				}
				catch (Exception e)
				{
					Log.Error($"Something wrong. Ping Dark7eamplar#2683 in #plugin-discussions and send this:" +
							  $"{Environment.NewLine}=====START=====" +
							  $"{Environment.NewLine}Identifier: Plugin::ConfigReload()::Regex loop" +
							  $"{Environment.NewLine}Error: ({e.InnerException}) {e.Message}" +
							  $"{Environment.NewLine}Stack Trace: {e.StackTrace}" +
							  $"{Environment.NewLine}Source: {e.Source}" +
							  $"{Environment.NewLine}Method: {e.TargetSite}" +
							  $"{Environment.NewLine}+++++" +
							  $"{Environment.NewLine}Extra details" +
							  $"{Environment.NewLine}Plugin version: {Plugin.pluginVersion}" +
							  $"{Environment.NewLine}EXILED version: {EventPlugin.Version.Major}.{EventPlugin.Version.Minor}.{EventPlugin.Version.Patch}" +
							  $"{Environment.NewLine}nna_custom_regex: \"{customRegexPattern}\"" +
							  $"{Environment.NewLine}nna_blacklisted_words: \"{string.Join(",", blacklist)}\"" +
							  $"{Environment.NewLine}Blacklisted words lenght: {blacklistCount}" +
							  $"{Environment.NewLine}Successful regices: {successfulRegices}" +
							  $"{Environment.NewLine}Pattern: \"{pattern}\"" +
							  $"{Environment.NewLine}=====END=====");
				}
			}
		}
    }
}
