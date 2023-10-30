using fsm;

namespace enemy
{
    public class EnemyPrepare : AbstractEnemyState
    {
        public override void OnStateEnter()
        {
            GoToState(FSMStateType.IDLE);
        }
    }
}
