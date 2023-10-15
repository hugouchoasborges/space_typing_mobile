using myproject.fsm;

namespace bootstrap
{
    class BootstrapPrepareState : AbstractBootstrapState
    {
        public override void OnStateEnter()
        {
            // Prepare Stuff

            GoToState(FSMStateType.IDLE);
        }
    }
}
