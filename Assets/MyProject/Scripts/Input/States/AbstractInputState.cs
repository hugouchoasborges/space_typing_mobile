using fsm;
using UnityEngine;

namespace input
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
                case FSMEventType.ON_APPLICATION_PAUSED:
                    OnApplicationPaused(true);
                    break;

                case FSMEventType.ON_APPLICATION_RESUMED:
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
