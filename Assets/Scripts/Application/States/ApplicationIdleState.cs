using scenes;

namespace application
{
    public class ApplicationIdleState : AbstractApplicationState
    {
        public override void OnStateEnter()
        {
#if UNITY_EDITOR
            // Do not load the initialization if another scene is already loaded
            // Testing behavior
            if (SceneHelper.SceneCount() > 1)
            {
                // Do nothing
                return;
            }
#endif

            // Go to Menu
            GoToState(fsm.FSMStateType.MENU);
        }
    }
}