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
                    inputController.SetPaused(true);
                    break;

                case FSMEventType.ON_APPLICATION_GAME:
                case FSMEventType.ON_APPLICATION_RESUMED:
                    inputController.SetPaused(false);
                    break;
            }
        }
    }
}
