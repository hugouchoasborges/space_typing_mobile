using myproject.fsm;
using UnityEngine;

namespace myproject.input
{
    [RequireComponent(typeof(InputController))]
    public class AbstractInputState : IFSMState
    {
        protected InputController inputController;

        private void Awake()
        {
            if (inputController == null)
                inputController = GetComponent<InputController>();
        }

        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch (eventType)
            {
                case FSMEventType.APPLICATION_PAUSED:
                    OnApplicationPaused(true);
                    break;

                case FSMEventType.APPLICATION_RESUMED:
                    OnApplicationPaused(false);
                    break;
            }
        }

        protected void OnApplicationPaused(bool paused)
        {
            inputController.SetPaused(paused);
        }
    }
}
