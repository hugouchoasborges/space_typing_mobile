using fsm;

namespace menu
{
    public class MenuMainState : AbstractMenuState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();

            controller.OpenMainMenu();
        }

        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch (eventType)
            {
                case FSMEventType.ON_APPLICATION_GAME:
                    GoToState(FSMStateType.GAME);
                break;

                case FSMEventType.ON_APPLICATION_PLAYER_SELECTOR:
                    GoToState(FSMStateType.PLAYER_SELECTOR);
                    break;
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();

            controller.CloseMainMenu();
        }
    }
}
