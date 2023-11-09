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

        public override void OnStateExit()
        {
            base.OnStateExit();

            controller.CloseGameUI();
        }
    }
}
