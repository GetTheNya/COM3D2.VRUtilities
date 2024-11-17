using UnityEngine;
using VRUtilities.OnScreenKeyboard.UIHelper;

namespace VRUtilities.OnScreenKeyboard {
    public class Prefab {
        public static GameObject CreateOnScreenKeyboard(GameObject uiRoot) {
            var instance = OnScreenKeyboardBuilder.Build(uiRoot);
            instance.AddComponent<OnScreenKeyboardManager>();
            instance.name = "OnScreenKeyboard";

            return instance;
        }
    }
}
