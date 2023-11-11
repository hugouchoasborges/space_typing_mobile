using fsm;

namespace menu
{
    public class MenuGameState : AbstractMenuState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();

            controller.OpenGameUI();
        }

        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch (eventType)
            {
                case FSMEventType.ON_APPLICATION_POWER_UP_COUNTDOWN:
                    controller.SetPowerUpButtonLoadPercentage((float)data);
                    break;

                case FSMEventType.ON_APPLICATION_POWER_UP_DISABLED:
                    controller.SetPowerUpButtonInteractable(false);
                    break;
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();

            controller.CloseGameUI();
        }
    }
}
