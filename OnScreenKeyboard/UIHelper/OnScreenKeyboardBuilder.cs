using System;
using UnityEngine;

namespace VRUtilities.OnScreenKeyboard.UIHelper {
    public class K {
        public string Key;
        public string CapsKey;
        public int Width;
        public UISprite Button;

        public K(string key, int width = 50) {
            Key = key;
            CapsKey = key.ToUpper();
            Width = width;
        }

        public K(string key, string shiftKey, int width = 50) {
            Key = key;
            CapsKey = shiftKey ?? key;
            Width = width;
        }


        public string GetKey(bool isCaps) {
            return isCaps ? CapsKey : Key;
        }

        public UIButton GetKeyButton() {
            if (Button != null) {
                return Button.GetComponentInChildren<UIButton>();
            }

            return null;
        }

        public UILabel GetKeyLabel() {
            if (Button != null) {
                return Button.GetComponentInChildren<UILabel>();
            }

            return null;
        }
    }

    public static class OnScreenKeyboardBuilder {
        public static readonly K[] ROW1 = { new K("1", "!"), new K("2", "@"), new K("3", "#"), new K("4", "$"), new K("5", "%"), new K("6", "^"), new K("7", "&"), new K("8", "*"), new K("9", "("), new K("0", ")"), new K("-", "_"), new K("=", "+"), new K("BcSp", null, 100) };
        public static readonly K[] ROW2 = { new K("q"), new K("w"), new K("e"), new K("r"), new K("t"), new K("y"), new K("u"), new K("i"), new K("o"), new K("p"), new K("[", "{"), new K("]", "}"), new K("\\", "|") };
        public static readonly K[] ROW3 = { new K("a"), new K("s"), new K("d"), new K("f"), new K("g"), new K("h"), new K("j"), new K("k"), new K("l"), new K(";", ":"), new K("'", "\""), new K("Enter", null, 100) };
        public static readonly K[] ROW4 = { new K("z"), new K("x"), new K("c"), new K("v"), new K("b"), new K("n"), new K("m"), new K(",", "<"), new K(".", ">"), new K("/", "?"), new K("Caps", null, 100) };
        public static readonly K[] ROW5 = { new K("Space", null, 500) };

        public static readonly K[][] KEY_ROWS = { ROW1, ROW2, ROW3, ROW4, ROW5 };

        public static GameObject NewGameObject(GameObject parent, string name, params Type[] components) {
            var obj = new GameObject(name, components) {
                layer = 8
            };
            obj.transform.SetParent(parent.transform, false);

            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localEulerAngles = Vector3.zero;

            return obj;
        }

        public static T AddWidget<T>(GameObject go) where T : UIWidget {
            var component = go.AddComponent<T>();
            component.depth = NGUITools.CalculateNextDepth(go.transform.parent.gameObject);
            return component;
        }

        public static GameObject Build(GameObject root) {
            var osc = NewGameObject(root, "OnScreenKeyboard", typeof(UIPanel));
            osc.GetComponent<UIPanel>().depth = NGUITools.CalculateNextDepth(root);
            osc.transform.localPosition = new Vector3(0, -660f);

            var itemWindow = NewGameObject(osc, "ItemWindow");

            CreateBG3(itemWindow);

            var bg = NewGameObject(itemWindow, "BG");

            CreateTitleBar(bg);

            var keyPos = new Vector3(GetStartPoint(ROW1), 380);
            DrawRow(ROW1, ref keyPos, bg);
            keyPos = new Vector3(GetStartPoint(ROW2), 325);
            DrawRow(ROW2, ref keyPos, bg);
            keyPos = new Vector3(GetStartPoint(ROW3), 270);
            DrawRow(ROW3, ref keyPos, bg);
            keyPos = new Vector3(GetStartPoint(ROW4), 215);
            DrawRow(ROW4, ref keyPos, bg);
            keyPos = new Vector3(GetStartPoint(ROW5), 160);
            DrawRow(ROW5, ref keyPos, bg);

            CreateBG2(itemWindow);

            return osc;
        }

        private static float GetStartPoint(K[] row) {
            var rowLength = 5;
            foreach (var k in row) rowLength += k.Width + 5;

            return -(rowLength / 2);
        }

        private static void DrawRow(K[] row, ref Vector3 keyPos, GameObject parent) {
            foreach (var k in row) {
                keyPos.x += k.Width / 2 + 5;
                k.Button = CreateButton(k.Key, k.Key, new Vector2(k.Width, 50), keyPos, parent);
                keyPos.x += k.Width / 2;
            }
        }

        private static UISprite CreateButton(string name, string text, Vector2 size, Vector3 pos, GameObject parent) {
            var button = NGUITools.AddSprite(parent, UIUtility.GetAtlasCommon(), "cm3d2_common_plate_white");
            button.name = name;
            button.width = (int)size.x;
            button.height = (int)size.y;
            button.transform.localPosition = pos;
            NGUITools.AddWidgetCollider(button.gameObject);

            button.GetOrAddComponent<UIButton>();

            var label = NGUITools.AddWidget<UILabel>(button.gameObject);
            label.trueTypeFont = UIUtility.GetNotoSansCJKjpDemiLightFont();
            label.text = text;
            label.color = Color.black;

            return button;
        }

        private static void CreateTitleBar(GameObject bg) {
            var titleBar = NewGameObject(bg, "TitleBar");
            var titleBarSprite = AddWidget<UISprite>(titleBar);
            titleBarSprite.atlas = UIUtility.GetAtlasCommon();
            titleBarSprite.spriteName = "cm3d2_common_plate_black_top_win";
            titleBarSprite.type = UIBasicSprite.Type.Sliced;
            titleBarSprite.width = 770;
            titleBarSprite.height = 20;
            titleBarSprite.pivot = UIWidget.Pivot.Top;
            titleBar.transform.localPosition = new Vector3(0, 430, 0);


            var titleBarText = NewGameObject(titleBar, "Text");
            var label = AddWidget<UILabel>(titleBarText);
            label.trueTypeFont = UIUtility.GetNotoSansCJKjpDemiLightFont();
            label.text = "OnScreenKeyboard";
            label.fontSize = 16;
            label.color = Color.white;
            label.width = 174;
            label.height = 20;
            label.pivot = UIWidget.Pivot.Left;
            label.alignment = NGUIText.Alignment.Left;
            titleBarText.transform.localPosition = new Vector3(-380, -10f, 0f);

            var endButton = NewGameObject(titleBar, "End");
            endButton.AddComponent<BoxCollider>();

            var endSprite = AddWidget<UISprite>(endButton);
            endSprite.atlas = UIUtility.GetAtlasCommon();
            endSprite.spriteName = "cm3d2_common_win_btn_end";
            endSprite.width = endSprite.height = 16;
            endSprite.type = UIBasicSprite.Type.Sliced;
            endSprite.pivot = UIWidget.Pivot.Center;
            endSprite.ResizeCollider();

            endButton.AddComponent<UIButton>();

            endButton.transform.localPosition = new Vector3(372f, -10f);
        }

        private static void CreateBG2(GameObject itemWindow) {
            var bg2 = NGUITools.AddSprite(itemWindow, UIUtility.GetAtlasCommon(), "cm3d2_common_lineframe_white_d");
            bg2.name = "BG2";
            bg2.transform.localPosition = new Vector3(0f, 270f);
            bg2.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
            bg2.width = 770;
            bg2.height = 280;
        }

        private static void CreateBG3(GameObject itemWindow) {
            var bgSprite = NGUITools.AddSprite(itemWindow, UIUtility.GetAtlasCommon(), "cm3d2_common_plate_black");

            bgSprite.name = "BG3";
            bgSprite.width = 770;
            bgSprite.height = 300;
            bgSprite.depth = -2;

            NGUITools.AddWidgetCollider(bgSprite.gameObject);
            bgSprite.transform.localPosition = new Vector3(0, 280, 0);

            var dragger = bgSprite.GetOrAddComponent<PhotoWindowDragMove>();
            dragger.WindowTransform = itemWindow.transform.parent.transform;
        }
    }
}
