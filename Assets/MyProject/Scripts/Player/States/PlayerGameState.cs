using fsm;
using input;

namespace player
{
    public class PlayerGameState : AbstractPlayerState
    {
        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch (eventType)
            {
                case FSMEventType.TOUCH_MOVED:
                    TouchInputModel input = (TouchInputModel)data;
                    controller.Move(input.Delta);
                    break;

                case FSMEventType.KEYBOARD_MOVED:
                    KeyboardInputModel keyboardInput = (KeyboardInputModel)data;
                    controller.Move(keyboardInput.Delta);
                    break;

                case FSMEventType.PLAYER_SHOOT:
                    controller.Shoot();
                    break;

                case FSMEventType.ON_APPLICATION_PAUSED:
                    GoToState(FSMStateType.PAUSED);
                    break;

                case FSMEventType.ON_APPLICATION_GAME_OVER:
                case FSMEventType.ON_APPLICATION_MAIN_MENU:
                    GoToState(FSMStateType.IDLE);
                    break;

                case FSMEventType.ON_APPLICATION_POWER_UP:
                    controller.ActivatePowerUp();
                    break;
            }
        }
    }
}
