using Sirenix.OdinInspector;
using UnityEngine;
using System.IO;
using tools;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace log.settings
{
    class ELogSettingsSO : ScriptableObject
    {
        public static ELogSettingsSO Instance => MenuExtensions.LoadSOFromResources<ELogSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));

        [Header("Constants")]
        [ShowInInspector] public const string CONFIG_FILE_ROOT = "Assets/MyProject/Resources/Settings/LOG";

        [ShowInInspector] public const string CONFIG_FILE_PATH = "log_settings.asset";
        [ShowInInspector] public const string CONFIG_FILE_TYPES = "log_type_settings.asset";

#if UNITY_EDITOR
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 0)]
        [Button("Type Settings")]
        private void GoToTypeSettings()
        {
            MenuItem_LogTypeSettings();
        }


        // ----------------------------------------------------------------------------------
        // ========================== Menu Items ============================
        // ----------------------------------------------------------------------------------

        [MenuItem("MyProject/LOG/Settings")]
        public static void MenuItem_LogSettings()
        {
            MenuExtensions.PingOrCreateSO<ELogSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));
        }

        [MenuItem("MyProject/LOG/Type Settings")]
        public static void MenuItem_LogTypeSettings()
        {
            MenuExtensions.PingOrCreateSO<ELogTypeSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_TYPES));
        }
#endif
    }
}
