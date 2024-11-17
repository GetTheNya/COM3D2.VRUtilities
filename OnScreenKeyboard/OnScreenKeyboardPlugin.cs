using System;
using COM3D2API;
using UnityEngine;
using VRUtilities.OnScreenKeyboard.UIHelper;

namespace VRUtilities.OnScreenKeyboard {
    public static class OnScreenKeyboardPlugin {
        public const string ICON = "iVBORw0KGgoAAAANSUhEUgAAABwAAAAcCAYAAAByDd+UAAABKklEQVRIidWWwUrDQBRFzyQl0NKVCrprPyE7v6G49lf9hWShmwT9BgvWZYVAZuSFSY2hptOQGfWuXoZkzrw7D3IVsASugGvgEpgDEdNIA3vgHXgFdrLrKk3T+6Iono0nZVn2KAxgLcBbWfAFayUNCUss3RhjHiaycFBKqTu5q0UImNU8mnBAXBSFhDWadR+UUl4gxphDfejQF6y/d2hLF7Njq60FcrKxNcdd2//u0LTqnmxM3e3SCdi35Vxbh/T3LO0/n1s7AX+y9JQcpvQfWHpKQ+8mSaKCdhjH8RfQ9Z7GSGvdfFVVlflmqU+oqK5rE3xoIhvlQkkL8CMgsPlb7MqyfPFNyvP8yQZi1hJSfWZTyaQ2CK/aqC8R/wa4sLFxyqgvVyYRfwu8fQKaaoPYw4OrLQAAAABJRU5ErkJggg==";

        public static OnScreenKeyboardManager Manager { get; private set; }

        public static void Init() {
            SystemShortcutAPI.AddButton("OnScreenKeyboard", OnSystemButtonClick, "Open OnScreenKeyboard", Convert.FromBase64String(ICON));
        }

        public static void OnSystemButtonClick() {
            EnsureManagerInitialized().ToggleWindow();
        }

        private static OnScreenKeyboardManager EnsureManagerInitialized() {
            if (Manager == null) {
                GameObject uiRoot = GameObject.Find("SystemUI Root");
                var obj = Prefab.CreateOnScreenKeyboard(uiRoot);
                Manager = obj.GetComponent<OnScreenKeyboardManager>();
                Manager.HideWindow();
            }

            return Manager;
        }
    }
}
