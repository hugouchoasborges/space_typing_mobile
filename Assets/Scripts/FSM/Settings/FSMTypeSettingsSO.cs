using Sirenix.OdinInspector;
using tools;

namespace myproject.fsm.settings
{
    class FSMTypeSettingsSO : CustomEnumSO
    {
        public override string[] DEFAULT_ADDITIONAL_ENTRIES => new string[] { "ALL" };

#if UNITY_EDITOR
        [Button("FSM Settings")]
        private void GoToFSMSettings()
        {
            FSMSettingsSO.MenuItem_FSMSettings();
        }
#endif
    }
}
