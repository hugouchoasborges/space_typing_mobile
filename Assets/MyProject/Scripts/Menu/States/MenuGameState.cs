using fsm;

namespace menu
{
    public class MenuGameState : AbstractMenuState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }

        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch (eventType)
            {
                case FSMEventType.ON_APPLICATION_GAME_OVER:
                    GoToState(FSMStateType.GAME_OVER);
                    break;
            }
        }
    }
}
