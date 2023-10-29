using System;
using tools;
using UnityEngine;

namespace gun
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BulletController : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        private Action<BulletController> _onDestroy;

        private float _maxLifetimeSeconds = 5f;
        private DelayedCall _lifecycleDelayedCall = null;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Fire(float impulse, Action<BulletController> onDestroy = null)
        {
            _onDestroy = onDestroy;

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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            log.ELog.Log_CurrentMethod(log.ELogType.GUN);
        }

        private void OnDestroy()
        {
            if (_lifecycleDelayedCall != null)
            {
                DOTweenDelayedCall.KillDelayedCall(_lifecycleDelayedCall);
                _lifecycleDelayedCall = null;
            }

            gameObject.SetActive(false);
            _onDestroy?.Invoke(this);
        }
    }
}
