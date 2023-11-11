using log;
using System.Collections.Generic;
using tools;
using UnityEngine;

namespace enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            SetCollisionMask(_collisionMask);
        }


        // ----------------------------------------------------------------------------------
        // ========================== Collision ============================
        // ----------------------------------------------------------------------------------

        [Header("Collision")]
        [SerializeField] private LayerMask _collisionMask;
        private List<int> _collisionMaskLayers;

        public void SetCollisionMask(LayerMask mask)
        {
            _collisionMask = mask;
            _collisionMaskLayers = _collisionMask.GetMaskIndexes() as List<int>;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_collisionMaskLayers.Contains(collision.gameObject.layer)) return;

            OnHit();
        }

        private void OnHit()
        {
            // MEDO: Particles

            gameObject.SetActive(false);

            // Dispatch event, so:
            // * Points are added to the player
            // * Spawner can recycle this enemy (pool)
            fsm.FSM.DispatchGameEventAll(fsm.FSMEventType.ENEMY_DESTROYED, this);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Movement ============================
        // ----------------------------------------------------------------------------------

        public void SetMovementActive(bool active)
        {
            _rigidbody2D.simulated = active;
        }

        public void SetImpulse(Vector2 direction)
        {
            _rigidbody2D.AddForce(direction, ForceMode2D.Impulse);
        }
    }
}
