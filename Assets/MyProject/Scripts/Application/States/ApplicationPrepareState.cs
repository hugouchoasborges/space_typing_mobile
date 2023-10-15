using myproject.fsm;
using UnityEngine;

namespace application
{
    public class ApplicationPrepareState : AbstractApplicationState
    {
        [Header("Quality Settings")]
        [Range(1, 120), SerializeField] private int _targetFPS = 60;

        public override void OnStateEnter()
        {
            ApplyQualitySettings();
            GoToState(FSMStateType.IDLE);
        }

        private void ApplyQualitySettings()
        {
            Application.targetFrameRate = _targetFPS;
        }
    }
}