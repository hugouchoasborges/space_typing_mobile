using fsm;
using gun.settings;
using gun;

namespace enemy
{
    public class EnemyPrepare : AbstractEnemyState
    {
        public override void OnStateEnter()
        {

            GunController enemyGun = Instantiate(GunSettingsSO.Instance.EnemyGun).GetComponent<GunController>();
            controller.AddGun(enemyGun);

            GoToState(FSMStateType.IDLE);
        }
    }
}
