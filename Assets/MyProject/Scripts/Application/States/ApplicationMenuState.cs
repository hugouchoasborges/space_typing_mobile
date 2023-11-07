using fsm;

namespace application
{
    public class ApplicationMenuState : AbstractApplicationState
    {
        public override void OnStateEnter()
        {
            // Load Menu
            controller.LoadMainMenu();
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