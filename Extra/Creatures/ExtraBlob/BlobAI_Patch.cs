using HarmonyLib;
using UnityEngine;

namespace ExtraLethalCompany.Extra.Creatures
{
    internal static class BlobAI_Patch
    {
        [HarmonyPatch(typeof(BlobAI), nameof(BlobAI.Start)), HarmonyPrefix]
        private static void StartPrefix(BlobAI __instance)
        {
            GameObject go = __instance.gameObject;
            
            if (go.GetComponent<ExtraBlob>() == null)
                go.AddComponent<ExtraBlob>();
        }

        [HarmonyPatch(typeof(BlobAI), nameof(BlobAI.SlimeKillPlayerEffectServerRpc)), HarmonyPostfix]
        private static void SlimeKillPlayerEffectServerRpc(BlobAI __instance, int playerKilled)
        {
            if (!__instance.IsHost)
                return;

            ExtraBlob extraBlob = __instance.gameObject.GetComponent<ExtraBlob>();
            extraBlob?.AskEatPlayerBody(playerKilled);
        }
    }
}
