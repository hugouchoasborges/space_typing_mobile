using Sirenix.OdinInspector;
using tools;
using System.IO;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace powerup.settings
{
    public class PowerUpSettingsSO : ScriptableObject
    {
        public static PowerUpSettingsSO Instance => MenuExtensions.LoadSOFromResources<PowerUpSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));

        [Header("Constants")]
        [ShowInInspector] public const string CONFIG_FILE_ROOT = "Assets/MyProject/Resources/Settings/PowerUp";

        [ShowInInspector] public const string CONFIG_FILE_PATH = "powerUp_settings.asset";
        [ShowInInspector] public const string CONFIG_FILE_TYPES = "powerUp_type_settings.asset";

        [Header("PowerUps")]
        public PowerUpMultiShootData MultiShoot;


#if UNITY_EDITOR

        // ----------------------------------------------------------------------------------
        // ========================== Menu Items ============================
        // ----------------------------------------------------------------------------------

        [MenuItem("MyProject/POWERUPS/Settings")]
        public static void MenuItem_SceneSettings()
        {
            MenuExtensions.PingOrCreateSO<PowerUpSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));
        }
#endif
    }

    [System.Serializable]
    public struct PowerUpMultiShootData
    {
        [Header("Components")]
        [SerializeField] private Sprite IconUI;
        [SerializeField] private Sprite IconGameplay;


        // =========== MultiShoot specifics ===========
        [Header("Power-Up specifics")]
        [Range(1, 200)] public int PointsRequired;
        [Range(1, 30)] public int DurationSeconds;

    }
}
