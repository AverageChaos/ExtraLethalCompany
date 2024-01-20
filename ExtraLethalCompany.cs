using BepInEx;
using BepInEx.Logging;
using ExtraLethalCompany.Patches;
using HarmonyLib;
using GameNetcodeStuff;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using ExtraLethalCompany.Unity;
using LC_API.ServerAPI;
using LC_API.Data;
using LC_API.GameInterfaceAPI;
using ExtraLethalCompany.Extra.Furniture.ExtraItemCharger;
using ExtraLethalCompany.Extra.Creatures;
using Steamworks;
using Steamworks.Data;
using System;

namespace ExtraLethalCompany
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, "1.1.5")]
    [BepInDependency("LC_API", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("LethalCompanyMinimap", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("dev.alexanderdiaz.biggerbattery", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("stormytuna.RouteRandom", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("NoPenalty", BepInDependency.DependencyFlags.SoftDependency)]
    public class ExtraLethalCompany : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;
        //public static readonly AssetBundle Assets = AssetBundle.LoadFromFile($"{Paths.PluginPath}\\ExtraLethalCompany\\scps");
        //public static readonly GameObject SCP173 = Assets.LoadAsset<GameObject>("SCP-173");
        //private static GameObject SCP173Inst;

        private static readonly Harmony Harmony = new(PluginInfo.PLUGIN_GUID);

        private void Awake()
        {
            if (gameObject.name != "ExtraLethalCompany")
            {
                GameObject go = new("ExtraLethalCompany");
                go.AddComponent<ExtraLethalCompany>();
                go.AddComponent<ExtraSporeLizzard>();
                DontDestroyOnLoad(go);
                go.hideFlags = HideFlags.HideAndDontSave;
                return;
            }

            LogSource = Logger;

            ModdedServer.SetServerModdedOnly();
            Disabler.Init();

            Harmony.PatchAll(typeof(HUDManager_Patch));
            Harmony.PatchAll(typeof(Terminal_Patch));
            Harmony.PatchAll(typeof(ItemCharger_Patch));
            Harmony.PatchAll(typeof(StartOfRound_Patch));
            Harmony.PatchAll(typeof(TimeOfDay_Patch));
            Harmony.PatchAll(typeof(BlobAI_Patch));

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void Update()
        {
            /*Keyboard keyboard = Keyboard.current;
            if (keyboard.f1Key.wasPressedThisFrame)
            {
                SCP173.GetComponent<MeshRenderer>().SetMaterial(HDRPMaterial.CreateMaterial("SCP-173", HDRPMaterial.HDRP_Lit));

                DestroyImmediate(SCP173Inst);
                SCP173Inst = Instantiate(SCP173, FindObjectsOfType<PlayerControllerB>().Where(pc => pc.gameObject.name == "Player").First().transform.position, Quaternion.identity);
                SCP173Inst.transform.localScale *= 1.5f;
            }*/
        }
    }
}
