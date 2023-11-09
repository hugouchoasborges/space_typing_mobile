using fsm;

namespace bootstrap
{
    class BootstrapPrepareState : AbstractBootstrapState
    {
        public override void OnStateEnter()
        {
            // Prepare Stuff

#if UNITY_EDITOR
            // Reset Scene loading hack(Editor Only)
            application.ApplicationController.HackedStartupState = FSMStateType.NONE;
#endif

            GoToState(FSMStateType.IDLE);
        }
    }
}
