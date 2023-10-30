using fsm;
using UnityEngine;

namespace enemy
{
    [RequireComponent(typeof(EnemyController))]
    public class AbstractEnemyState : IFSMState
    {
        protected EnemyController controller;

        private void Awake()
        {
            controller = GetComponent<EnemyController>();
        }
    }
}
