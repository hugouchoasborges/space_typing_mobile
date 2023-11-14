using Sirenix.OdinInspector;
using tools;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace enemy.settings
{
    class EnemySettingsSO : ScriptableObject
    {
        public static EnemySettingsSO Instance => MenuExtensions.LoadSOFromResources<EnemySettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));

        [Header("Constants")]
        [ShowInInspector] public const string CONFIG_FILE_ROOT = "Assets/MyProject/Resources/Settings/Enemy";

        [ShowInInspector] public const string CONFIG_FILE_PATH = "enemy_settings.asset";

        [Header("Enemies")]
        public GameObject EnemyDefault;
        public GameObject EnemyShooter;


#if UNITY_EDITOR

        // ----------------------------------------------------------------------------------
        // ========================== Menu Items ============================
        // ----------------------------------------------------------------------------------

        [MenuItem("MyProject/ENEMY/Settings")]
        public static void MenuItem_EnemySettings()
        {
            MenuExtensions.PingOrCreateSO<EnemySettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));
        }
#endif
    }
}
