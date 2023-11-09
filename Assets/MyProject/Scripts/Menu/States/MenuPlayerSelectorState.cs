using fsm;

namespace menu
{
    public class MenuPlayerSelectorState : AbstractMenuState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();

            controller.OpenPlayerSelector();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();

            controller.ClosePlayerSelector();
        }
    }
}
