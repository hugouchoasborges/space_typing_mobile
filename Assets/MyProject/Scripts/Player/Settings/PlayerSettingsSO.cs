using Sirenix.OdinInspector;
using tools;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace player.settings
{
    class PlayerSettingsSO : ScriptableObject
    {
        public static PlayerSettingsSO Instance => MenuExtensions.LoadSOFromResources<PlayerSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));

        [Header("Constants")]
        [ShowInInspector] public const string CONFIG_FILE_ROOT = "Assets/MyProject/Resources/Settings/Player";

        [ShowInInspector] public const string CONFIG_FILE_PATH = "player_settings.asset";

        [Header("Components")]
        public GameObject Prefab;
        [ListDrawerSettings(Expanded = true)] public Sprite[] Skins;


#if UNITY_EDITOR

        // ----------------------------------------------------------------------------------
        // ========================== Menu Items ============================
        // ----------------------------------------------------------------------------------

        [MenuItem("MyProject/PLAYER/Settings")]
        public static void MenuItem_PlayerSettings()
        {
            MenuExtensions.PingOrCreateSO<PlayerSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));
        }
#endif
    }
}
