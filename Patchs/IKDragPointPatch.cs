using HarmonyLib;
using RootMotion.FinalIK;
using UnityEngine;

namespace VRUtilities {
    public static class IKDragPointPatch {
        private static readonly AccessTools.FieldRef<IKDragPoint, bool> is_drag_ = AccessTools.FieldRefAccess<bool>(typeof(IKDragPoint), "is_drag_");
        private static readonly AccessTools.FieldRef<IKDragPoint, Transform> target_trans_ = AccessTools.FieldRefAccess<Transform>(typeof(IKDragPoint), "target_trans_");

        [HarmonyPostfix]
        [HarmonyPatch(typeof(IKDragPoint), "Update")]
        static void Update(IKDragPoint __instance) {
            Transform targetTransform = target_trans_(__instance);

            if (__instance.axis_obj == null || targetTransform == null) return;

            bool isDraggingPoint = is_drag_(__instance);

            if (isDraggingPoint) {
                targetTransform.rotation = __instance.axis_obj.transform.rotation;
            } else {
                __instance.axis_obj.transform.rotation = targetTransform.rotation;
                // __instance.axis_obj.transform.Rotate(0, 2, 0);
            }

            RotationLimit rotationLimit = targetTransform.GetComponent<RotationLimit>();

            if (rotationLimit != null) {
                rotationLimit.Apply();
            }
        }

        // [HarmonyPostfix]
        // [HarmonyPatch(typeof(IKDragPoint), "OnDestroy")]
        // static void OnDestroy(IKDragPoint __instance) {
        //     if (__instance.name.Contains("R Forearm")) {
        //         VRUtilities.PluginLogger.LogInfo("OnDestroy called");
        //     }
        // }
        //
        // [HarmonyPostfix]
        // [HarmonyPatch(typeof(IKDragPoint), "SetTargetIKPoint")]
        // static void SetTargetIKPoint(IKDragPoint __instance, GameObject obj) {
        //     if (__instance.name.Contains("R Forearm")) {
        //         VRUtilities.PluginLogger.LogInfo("SetTargetIKPoint called");
        //     }
        // }
        //
        // [HarmonyPostfix]
        // [HarmonyPatch(typeof(IKDragPoint), "SetWorldTransformAxis")]
        // static void SetWorldTransformAxis(IKDragPoint __instance, WorldTransformAxis axis_obj) {
        //     if (__instance.name.Contains("R Forearm")) {
        //         VRUtilities.PluginLogger.LogInfo("SetWorldTransformAxis called");
        //     }
        // }
        //
        // [HarmonyPostfix]
        // [HarmonyPatch(typeof(IKDragPoint), "OnDragStart")]
        // static void OnDragStart(IKDragPoint __instance) {
        //     if (__instance.name.Contains("R Forearm")) {
        //         VRUtilities.PluginLogger.LogInfo("OnDragStart called");
        //     }
        // }
        //
        // [HarmonyPostfix]
        // [HarmonyPatch(typeof(IKDragPoint), "OnDrag")]
        // static void OnDrag(IKDragPoint __instance) {
        //     if (__instance.name.Contains("R Forearm")) {
        //         VRUtilities.PluginLogger.LogInfo("OnDrag called");
        //     }
        // }
        //
        // [HarmonyPostfix]
        // [HarmonyPatch(typeof(IKDragPoint), "OnDragEnd")]
        // static void OnDragEnd(IKDragPoint __instance) {
        //     if (__instance.name.Contains("R Forearm")) {
        //         VRUtilities.PluginLogger.LogInfo("OnDragEnd called");
        //     }
        // }
    }
}
