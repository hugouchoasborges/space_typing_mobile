using Sirenix.OdinInspector;
using tools;

namespace myproject.fsm.settings
{
    class FSMEventTypeSettingsSO : CustomEnumSO
    {

#if UNITY_EDITOR
        [Button("FSM Settings")]
        private void GoToFSMSettings()
        {
            FSMSettingsSO.MenuItem_FSMSettings();
        }
#endif
    }
}
