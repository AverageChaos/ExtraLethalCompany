using System.Reflection;
using BepInEx.Bootstrap;
using HarmonyLib;
using UnityEngine;

namespace ExtraLethalCompany
{
    internal static class Disabler
    {
        public static int MinimumToExplode = 0;

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

        private static void LethalCompanyMinimap(object inst)
        {
            DisableHarmony(inst, "harmony");
            Object.DestroyImmediate(GameObject.Find("MinimapGUI"));
        }
        private static void dev_alexanderdiaz_biggerbattery(object _) => MinimumToExplode = -3;
        private static void stormytuna_RouteRandom(object inst) => DisableHarmony(inst, "harmony");
        private static void NoPenalty(object inst) => DisableHarmony(inst, "harmonymain");

        private static void DisableHarmony(object inst, string harmonyName) => Traverse.Create(inst).Field(harmonyName).GetValue<Harmony>().UnpatchSelf();
    }
}
