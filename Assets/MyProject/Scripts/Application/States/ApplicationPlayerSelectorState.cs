using fsm;

namespace application
{
    public class ApplicationPlayerSelectorState : AbstractApplicationState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.LoadPlayerSelector();
        }

        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch (eventType)
            {
                case FSMEventType.REQUEST_PLAY:
                    GoToState(FSMStateType.GAME);
                    break;
            }
        }
    }
}