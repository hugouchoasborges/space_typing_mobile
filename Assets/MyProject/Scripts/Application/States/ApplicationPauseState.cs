using core;
using fsm;

namespace application
{
    public class ApplicationPauseState : AbstractApplicationState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();

            // Load Pause menu
            controller.LoadPause();

            Locator.ApplicationController.PlayAudioClip(sound.ESoundType.APPLICATION_PAUSED);
        }

        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch (eventType)
            {
                case FSMEventType.REQUEST_RESUME:
                    fsm.FSM.DispatchGameEventAll(FSMEventType.ON_APPLICATION_RESUMED);
                    GoToState(FSMStateType.GAME);
                    break;

                case FSMEventType.REQUEST_MAIN_MENU:
                    GoToState(FSMStateType.MENU);
                    break;
            }
        }
    }
}