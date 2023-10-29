using Sirenix.OdinInspector;
using tools;

namespace fsm.settings
{
    class FSMStateTypeSettingsSO : CustomEnumSO
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
