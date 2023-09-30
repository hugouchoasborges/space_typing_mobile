using Sirenix.OdinInspector;
using UnityEngine;
using System.IO;
using tools;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace myproject.fsm.settings
{
    class FSMSettingsSO : ScriptableObject
    {
        public static FSMSettingsSO Instance => MenuExtensions.LoadSOFromResources<FSMSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));

        [Header("Constants")]
        [ShowInInspector] public const string CONFIG_FILE_ROOT = "Assets/MyProject/Resources/Settings/FSM";

        [ShowInInspector] public const string CONFIG_FILE_PATH = "fsm_settings.asset";
        [ShowInInspector] public const string CONFIG_FILE_STATES = "fsm_state_settings.asset";
        [ShowInInspector] public const string CONFIG_FILE_EVENTS = "fsm_event_settings.asset";
        [ShowInInspector] public const string CONFIG_FILE_CONTROLLER = "fsm_controller_settings.asset";

#if UNITY_EDITOR
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 0)]
        [Button("State Settings")]
        private void GoToStateSettings()
        {
            MenuItem_FSMStateSettings();
        }


        [Button("Event Settings")]
        private void GoToEventSettings()
        {
            MenuItem_FSMEventSettings();
        }

        [Button("Controller Settings")]
        private void GoToControllerSettings()
        {
            MenuItem_FSMControllerSettings();
        }


        // ----------------------------------------------------------------------------------
        // ========================== MENU Items ============================
        // ----------------------------------------------------------------------------------

        [MenuItem("MyProject/FSM/Settings")]
        public static void MenuItem_FSMSettings()
        {
            MenuExtensions.PingOrCreateSO<FSMSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_PATH));
        }

        [MenuItem("MyProject/FSM/State Settings")]
        public static void MenuItem_FSMStateSettings()
        {
            MenuExtensions.PingOrCreateSO<FSMStateTypeSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_STATES));
        }

        [MenuItem("MyProject/FSM/Event Settings")]
        public static void MenuItem_FSMEventSettings()
        {
            MenuExtensions.PingOrCreateSO<FSMEventTypeSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_EVENTS));
        }

        [MenuItem("MyProject/FSM/Controller Settings")]
        public static void MenuItem_FSMControllerSettings()
        {
            MenuExtensions.PingOrCreateSO<FSMTypeSettingsSO>(Path.Combine(CONFIG_FILE_ROOT, CONFIG_FILE_CONTROLLER));
        }
#endif
    }
}
