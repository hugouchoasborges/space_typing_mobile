using core;
using enemy;
using fsm;
using player;

namespace application
{
    public class ApplicationGameState : AbstractApplicationState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.LoadGame();
        }

        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch (eventType)
            {
                case FSMEventType.ENEMY_DESTROYED:
                    controller.OnEnemyDestroyed(data as EnemyController);
                    break;

                case FSMEventType.REQUEST_GAME_OVER:
                    GoToState(FSMStateType.GAME_OVER);
                    break;

                case FSMEventType.PLAYER_DESTROYED:
                    controller.OnPlayerDestroyed(data as PlayerController);
                    break;

                case FSMEventType.REQUEST_PAUSE:
                    GoToState(FSMStateType.PAUSED);
                    break;

                case FSMEventType.REQUEST_PLAYER_COLLECT:
                    controller.OnCollectableCollected(data as UnityEngine.GameObject);
                    break;

                case FSMEventType.REQUEST_POWER_UP:
                    fsm.FSM.DispatchGameEventAll(FSMEventType.ON_APPLICATION_POWER_UP);
                    break;

                case FSMEventType.ON_APPLICATION_POWER_UP_DISABLED:
                    controller.PlayerPowerUpMultiShoot.Reset();
                    break;
            }
        }
    }
}