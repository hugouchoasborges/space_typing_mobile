using log;
using myproject.fsm;
using myproject.input;

namespace myproject.player
{
    public class PlayerIdleState : AbstractPlayerState
    {
        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch(eventType)
            {
                case FSMEventType.TOUCH_MOVED:
                    TouchInputModel input = (TouchInputModel)data;
                    controller.Move(input.Delta);
                    break;

                case FSMEventType.KEYBOARD_MOVED:
                    KeyboardInputModel keyboardInput = (KeyboardInputModel)data;
                    controller.Move(keyboardInput.Delta);
                    break;
            }
        }
    }
}
