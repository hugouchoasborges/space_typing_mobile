namespace application
{
    public class ApplicationMenuState : AbstractApplicationState
    {
        public override void OnStateEnter()
        {
            // Load Menu
            applicationController.LoadMainMenu();
        }
    }
}