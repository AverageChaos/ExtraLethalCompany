using HarmonyLib;
using System.Linq;
using System.Reflection;

namespace ExtraLethalCompany.Helpers
{
    internal class DynamicPatch
    {
        internal delegate bool PrefixDel(HarmonyParams args);
        internal event PrefixDel Prefix;

        internal delegate void PostfixeDel(HarmonyParams args);
        internal event PostfixeDel Postfix;

        private readonly string[] ParamNames;

        internal DynamicPatch(MethodInfo original)
        {
            ParamNames = original.GetParameters().Select(p => p.Name).ToArray();
            HarmonyPatcher.HarmonyInstance.Patch(
                original, 
                prefix: new(SymbolExtensions.GetMethodInfo(() => PrefixMethod)), 
                postfix: new(SymbolExtensions.GetMethodInfo(() => PostfixMethod))
            );
        }

        private bool PrefixMethod(object __instance, object __result, object[] __args) => (bool)Prefix?.Invoke(new HarmonyParams(__instance, __result, __args, ParamNames));
        private void PostfixMethod(object __instance, object __result, object[] __args) => Postfix?.Invoke(new HarmonyParams(__instance, __result, __args, ParamNames));
    }
}
