using fsm;

namespace menu
{
    public class MenuIdleState : AbstractMenuState
    {
        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch (eventType)
            {
                case FSMEventType.ON_APPLICATION_MAIN_MENU:
                    GoToState(FSMStateType.MENU);
                    break;

                case FSMEventType.ON_APPLICATION_PLAYER_SELECTOR:
                    GoToState(FSMStateType.PLAYER_SELECTOR);
                    break;
            }
        }
    }
}
