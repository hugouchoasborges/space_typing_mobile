using fsm;

namespace menu
{
    public class MenuGameOverState : AbstractMenuState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();

            controller.OpenGameOver();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();

            controller.CloseGameOver();
        }
    }
}
