using HarmonyLib;
using System;

namespace ExtraLethalCompany.Extra.HUD
{
    internal static class HUDManager_Patch
    {
        [HarmonyPatch(typeof(HUDManager), nameof(HUDManager.ApplyPenalty)), HarmonyPrefix]
        static bool ApplyPenaltyPrefix(HUDManager __instance, int playersDead, ref int bodiesInsured)
        {
            Terminal terminal = UnityEngine.Object.FindObjectOfType<Terminal>();
            float profitQuota = TimeOfDay.Instance.profitQuota;
            if (Disabler.UseExtraLethalPenalty)
                profitQuota *= Math.Max(1, terminal.groupCredits);

            int total = (int)Math.Min(profitQuota * ((playersDead - bodiesInsured) * 0.2f) + profitQuota * (bodiesInsured * 0.08f), int.MaxValue);
            int remainder = -Math.Min(Math.Max(terminal.groupCredits, 0) - total, 0);

            terminal.groupCredits -= total - remainder;
            TimeOfDay.Instance.profitQuota = (int)Math.Min((float)TimeOfDay.Instance.profitQuota + remainder, int.MaxValue);

            __instance.statsUIElements.penaltyAddition.text =
                $"{playersDead} casualties: -{20f * (playersDead - bodiesInsured)}%\n" +
                $"{bodiesInsured} injuries: -{8f * bodiesInsured}%\n" +
                "\nAll unpaid fines will be added to your quota.";

            __instance.statsUIElements.penaltyTotal.text =
                $"PAID: {total - remainder}" +
                "\n" +
                $"DUE: {remainder}";

            return false;
        }
    }
}
