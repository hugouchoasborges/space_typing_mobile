using fsm;
using gun.settings;

namespace enemy
{
    public class EnemyPrepare : AbstractEnemyState
    {
        public override void OnStateEnter()
        {

            controller.AddGun(GunSettingsSO.Instance.EnemyGun);

            GoToState(FSMStateType.IDLE);
        }
    }
}
