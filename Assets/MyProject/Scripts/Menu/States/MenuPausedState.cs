using fsm;

namespace menu
{
    public class MenuPausedState : AbstractMenuState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();

            controller.OpenPause();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();

            controller.ClosePause();
        }
    }
}
