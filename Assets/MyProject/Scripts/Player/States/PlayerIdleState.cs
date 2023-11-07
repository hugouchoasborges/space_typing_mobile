using fsm;

namespace player
{
    public class PlayerIdleState : AbstractPlayerState
    {
        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch (eventType)
            {
                case FSMEventType.ON_APPLICATION_RESUMED:
                case FSMEventType.ON_APPLICATION_GAME:
                    GoToState(FSMStateType.GAME);
                    break;

                case FSMEventType.ON_APPLICATION_PLAYER_SELECTOR:
                    GoToState(FSMStateType.PLAYER_SELECTOR);
                    break;
            }
        }
    }
}
