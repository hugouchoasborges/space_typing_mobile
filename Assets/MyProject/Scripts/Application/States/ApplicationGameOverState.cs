using core;
using enemy;
using fsm;

namespace application
{
    public class ApplicationGameOverState : AbstractApplicationState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.LoadGameOver();

            Locator.ApplicationController.PlayAudioClip(sound.ESoundType.APPLICATION_GAMEOVER);
        }

        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch (eventType)
            {
                case FSMEventType.ENEMY_DESTROYED:
                    controller.OnEnemyDestroyed(data as EnemyController);
                    break;

                case FSMEventType.REQUEST_MAIN_MENU:
                    GoToState(FSMStateType.MENU);
                    break;

                case FSMEventType.REQUEST_PLAYER_SELECTOR:
                    GoToState(FSMStateType.PLAYER_SELECTOR);
                    break;

                case FSMEventType.REQUEST_PLAY:
                    GoToState(FSMStateType.GAME);
                    break;
            }
        }
    }
}