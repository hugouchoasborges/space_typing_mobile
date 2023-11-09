using fsm;
using UnityEngine;

namespace menu
{
    [RequireComponent(typeof(MenuController))]
    public class AbstractMenuState : IFSMState
    {
        protected MenuController controller;

        private void Awake()
        {
            controller = GetComponent<MenuController>();
        }

        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch (eventType)
            {
                case FSMEventType.ON_APPLICATION_RESUMED:
                case FSMEventType.ON_APPLICATION_GAME:
                    GoToState(FSMStateType.GAME);
                    break;

                case FSMEventType.ON_APPLICATION_PLAYER_SELECTOR:
                    GoToState(FSMStateType.PLAYER_SELECTOR);
                    break;

                case FSMEventType.ON_APPLICATION_MAIN_MENU:
                    GoToState(FSMStateType.MENU);
                    break;

                case FSMEventType.ON_APPLICATION_GAME_OVER:
                    GoToState(FSMStateType.GAME_OVER);
                    break;

                case FSMEventType.ON_APPLICATION_PAUSED:
                    GoToState(FSMStateType.PAUSED);
                    break;
            }
        }
    }
}
