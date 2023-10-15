namespace bootstrap
{
    class BootstrapIdleState : AbstractBootstrapState
    {
        public override void OnStateEnter()
        {
            initialization.Init();
        }
    }
}
