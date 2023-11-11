using System;
using System.Collections.Generic;
using tools;
using UnityEngine;

namespace gun
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BulletController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private LayerMask _collisionMask;
        private List<int> _collisionMaskLayers;

        [Header("Components")]
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private TrailRenderer _trailRenderer;

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
            _collisionMaskLayers = _collisionMask.GetMaskIndexes() as List<int>;
        }

        public void Fire(float impulse, Action<GameObject> onTargetHit = null, Action<BulletController> onDestroy = null)
        {
            Fire(impulse, transform.up, onTargetHit: onTargetHit, onDestroy: onDestroy);
        }

        public void Fire(float impulse, Vector2 direction, Action<GameObject> onTargetHit = null, Action<BulletController> onDestroy = null)
        {
            _onDestroy = onDestroy;
            _onTargetHit = onTargetHit;

            gameObject.SetActive(true);
            _rigidbody2D.AddForce(direction * impulse, ForceMode2D.Impulse);

            OnFire();
        }

        private void OnFire()
        {
            // Clears trail -- Fix bug
            _trailRenderer.Clear();
            _trailRenderer.emitting = true;

            // Start lifetime cycle
            _lifecycleDelayedCall = DOTweenDelayedCall.DelayedCall(OnDestroy, _maxLifetimeSeconds);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_collisionMaskLayers.Contains(collision.gameObject.layer)) return;

            OnHit(collision.gameObject);
        }

        private void OnHit(GameObject target)
        {
            // Callback
            _onTargetHit?.Invoke(target);

            // MEDO: Spawn Particles

            // Destroy this bullet
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

            // Disable trail
            _trailRenderer.emitting = false;

            // Reset Transform
            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            transform.position = Vector3.zero;

            // Callback?
            _onDestroy?.Invoke(this);

            // Remove callbacks
            _onDestroy = null;
            _onTargetHit = null;
        }


        // ----------------------------------------------------------------------------------
        // ========================== Pause System ============================
        // ----------------------------------------------------------------------------------

        public void SetMovementActive(bool active)
        {
            _rigidbody2D.simulated = active;
        }

        public void SetLifecycleActive(bool active)
        {
            _lifecycleDelayedCall?.SetPaused(!active);
        }

        public void SetParticlesActive(bool active)
        {
            if (!active)
            {
                _particles.Pause();
            }
            else
            {
                _particles.Play();
            }
            // MEDO: Freeze trail
        }
    }
}
