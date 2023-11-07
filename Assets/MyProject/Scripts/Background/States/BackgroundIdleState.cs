using fsm;

namespace background
{
    public class BackgroundIdleState : AbstractBackgroundState
    {
        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch (eventType)
            {
                case FSMEventType.ON_APPLICATION_MAIN_MENU:
                    controller.PlayStars();
                    controller.StopMeteors();
                    break;

                case FSMEventType.ON_APPLICATION_PLAYER_SELECTOR:
                case FSMEventType.ON_APPLICATION_GAME:
                    controller.PlayStars();
                    controller.PlayMeteors();
                    break;
            }
        }
    }
}
