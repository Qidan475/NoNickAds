using Exiled;
using Exiled.API.Features;
using System;

namespace NoNickAds
{
	public class Plugin : Plugin<Configs>
	{
		public EventHandlers EventHandlers;

		public override string Author { get; } = "Dark7eamplar#2683";

		public override string Name { get; } = "NoNickAds";

		public override string Prefix { get; } = "NoNickAds";

		public override Version Version { get; } = new Version(2, 0, 0);

		public override Version RequiredExiledVersion { get; } = new Version(2, 0, 0);

		public override Exiled.API.Enums.PluginPriority Priority { get; } = Exiled.API.Enums.PluginPriority.Medium;

        internal static Plugin plugin;

		public override void OnEnabled()
		{
			try
			{
				plugin = this;
				if (!plugin.Config.IsEnabled)
				{
					Log.Info("NoNickAds is disabled by config setting");
					return;
				}
				EventHandlers = new EventHandlers(this);
				Exiled.Events.Handlers.Player.Joined += EventHandlers.OnPlayerJoin;
				Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.WaitingForPlayers;
				Log.Info("NoNickAds plugin loaded");
			}
			catch (Exception e)
			{
				Log.Error($"There was an error loading the plugin: {e}");
			}
		}

		public override void OnDisabled()
		{
			Exiled.Events.Handlers.Player.Joined -= EventHandlers.OnPlayerJoin;
			Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.WaitingForPlayers;
			EventHandlers = null;
		}

		public override void OnReloaded()
		{
		}
	}
}