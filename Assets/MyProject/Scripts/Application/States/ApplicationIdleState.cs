namespace application
{
    public class ApplicationIdleState : AbstractApplicationState
    {
        public override void OnStateEnter()
        {
            controller.PlayAudioClip(sound.ESoundType.APPLICATION_GAME, volume: 0.3f, playOneShot: false, loop: true);

#if UNITY_EDITOR
            // Do not load the initialization if another scene is already loaded
            // Testing behavior
            if (ApplicationController.HackedStartupState != fsm.FSMStateType.NONE)
            {
                // Do nothing
                GoToState(ApplicationController.HackedStartupState);
                return;
            }
#endif

            // Go to Menu
            GoToState(fsm.FSMStateType.MENU);
        }
    }
}