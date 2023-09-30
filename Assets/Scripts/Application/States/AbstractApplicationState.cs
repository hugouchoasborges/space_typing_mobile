using myproject.fsm;
using UnityEngine;

namespace application
{
    [RequireComponent(typeof(ApplicationController))]
    public class AbstractApplicationState : IFSMState
    {
        protected ApplicationController applicationController;

        private void Awake()
        {
            if (applicationController == null)
            {
                applicationController = GetComponent<ApplicationController>();
            }
        }
    }
}
