using System.Reflection;
using System.Runtime.CompilerServices;
using BepInEx.Bootstrap;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace ExtraLethalCompany
{
    internal static class Disabler
    {
        public static int MinimumToExplode { get; private set; } = 0;
        public static bool UseExtraLethalPenalty { get; private set; } = false;
        public static bool HideEnemies { get; private set; } = false;

        internal static void Init()
        {
            foreach (BepInEx.PluginInfo pluginInfo in Chainloader.PluginInfos.Values)
            {
                MethodInfo method = typeof(Disabler).GetMethod(pluginInfo.Metadata.GUID.Replace('.', '_').Replace(' ', '_'), BindingFlags.NonPublic | BindingFlags.Static);
                if (method != null)
                {
                    ExtraLethalCompany.LogSource.LogInfo("Disabling: " + pluginInfo.Instance.GetType().Name);
                    method.Invoke(typeof(Disabler), [pluginInfo.Instance]);
                }
            }
        }

        private static void LethalCompanyMinimap(object inst) => HideEnemies = true;
        private static void dev_alexanderdiaz_biggerbattery(object _) => MinimumToExplode = -3;
        private static void stormytuna_RouteRandom(object inst) => DisableHarmony(inst, "harmony");
        private static void NoPenalty(object inst)
        {
            DisableHarmony(inst, "harmonymain");
            UseExtraLethalPenalty = true;
        }

        private static void DisableHarmony(object inst, string harmonyName) => Traverse.Create(inst).Field(harmonyName).GetValue<Harmony>().UnpatchSelf();

        [HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.Start)), HarmonyPrefix]
        static void StartPrefix(EnemyAI __instance)
        {
            if (!HideEnemies)
                return;

            foreach (Transform child in __instance.gameObject.gameObject.transform)
            {
                if (child.name.Contains("MapDot"))
                {
                    child.gameObject.SetActive(false);
                    return;
                }
            }
        }
    }
}
