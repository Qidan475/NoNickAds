using EXILED;
using System;

namespace NoNickAds
{
	public class Plugin : EXILED.Plugin
	{
		public EventHandlers EventHandlers;

		public override string getName { get; } = "NoNickAds";

		public const string pluginVersion = "1.2";

		public override void OnEnable()
		{
			try
			{
				Configs.disabled = Plugin.Config.GetBool("nna_disable", false);
				if (Configs.disabled)
				{
					Log.Info("NoNickAds is disabled by config setting");
					return;
				}
				Log.Debug("Initializing event handlers..");
				EventHandlers = new EventHandlers(this);
				Events.PlayerJoinEvent += EventHandlers.OnPlayerJoin;
				Events.WaitingForPlayersEvent += EventHandlers.WaitingForPlayers;
				Log.Info("NoNickAds plugin loaded");
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
	}
}