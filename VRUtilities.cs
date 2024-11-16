using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;

namespace VRUtilities {
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class VRUtilities : BaseUnityPlugin {
        public static class PluginInfo {
            public const string NAME = "VRUtilities";
            public const string GUID = "GetTheNya.COM3D2." + NAME;
            public const string VERSION = "0.1";
        }

        public static VRUtilities Instance;
        public static ManualLogSource PluginLogger => Instance.Logger;

        public static readonly Type[] PatchTypes = {
            typeof(IKDragPointPatch),
            typeof(WorldTransformAxisPatch),
        };


        void Awake() {
            Instance = this;

            PluginLogger.LogInfo($"Hello from {PluginInfo.NAME}!");

            if (!GameMain.Instance.VRMode) {
                PluginLogger.LogInfo("VR Not detected");
                return;
            }

            PatchAll(PatchTypes);

            PluginLogger.LogInfo("Game patched");
        }

        static void PatchAll(Type[] patchTypes) {
            foreach (var type in patchTypes) {
                PluginLogger.LogInfo($"Patching {type.FullName.Replace("Patch", "")}");
                Harmony.CreateAndPatchAll(type);
            }
        }
    }
}