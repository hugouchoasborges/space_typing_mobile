using Sirenix.OdinInspector;
using tools;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace collectable.settings
{
    class CollectableSettingsSO : ScriptableObject
    {
        public static CollectableSettingsSO Instance => MenuExtensions.LoadSOFromResources<CollectableSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));

        [Header("Constants")]
        [ShowInInspector] public const string CONFIG_FILE_ROOT = "Assets/MyProject/Resources/Settings/Collectable";

        [ShowInInspector] public const string CONFIG_FILE_PATH = "collectable_settings.asset";

        [Header("Components")]
        public GameObject DefaultPrefab;


#if UNITY_EDITOR

        // ----------------------------------------------------------------------------------
        // ========================== Menu Items ============================
        // ----------------------------------------------------------------------------------

        [MenuItem("MyProject/COLLECTABLES/Settings")]
        public static void MenuItem_CollectableSettings()
        {
            MenuExtensions.PingOrCreateSO<CollectableSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));
        }
#endif
    }
}
