using fsm;
using UnityEngine;

namespace background
{
    [RequireComponent(typeof(BackgroundController))]
    public class AbstractBackgroundState : IFSMState
    {
        protected BackgroundController controller;

        private void Awake()
        {
            controller = GetComponent<BackgroundController>();
        }
    }
}
