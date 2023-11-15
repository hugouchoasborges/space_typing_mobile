using Sirenix.OdinInspector;
using UnityEngine;
using System.IO;
using tools;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace sound.settings
{
    class ESoundSettingsSO : ScriptableObject
    {
        public static ESoundSettingsSO Instance => MenuExtensions.LoadSOFromResources<ESoundSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));

        [Header("Constants")]
        [ShowInInspector] public const string CONFIG_FILE_ROOT = "Assets/MyProject/Resources/Settings/SOUND";

        [ShowInInspector] public const string CONFIG_FILE_PATH = "sound_settings.asset";
        [ShowInInspector] public const string CONFIG_FILE_TYPES = "sound_type_settings.asset";

#if UNITY_EDITOR
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 0)]
        
		[Button("Settings")]
        private void GoToSettings()
        {
            MenuItem_SoundSettings();
        }
		
		[Button("Type Settings")]
        private void GoToTypeSettings()
        {
            MenuItem_SoundTypeSettings();
        }


        // ----------------------------------------------------------------------------------
        // ========================== Menu Items ============================
        // ----------------------------------------------------------------------------------

        [MenuItem("MyProject/SOUND/Settings")]
        public static void MenuItem_SoundSettings()
        {
            MenuExtensions.PingOrCreateSO<ESoundSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));
        }

        [MenuItem("MyProject/SOUND/Type Settings")]
        public static void MenuItem_SoundTypeSettings()
        {
            MenuExtensions.PingOrCreateSO<ESoundTypeSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_TYPES));
        }
#endif
    }
}
