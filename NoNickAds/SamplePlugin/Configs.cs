using System.Collections.Generic;

namespace NoNickAds
{
    internal static class Configs
    {
        internal static bool disabled = false;
        internal static int blacklistCount;
        internal static List<string> whitelist = new List<string>();
        internal static byte mode = 1;
        internal static string text = string.Empty;
        internal static int banDuration = 2;
        internal static bool selectiveReplacing = true;
        internal static bool useUnicodeNormalization = true;
        internal static bool useSmartSiteReplacer = true;
        internal static System.Text.RegularExpressions.Regex[] regices = new System.Text.RegularExpressions.Regex[0];
        internal static System.Text.NormalizationForm currentNormalizationForm = System.Text.NormalizationForm.FormC;
    }
}
