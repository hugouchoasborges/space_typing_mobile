using fsm;
using UnityEngine;

namespace gun
{
    [RequireComponent(typeof(GunController))]
    public class AbstractGunState : IFSMState
    {
        protected GunController controller;

        private void Awake()
        {
            controller = GetComponent<GunController>();
        }
    }
}
