using UnityEngine;

namespace VRUtilities.OnScreenKeyboard.UIHelper {
    public class UIUtility {
        static UIAtlas atlasCommon;

        public static UIAtlas GetAtlasCommon() {
            if (atlasCommon == null) {
                var prefab = Resources.Load<UIAtlas>("CommonUI/Atlas/AtlasCommon");
                atlasCommon = Object.Instantiate(prefab);
                Object.DontDestroyOnLoad(atlasCommon);
            }

            return atlasCommon;
        }

        public static Font GetNotoSansCJKjpDemiLightFont()
        {
            return Resources.Load<Font>("font/notosanscjkjp-hinted/notosanscjkjp-demilight");
        }
    }
}
