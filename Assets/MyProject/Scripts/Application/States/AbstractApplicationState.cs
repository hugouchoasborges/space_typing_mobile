using fsm;
using UnityEngine;

namespace application
{
    [RequireComponent(typeof(ApplicationController))]
    public class AbstractApplicationState : IFSMState
    {
        protected ApplicationController controller;

        private void Awake()
        {
            if (controller == null)
            {
                controller = GetComponent<ApplicationController>();
            }
        }
    }
}
