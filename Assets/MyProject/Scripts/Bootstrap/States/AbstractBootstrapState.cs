using myproject.fsm;

namespace bootstrap
{
    class AbstractBootstrapState : IFSMState
    {
        protected Initialization initialization;

        private void Awake()
        {
            initialization = GetComponent<Initialization>();
        }
    }
}
