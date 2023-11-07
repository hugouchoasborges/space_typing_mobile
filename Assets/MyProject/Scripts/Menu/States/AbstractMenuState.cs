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
    }
}
