using fsm;

namespace player
{
    public class PlayerPauseState : AbstractPlayerState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();

            controller.OnPaused();
        }

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

                case FSMEventType.ON_APPLICATION_GAME_OVER:
                case FSMEventType.ON_APPLICATION_MAIN_MENU:
                    GoToState(FSMStateType.IDLE);
                    break;
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();

            controller.OnResumed();
        }
    }
}
