using Smod2;
using Smod2.Attributes;
using Smod2.Config;
using Smod2.Events;

namespace NoNickAds
{
    [PluginDetails(
		author = "dark7",
		name = "NoNickAds",
		description = "No more any ads in players's nicknames",
		id = "dark7.noads",
		configPrefix = "nna",
		langFile = "nonickads",
		version = "1.0",
		SmodMajor = 3,
		SmodMinor = 5,
		SmodRevision = 0
		)]

	public class NoNickAds : Plugin
	{
		public override void OnDisable()
		{
			this.Info(this.Details.name + " was disabled");
		}

		public override void OnEnable()
		{
			this.Info(this.Details.name + " has loaded");
		}
		
        public override void Register()
		{
            AddEventHandlers(new EventHandlers(this), Priority.Normal);
        }

        [ConfigOption(true, false)]
        internal bool Disabled = false;

        [ConfigOption(true, false)]
        internal string[] BlacklistedWords = new string[0];

        [ConfigOption(true, false)]
        internal string[] Whitelist = new string[0];

        [ConfigOption(true, false)]
        internal string Text = string.Empty;

        [ConfigOption(true, false)]
        internal int Duration = 2;

        [ConfigOption(true, false)]
        internal int Mode = 1;

        [ConfigOption(true, false)]
        internal bool SelectiveDeletion = true;
    }
}
