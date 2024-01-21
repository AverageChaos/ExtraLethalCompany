using ExtraLethalCompany.Extra.Company;
using ExtraLethalCompany.Extra.Creatures;
using ExtraLethalCompany.Extra.Furniture.ExtraItemCharger;
using ExtraLethalCompany.Extra.Furniture.ExtraTerminal;
using ExtraLethalCompany.Extra.HUD;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExtraLethalCompany.Helpers
{
    internal static class HarmonyPatcher
    {
        internal static readonly Harmony HarmonyInstance = new(PluginInfo.PLUGIN_GUID);
        private static readonly Dictionary<MethodInfo, DynamicPatch> DynamicPatches = [];

        static HarmonyPatcher()
        {
            HarmonyInstance.PatchAll(typeof(HUDManager_Patch));
            HarmonyInstance.PatchAll(typeof(Terminal_Patch));
            HarmonyInstance.PatchAll(typeof(ItemCharger_Patch));
            HarmonyInstance.PatchAll(typeof(Extra.Furniture.ExtraTerminal.StartOfRound_Patch));
            HarmonyInstance.PatchAll(typeof(Extra.Ship.StartOfRound_Patch));
            HarmonyInstance.PatchAll(typeof(TimeOfDay_Patch));
            HarmonyInstance.PatchAll(typeof(BlobAI_Patch));
            HarmonyInstance.PatchAll(typeof(Disabler));
        }

        internal static DynamicPatch GetOrCreateDynamicPatch(Type type, string methodName)
        {
            MethodInfo mOriginal = AccessTools.Method(type, methodName);
            return DynamicPatches.GetOrCreate(mOriginal, () => new DynamicPatch(mOriginal));
        }
    }
}
