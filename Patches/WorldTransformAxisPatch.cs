using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace VRUtilities.Patches {
    public static class WorldTransformAxisPatch {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(WorldTransformAxis), "Update")]
        static IEnumerable<CodeInstruction> UpdateTranspilerInjection(IEnumerable<CodeInstruction> instructions) {
            var codes = new List<CodeInstruction>(instructions);

            for (int i = 0; i < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Callvirt && codes[i].operand.ToString().Contains("get_VRMode")) {
                    codes.RemoveRange(i - 1, codes.Count - i);
                    break;
                }
            }

            return codes.AsEnumerable();
        }
    }
}
