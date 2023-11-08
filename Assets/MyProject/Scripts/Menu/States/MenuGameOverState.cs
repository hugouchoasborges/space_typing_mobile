using fsm;

namespace menu
{
    public class MenuGameOverState : AbstractMenuState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();

            controller.OpenGameOver();
        }

        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch (eventType)
            {
                case FSMEventType.ON_APPLICATION_GAME:
                    GoToState(FSMStateType.GAME);
                    break;

                case FSMEventType.ON_APPLICATION_MAIN_MENU:
                    GoToState(FSMStateType.MENU);
                    break;

                case FSMEventType.ON_APPLICATION_PLAYER_SELECTOR:
                    GoToState(FSMStateType.PLAYER_SELECTOR);
                    break;
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();

            controller.CloseGameOver();
        }
    }
}
