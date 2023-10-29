namespace input
{
    public class InputIdleState : AbstractInputState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();

            inputController.SetPaused(false);
        }
    }
}
