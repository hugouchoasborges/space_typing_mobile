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

        public override void OnStateExit()
        {
            base.OnStateExit();

            controller.CloseMainMenu();
        }
    }
}
