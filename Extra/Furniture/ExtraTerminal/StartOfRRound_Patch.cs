using HarmonyLib;

namespace ExtraLethalCompany.Extra.Furniture.ExtraTerminal
{
    internal static class StartOfRRound_Patch
    {
        [HarmonyPatch(typeof(StartOfRound), "SetMapScreenInfoToCurrentLevel"), HarmonyPostfix]
        private static void SetMapScreenInfoToCurrentLevelPrefix(StartOfRound __instance)
        {
            __instance.screenLevelDescription.text = string.Concat(
            [
                "Orbiting: ",
                __instance.currentLevel.PlanetName,
                "\n",
                __instance.currentLevel.LevelDescription,
                "\n",
                "{Sensor Err}"
            ]);
        }
    }
}
