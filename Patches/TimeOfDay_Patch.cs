using HarmonyLib;

namespace ExtraLethalCompany.Patches
{
    internal static class TimeOfDay_Patch
    {
        [HarmonyPatch(typeof(TimeOfDay), "SetBuyingRateForDay"), HarmonyPrefix]
        private static bool SetBuyingRateForDayPrefix(TimeOfDay __instance)
        {
            __instance.daysUntilDeadline = (int)(__instance.timeUntilDeadline / __instance.totalTime);
            StartOfRound.Instance.companyBuyingRate = 1.15f - (__instance.quotaVariables.deadlineDaysAmount - __instance.daysUntilDeadline) * (0.54f / __instance.quotaVariables.deadlineDaysAmount);
            
            return false;
        }

        [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.SetShipLeaveEarlyServerRpc)), HarmonyPostfix]
        private static void SetShipLeaveEarlyServerRpcPostfix(TimeOfDay __instance)
        {
            int num = StartOfRound.Instance.connectedPlayersAmount + 1 - StartOfRound.Instance.livingPlayers;
            if (__instance.votesForShipToLeaveEarly >= num)
            {
                __instance.SetShipLeaveEarlyClientRpc(__instance.normalizedTimeOfDay + 0.01f, __instance.votesForShipToLeaveEarly);
                return;
            }
        }
    }
}