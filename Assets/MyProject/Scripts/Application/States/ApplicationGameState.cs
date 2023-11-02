using enemy;
using fsm;

namespace application
{
    public class ApplicationGameState : AbstractApplicationState
    {
        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch (eventType)
            {
                case FSMEventType.ENEMY_DESTROYED:
                    applicationController.OnEnemyDestroyed(data as EnemyController);
                    break;
            }
        }
    }
}