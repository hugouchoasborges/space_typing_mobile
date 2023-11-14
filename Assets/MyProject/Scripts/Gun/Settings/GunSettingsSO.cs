using Sirenix.OdinInspector;
using tools;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace gun.settings
{
    class GunSettingsSO : ScriptableObject
    {
        public static GunSettingsSO Instance => MenuExtensions.LoadSOFromResources<GunSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));

        [Header("Constants")]
        [ShowInInspector] public const string CONFIG_FILE_ROOT = "Assets/MyProject/Resources/Settings/Gun";

        [ShowInInspector] public const string CONFIG_FILE_PATH = "gun_settings.asset";
        [ShowInInspector] public const string CONFIG_FILE_TYPES = "gun_type_settings.asset";

        [Header("Guns")]
        public GameObject GunDefault;
        public GameObject EnemyGun;

        [Header("Bullets")]
        public GameObject BulletDefault;


#if UNITY_EDITOR

        // ----------------------------------------------------------------------------------
        // ========================== Menu Items ============================
        // ----------------------------------------------------------------------------------

        [MenuItem("MyProject/GUN/Settings")]
        public static void MenuItem_SceneSettings()
        {
            MenuExtensions.PingOrCreateSO<GunSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));
        }
#endif
    }
}
