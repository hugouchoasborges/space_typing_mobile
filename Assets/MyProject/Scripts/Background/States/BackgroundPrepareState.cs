using fsm;

namespace background
{
    public class BackgroundPrepare : AbstractBackgroundState
    {
        public override void OnStateEnter()
        {
            GoToState(FSMStateType.IDLE);
        }
    }
}
