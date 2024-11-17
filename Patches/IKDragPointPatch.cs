using HarmonyLib;
using RootMotion.FinalIK;
using UnityEngine;

namespace VRUtilities.Patches {
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
            }

            RotationLimit rotationLimit = targetTransform.GetComponent<RotationLimit>();

            if (rotationLimit != null) {
                rotationLimit.Apply();
            }
        }
    }
}
