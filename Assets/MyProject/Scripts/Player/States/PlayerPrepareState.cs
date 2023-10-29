using fsm;
using gun;
using gun.settings;

namespace player
{
    public class PlayerPrepare : AbstractPlayerState
    {
        public override void OnStateEnter()
        {
            InitializePlayer();
            GoToState(FSMStateType.IDLE);
        }

        private void InitializePlayer()
        {
            // Instantiate GUN
            // MEDO: Do this by using a GunManufacturer <pool> structure

            GunController defaultGun = Instantiate(GunSettingsSO.Instance.GunDefault).GetComponent<GunController>();
            controller.AddGun(defaultGun);
        }
    }
}
