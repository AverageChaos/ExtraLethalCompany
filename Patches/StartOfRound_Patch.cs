using HarmonyLib;
using UnityEngine;

namespace ExtraLethalCompany.Patches
{
    internal static class StartOfRound_Patch
    {
        [HarmonyPatch(typeof(StartOfRound), "PowerSurgeShip"), HarmonyPrefix]
        private static bool PowerSurgeShipPrefix(StartOfRound __instance)
        {
            __instance.mapScreen.SwitchScreenOn(false);
            if (!__instance.IsServer)
                return false;

            Object.FindObjectsOfType<TVScript>().Do(obj => obj.TurnOffTVServerRpc());

            __instance.shipRoomLights.SetShipLightsServerRpc(false);
            if (Random.Range(0, 10) == 0)
                TimeOfDay.Instance.SetShipLeaveEarlyClientRpc(TimeOfDay.Instance.normalizedTimeOfDay + 0.04167f, TimeOfDay.Instance.votesForShipToLeaveEarly);

            return false;
        }

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