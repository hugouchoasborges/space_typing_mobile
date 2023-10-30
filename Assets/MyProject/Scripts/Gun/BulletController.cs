using System;
using tools;
using UnityEngine;

namespace gun
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BulletController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private LayerMask _collisionMask;
        private LayerMask _collisionMaskLayer;

        private Rigidbody2D _rigidbody2D;
        private Action<BulletController> _onDestroy;
        private Action<GameObject> _onTargetHit;

        private float _maxLifetimeSeconds = 3f;
        private DelayedCall _lifecycleDelayedCall = null;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            SetCollisionMask(_collisionMask);
        }

        public void SetCollisionMask(LayerMask mask)
        {
            _collisionMask = mask;
            _collisionMaskLayer = (int)Mathf.Log(_collisionMask.value, 2);
        }

        public void Fire(float impulse, Action<GameObject> onTargetHit = null, Action<BulletController> onDestroy = null)
        {
            _onDestroy = onDestroy;
            _onTargetHit = onTargetHit;

            gameObject.SetActive(true);
            _rigidbody2D.AddForce(transform.up * impulse, ForceMode2D.Impulse);

            OnFire();
        }

        private void OnFire()
        {
            // MEDO: Particles

            // Start lifetime cycle
            _lifecycleDelayedCall = DOTweenDelayedCall.DelayedCall(OnDestroy, _maxLifetimeSeconds);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != _collisionMaskLayer) return;

            OnHit(collision.gameObject);
        }

        private void OnHit(GameObject target)
        {
            log.ELog.Log_CurrentMethod(log.ELogType.BULLET);

            // Callback
            _onTargetHit?.Invoke(target);

            // MEDO: Spawn Particles
            OnDestroy();
        }

        private void OnDestroy()
        {
            if (_lifecycleDelayedCall != null)
            {
                DOTweenDelayedCall.KillDelayedCall(_lifecycleDelayedCall);
                _lifecycleDelayedCall = null;
            }

            gameObject.SetActive(false);

            // Reset Physics
            _rigidbody2D.velocity = Vector3.zero;
            _rigidbody2D.angularVelocity = 0;

            // Reset Transform
            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            transform.position = Vector3.zero;

            // Callback?
            _onDestroy?.Invoke(this);

            // Remove callbacks
            _onDestroy = null;
            _onTargetHit = null;
        }
    }
}
