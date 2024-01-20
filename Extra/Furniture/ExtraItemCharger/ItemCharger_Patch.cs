using ExtraLethalCompany.Helpers;
using HarmonyLib;
using LC_API.Data;
using LC_API.GameInterfaceAPI;
using LC_API.Networking;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace ExtraLethalCompany.Extra.Furniture.ExtraItemCharger
{
    internal static class ItemCharger_Patch
    {
        internal static readonly List<ItemCharger> ItemChargers = [];
        internal static readonly Dictionary<ulong, ExtraItemCharger> ExtraItemChargerBinds = [];

        [HarmonyPatch(typeof(ItemCharger), nameof(ItemCharger.OnNetworkSpawn)), HarmonyPostfix]
        static void OnNetworkSpawnPostfix(ItemCharger __instance)
        {
            if (ExtraItemChargerBinds.ContainsKey(__instance.NetworkObjectId))
                return;

            ItemChargers.Add(__instance);
            ExtraItemChargerBinds.Add(__instance.NetworkObjectId, new ExtraItemCharger(__instance.NetworkObjectId, false));
            GameState.WentIntoOrbit += ExtraItemChargerBinds[__instance.NetworkObjectId].ResetItemCharger;
            Network.RegisterMessage($"IsBroken{__instance.NetworkObjectId}", true, (ulong _, ExtraItemCharger charger) => ExtraItemChargerBinds[__instance.NetworkObjectId] = charger);
        }

        [HarmonyPatch(typeof(ItemCharger), nameof(ItemCharger.OnDestroy)), HarmonyPostfix]
        static void OnDestroyPostfix(ItemCharger __instance)
        {
            if (!ExtraItemChargerBinds.ContainsKey(__instance.NetworkObjectId))
                return;

            Network.UnregisterMessage($"IsBroken{__instance.NetworkObjectId}");
            GameState.WentIntoOrbit -= ExtraItemChargerBinds[__instance.NetworkObjectId].ResetItemCharger;
            ExtraItemChargerBinds.Remove(__instance.NetworkObjectId);
            ItemChargers.Remove(__instance);
        }

        [HarmonyPatch(typeof(ItemCharger), nameof(ItemCharger.PlayChargeItemEffectServerRpc)), HarmonyPrefix]
        private static bool PlayChargeItemEffectServerRpcPrefix(ItemCharger __instance)
        {
            if (!StartOfRound.Instance.shipHasLanded)
                return false;

            if (ExtraItemChargerBinds[__instance.NetworkObjectId].IsBroken)
                return false;

            GrabbableObject currentlyHeldObjectServer = GameNetworkManager.Instance.localPlayerController.currentlyHeldObjectServer;
            if (currentlyHeldObjectServer == null)
                return false;

            if (!(currentlyHeldObjectServer.itemProperties.requiresBattery || currentlyHeldObjectServer.itemProperties.isConductiveMetal))
                return false;

            ExtraItemChargerBinds[__instance.NetworkObjectId].IsBroken = currentlyHeldObjectServer.itemProperties.isConductiveMetal || Random.RandomRangeInt(-10, (int)(currentlyHeldObjectServer.insertedBattery.charge * 10f) + 1) >= Disabler.MinimumToExplode;
            Network.Broadcast($"IsBroken{__instance.NetworkObjectId}", ExtraItemChargerBinds[__instance.NetworkObjectId]);

            Traverse chargeItemCoroutine = Traverse.Create(__instance).Field("chargeItemCoroutine");

            __instance.PlayChargeItemEffectServerRpc((int)GameNetworkManager.Instance.localPlayerController.playerClientId);
            if (chargeItemCoroutine.GetValue<Coroutine>() != null)
                __instance.StopCoroutine(chargeItemCoroutine.GetValue<Coroutine>());
            chargeItemCoroutine.SetValue(__instance.StartCoroutine("chargeItemDelayed", currentlyHeldObjectServer));
            
            return false;
        }

        [HarmonyPatch(typeof(ItemCharger), "chargeItemDelayed"), HarmonyPostfix]
        static void ChargeItemDelayedPostfix(ItemCharger __instance, ref IEnumerator __result)
        {
            var myEnumerator = new IEnumeratorPatchHelper(__result)
            {
                postfixAction = () => {
                    if (ExtraItemChargerBinds[__instance.NetworkObjectId].IsBroken)
                    {
                        Landmine.SpawnExplosion(__instance.transform.position, true, 2.4f, 5f);
                        StartOfRound.Instance.PowerSurgeShip();
                    }
                }
            };

            __result = myEnumerator.GetEnumerator();
        }

        [HarmonyPatch(typeof(ItemCharger), "Update"), HarmonyPrefix]
        private static bool UpdatePrefix(ItemCharger __instance)
        {
            if (NetworkManager.Singleton == null)
                return false;

            switch (GameState.ShipState)
            {
                case ShipState.InOrbit:
                    __instance.triggerScript.interactable = false;
                    __instance.triggerScript.disabledHoverTip = "[Wait for ship to start]";
                    return false;

                case ShipState.OnMoon:
                case ShipState.LeavingMoon:
                    if (ExtraItemChargerBinds.ContainsKey(__instance.NetworkObjectId) && ExtraItemChargerBinds[__instance.NetworkObjectId].IsBroken)
                    {
                        __instance.triggerScript.interactable = false;
                        __instance.triggerScript.disabledHoverTip = "[Charger broken for the round]";
                        return false;
                    }

                    GrabbableObject item = GameNetworkManager.Instance.localPlayerController?.currentlyHeldObjectServer;
                    __instance.triggerScript.interactable = item != null && (item.itemProperties.requiresBattery || item.itemProperties.isConductiveMetal);
                    __instance.triggerScript.disabledHoverTip = "";
                    return false;
            }

            return false;
        }
    }
}
