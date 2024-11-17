using UnityEngine;
using VRUtilities.OnScreenKeyboard.UIHelper;

namespace VRUtilities.OnScreenKeyboard {
    public class OnScreenKeyboardManager : MonoBehaviour {
        private bool _isHidden;

        private bool _isCaps;

        public static UIInput LastActiveInput;

        void Start() {
            SetupComponents();
        }

        void Update() {
            if (UIInput.selection != null) {
                LastActiveInput = UIInput.selection;
            }
        }

        private void SetupComponents() {
            var bg3 = gameObject.transform.Find("ItemWindow/BG3").gameObject;

            var bg = gameObject.transform.Find("ItemWindow/BG").gameObject;
            SetClickCallback(bg, "TitleBar/End", () => {
                HideWindow();
                DisableCaps();
            });

            foreach (var row in OnScreenKeyboardBuilder.KEY_ROWS) {
                foreach (var k in row) {
                    SetClickCallback(k.GetKeyButton(), () => { OnKeyClick(k.GetKey(_isCaps)); });
                }
            }
        }

        private void SetClickCallback(GameObject bg, string child, EventDelegate.Callback callback) {
            var btn = bg.transform.Find(child).gameObject;
            var uiBtn = btn.GetComponent<UIButton>();
            SetClickCallback(uiBtn, callback);
        }

        private void SetClickCallback(UIButton btn, EventDelegate.Callback callback) {
            btn.onClick.Clear();
            EventDelegate.Add(btn.onClick, callback);
        }

        private void OnKeyClick(string key) {
            switch (key) {
                case "Caps":
                    ToggleCaps();
                    break;
                case "BcSp":
                    Backspace();
                    break;
                case "Space":
                    InputKey(" ");
                    break;
                default:
                    InputKey(key);
                    break;
            }
        }

        public void InputKey(string c) {
            if (LastActiveInput == null) return;
            LastActiveInput.value += c;
        }

        private void Backspace() {
            if (LastActiveInput == null) return;
            LastActiveInput.value = RemoveLastChar(LastActiveInput.value);
        }

        private string RemoveLastChar(string str) {
            if (str.Length <= 0) return str;
            return str.Remove(str.Length - 1, 1);
        }

        public void ToggleCaps() {
            if (_isCaps) {
                DisableCaps();
            } else {
                EnableCaps();
            }
        }

        public void DisableCaps() {
            _isCaps = false;
            UpdateKeys();

        }

        public void EnableCaps() {
            _isCaps = true;
            UpdateKeys();
        }

        private void UpdateKeys() {
            foreach (var row in OnScreenKeyboardBuilder.KEY_ROWS) {
                foreach (var k in row) {
                    k.GetKeyLabel().text = k.GetKey(_isCaps);
                }
            }
        }

        public void ToggleWindow() {
            if (_isHidden) {
                ShowWindow();
            } else {
                HideWindow();
            }
        }

        public void HideWindow() {
            gameObject.SetActive(false);
            _isHidden = true;
        }

        public void ShowWindow() {
            gameObject.SetActive(true);
            _isHidden = false;
        }
    }
}
