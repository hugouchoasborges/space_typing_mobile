using Sirenix.OdinInspector;
using tools;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace scenes.settings
{
    class SceneHelperSettingsSO : ScriptableObject
    {
        public static SceneHelperSettingsSO Instance => MenuExtensions.LoadSOFromResources<SceneHelperSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));

        [Header("Constants")]
        [ShowInInspector] public const string CONFIG_FILE_ROOT = "Assets/MyProject/Resources/Settings/SCENES";

        [ShowInInspector] public const string CONFIG_FILE_PATH = "scene_settings.asset";
        [ShowInInspector] public const string CONFIG_FILE_TYPES = "scene_type_settings.asset";

#if UNITY_EDITOR

        [PropertySpace(SpaceBefore = 10, SpaceAfter = 0)]
        [Button("Type Settings")]
        private void GoToTypeSettings()
        {
            MenuItem_SceneTypeSettings();
        }


        // ----------------------------------------------------------------------------------
        // ========================== Menu Items ============================
        // ----------------------------------------------------------------------------------

        [MenuItem("MyProject/SCENES/Settings/Settings")]
        public static void MenuItem_SceneSettings()
        {
            MenuExtensions.PingOrCreateSO<SceneHelperSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));
        }

        [MenuItem("MyProject/SCENES/Settings/Type Settings")]
        public static void MenuItem_SceneTypeSettings()
        {
            MenuExtensions.PingOrCreateSO<SceneHelperTypeSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_TYPES));
        }
#endif
    }
}
