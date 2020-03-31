using EXILED;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NoNickAds
{
	public class Plugin : EXILED.Plugin
	{
		public EventHandlers EventHandlers;

		public override string getName { get; } = "NoNickAds";

		public const string pluginVersion = "1.1";

		public override void OnEnable()
		{
			try
			{
				Configs.disabled = Plugin.Config.GetBool("nna_disable", false);
				if (Configs.disabled)
				{
					return;
				}
				Log.Debug("Initializing event handlers..");
				EventHandlers = new EventHandlers(this);
				Events.PlayerJoinEvent += EventHandlers.OnPlayerJoin;
				Events.WaitingForPlayersEvent += EventHandlers.WaitingForPlayers;
				Log.Info($"NoNickAds plugin loaded. c:");
			}
			catch (Exception e)
			{
				Log.Error($"There was an error loading the plugin: {e}");
			}
		}

		public override void OnDisable()
		{
			Events.PlayerJoinEvent -= EventHandlers.OnPlayerJoin;
			Events.WaitingForPlayersEvent -= EventHandlers.WaitingForPlayers;
			EventHandlers = null;
		}

		public override void OnReload()
		{
		}

		internal void ConfigReload()
		{
			Configs.disabled = Plugin.Config.GetBool("nna_disable", false);
			if (!Configs.disabled)
			{
				int successfulConfigs = 0;
				string[] blacklist = new string[0];
				string unicodeNormalization = string.Empty;
				string customRegexPattern = string.Empty;
				try
				{
					blacklist = Plugin.Config.GetString("nna_blacklisted_words", "").Split(new char[] { ',' }, StringSplitOptions.None);
					successfulConfigs++;
					Configs.blacklistCount = blacklist.Length;
					successfulConfigs++;
					Configs.whitelist = new List<string>(Plugin.Config.GetString("nna_whitelisted_players", "").ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
					successfulConfigs++;
					Configs.mode = Plugin.Config.GetByte("nna_mode", 1);
					successfulConfigs++;
					Configs.text = Plugin.Config.GetString("nna_text", string.Empty);
					successfulConfigs++;
					Configs.banDuration = Plugin.Config.GetInt("nna_ban_duration", 2);
					successfulConfigs++; // successfulConfigs = 6
					Configs.selectiveReplacing = Plugin.Config.GetBool("nna_selective_replacing", true);
					successfulConfigs++;
					Configs.useUnicodeNormalization = Plugin.Config.GetBool("nna_use_unicode_normalization", true);
					successfulConfigs++;
					unicodeNormalization = Plugin.Config.GetString("nna_unicode_normalization", "FormC").Replace(" ", string.Empty);
					successfulConfigs++;
					customRegexPattern = Plugin.Config.GetString("nna_custom_regex", "");
					successfulConfigs++;
					Configs.useSmartSiteReplacer = Plugin.Config.GetBool("nna_use_smart_site_replacer", true);
					successfulConfigs++;
					Configs.regices = new Regex[Configs.blacklistCount];
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
							  $"{Environment.NewLine}Plugin version: {pluginVersion}" +
							  $"{Environment.NewLine}EXILED version: {EXILED.EventPlugin.Version.Major}.{EXILED.EventPlugin.Version.Minor}.{EXILED.EventPlugin.Version.Patch}" +
							  $"{Environment.NewLine}successfulConfigs: {successfulConfigs}" +
							  $"{Environment.NewLine}=====END=====");
				}
				if (Enum.TryParse(unicodeNormalization, out NormalizationForm form))
				{
					Configs.currentNormalizationForm = form;
				}
				else
				{
					Log.Warn($"Problem with config \"nna_unicode_normalization\": can't parse, using default FormC...");
					Configs.currentNormalizationForm = NormalizationForm.FormC;
				}
				if (Configs.mode > 2 || Configs.mode < 1)
				{
					Log.Warn($"Unknown value \"{Configs.mode}\" in \"nna_mode\", using default mode 1...");
					Configs.mode = 1;
				}
				int successfulRegices = 0;
				string pattern = string.Empty;
				try
				{
					for (int i = 0; i < Configs.blacklistCount; i++)
					{
						string word = Regex.Escape(blacklist[i]);
						if (!string.IsNullOrWhiteSpace(customRegexPattern))
						{
							pattern = customRegexPattern.Replace("{word}", word);
						}
						else if (Configs.selectiveReplacing || Configs.mode == 2)
						{
							pattern = word;
						}
						else
						{
							pattern = $@"(.)?{word}(\w*)";
						}
						Configs.regices[i] = new Regex(pattern);
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
							  $"{Environment.NewLine}Plugin version: {pluginVersion}" +
							  $"{Environment.NewLine}EXILED version: {EXILED.EventPlugin.Version.Major}.{EXILED.EventPlugin.Version.Minor}.{EXILED.EventPlugin.Version.Patch}" +
							  $"{Environment.NewLine}nna_custom_regex: \"{customRegexPattern}\"" +
							  $"{Environment.NewLine}nna_blacklisted_words: \"{string.Join(",", blacklist)}\"" +
							  $"{Environment.NewLine}Blacklisted words lenght: {Configs.blacklistCount}" +
							  $"{Environment.NewLine}Successful regices: {successfulRegices}" +
							  $"{Environment.NewLine}Pattern: \"{pattern}\"" +
							  $"{Environment.NewLine}=====END=====");
				}
			}
		}
	}
}