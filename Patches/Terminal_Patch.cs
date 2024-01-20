using HarmonyLib;
using System.Text.RegularExpressions;

namespace ExtraLethalCompany.Patches
{
    internal static class Terminal_Patch
    {
        [HarmonyPatch(typeof(Terminal), "TextPostProcess"), HarmonyPrefix]
        private static bool TextPostProcessPrefix(Terminal __instance, ref string __result, ref string modifiedDisplayText, TerminalNode node)
        {
            int num = modifiedDisplayText.Split("[planetTime]").Length - 1;
            if (num > 0)
            {
                Regex regex = new(Regex.Escape("[planetTime]"));
                int num2 = 0;
                while (num2 < num && num2 < __instance.moonsCatalogueList.Length)
                {
                    modifiedDisplayText = regex.Replace(modifiedDisplayText, "{SENSOR ERR}", 1);
                    num2++;
                }
            }

            if (node?.displayPlanetInfo != -1)
                modifiedDisplayText = modifiedDisplayText?.Replace("[currentPlanetTime]", "{SENSOR ERR}");

            __result = modifiedDisplayText;
            return true;
        }
    }
}