using core;
using gun;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using tools;
using UnityEngine;

namespace enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        [Header("Gun")]
        [SerializeField] private bool _canHaveGun = false;
        [SerializeField] private Transform _gunSlot;

        [Header("Shooting")]
        [MinMaxSlider(0.1f, 3f, true)]
        [SerializeField] private Vector2 _shootDelaySec = new Vector2(0.5f, 2f);
        [MinMaxSlider(0f, 1f, true)]
        [SerializeField] private Vector2 _shootProbability = new Vector2(0.2f, 0.7f);

        private List<GunController> _guns = new List<GunController>();
        private GunController _currentGun => _guns.Count == 0 ? null : _guns[0];

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            SetCollisionMask(_collisionMask);
        }

        private void OnDisable()
        {
            StopShooting();
        }


        // ----------------------------------------------------------------------------------
        // ========================== Initialization ============================
        // ----------------------------------------------------------------------------------

        public void AddGun(params GameObject[] gunPrefabs)
        {
            if (!_canHaveGun) return;

            List<GunController> newGuns = new List<GunController>();

            foreach (var prefab in gunPrefabs)
            {
                GunController newGun = Instantiate(prefab).GetComponent<GunController>();
                newGuns.Add(newGun);
                newGun.transform.SetParent(_gunSlot, false);
            }

            _guns.AddRange(newGuns);

            StartShooting();
        }


        // ----------------------------------------------------------------------------------
        // ========================== Shooting ============================
        // ----------------------------------------------------------------------------------

        DelayedCall _shootingDelayedCall;

        private void StartShooting()
        {
            StopShooting();

            _shootingDelayedCall = DOTweenDelayedCall.DelayedCall(Shoot, _shootDelaySec.GetRandomFloat(), loops: -1);
        }

        private void StopShooting()
        {
            if (_shootingDelayedCall != null)
            {
                DOTweenDelayedCall.KillDelayedCall(_shootingDelayedCall);
                _shootingDelayedCall = null;
            }
        }

        public void Shoot()
        {
            if (Random.Range(0f, 1f) > _shootProbability.GetRandomFloat()) return;

            if (_currentGun != null && _currentGun.Fire())
                Locator.ApplicationController.PlayAudioClip(sound.ESoundType.ENEMY_SHOOT, volume: .2f);
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
            StopShooting();
            Locator.ApplicationController.PlayAudioClip(sound.ESoundType.ENEMY_DEATH);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Movement ============================
        // ----------------------------------------------------------------------------------

        public void SetActive(bool active)
        {
            // Pause/Resume movement
            _rigidbody2D.simulated = active;

            // Pause/Resume guns
            _currentGun?.OnPaused(!active);

            if (active && gameObject.activeInHierarchy)
                StartShooting();
            else
                StopShooting();
        }

        public void SetImpulse(Vector2 direction)
        {
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.angularVelocity = 0;
            _rigidbody2D.AddForce(direction, ForceMode2D.Impulse);
        }
    }
}
