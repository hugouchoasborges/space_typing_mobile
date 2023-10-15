using myproject.fsm;
using UnityEngine;

namespace myproject.player
{
    [RequireComponent(typeof(PlayerController))]
    public class AbstractPlayerState : IFSMState
    {
        protected PlayerController controller;

        private void Awake()
        {
            controller = GetComponent<PlayerController>();
        }
    }
}
