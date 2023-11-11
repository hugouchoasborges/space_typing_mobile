using enemy;
using log;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using tools;
using UnityEngine;
using utils;

namespace gun
{
    public class GunController : MonoBehaviour
    {
        [ShowInInspector] private PoolController<BulletController> _queuedBullets;
        [ShowInInspector] private List<BulletController> _activeBullets;

        [Header("Settings")]
        [SerializeField][Range(1f, 20f)] private float _impulse = 5f;
        [SerializeField][Range(0.5f, 20f)] private float _bulletsPerSecond = 2;
        [SerializeField] private GameObject _bullet;

        private bool _fireRateLock = false;
        private DelayedCall _resetFireRateDelayedCall = null;

        public Action<GameObject> OnTargetHitCallback;

        private void Start()
        {
            if (_bullet == null)
            {
                ELog.LogError(ELogType.GUN, "No Bullet set. Using default one");
                SetBullet(settings.GunSettingsSO.Instance.BulletDefault);
            }

            _queuedBullets = new PoolController<BulletController>(_bullet);
            _activeBullets = new List<BulletController>();
        }

        /// <summary>
        /// Set the current bullet to a new one
        /// </summary>
        public void SetBullet(GameObject bullet)
        {
            _bullet = bullet;
            _queuedBullets?.UpdatePrefab(_bullet);
        }


        // ========================== Power Ups ============================

        [Header("Power-Up")]
        private bool _powerUp = false;
        private const int POWER_UP_MAX_COUNTDOWN = 15;
        private int _powerUpCountdown = POWER_UP_MAX_COUNTDOWN;

        public void ActivatePowerUp()
        {
            if (_powerUp) return;

            _powerUp = true;
            _powerUpCountdown = POWER_UP_MAX_COUNTDOWN;

            DOTweenDelayedCall.DelayedCall(PowerUpCountDown, 1.0f, loops: -1);
        }

        private void DeactivatePowerUp()
        {
            _powerUp = false;
            DOTweenDelayedCall.KillDelayedCall(PowerUpCountDown);
            fsm.FSM.DispatchGameEventAll(fsm.FSMEventType.ON_APPLICATION_POWER_UP_DISABLED);
        }

        private void PowerUpCountDown()
        {
            _powerUpCountdown--;

            if (_powerUpCountdown <= 0)
            {
                DeactivatePowerUp();
            }

            fsm.FSM.DispatchGameEventAll(fsm.FSMEventType.ON_APPLICATION_POWER_UP_COUNTDOWN, (float)_powerUpCountdown / POWER_UP_MAX_COUNTDOWN);
        }

        // ========================== Fire ============================

        public void Fire()
        {
            // Control fire rate
            if (!CanFire())
                return;

            //ELog.Log_CurrentMethod(ELogType.GUN);

            if (_powerUp)
            {
                // Get Bullet from pool
                BulletController[] bullets = _queuedBullets.Dequeue(3);
                Vector2[] directions = { transform.up, new Vector2(-.5f, .5f).normalized, new Vector2(.5f, .5f).normalized };
                _activeBullets.AddRange(bullets);

                for (int i = 0; i < bullets.Length; i++)
                {
                    BulletController bullet = bullets[i];
                    Vector2 direction = directions[i];
                    Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

                    bullet.transform.SetPositionAndRotation(transform.position, rotation);
                    bullet.Fire(_impulse / 150f, direction, OnTargetHit, OnBulletDestroyed);
                }
            }
            else
            {
                // Get Bullet from pool
                BulletController bullet = _queuedBullets.Dequeue();
                _activeBullets.Add(bullet);

                //bullet.transform.SetParent(transform, false);
                bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
                bullet.Fire(_impulse / 100f, OnTargetHit, OnBulletDestroyed);
            }

            OnFire();
        }

        private void OnTargetHit(GameObject target)
        {
            OnTargetHitCallback?.Invoke(target);
        }

        private void OnBulletDestroyed(BulletController bullet)
        {
            _queuedBullets.Enqueue(bullet);
            _activeBullets.Remove(bullet);
        }

        private void OnFire()
        {
            // Fire Rate lock
            StartFireRateLockTimer();

            // MEDO: Fire animation (recoil + smoke particles)

            // MEDO: Update Bullets count
        }


        // ========================== Fire Rate ============================
        private bool CanFire()
        {
            return !_fireRateLock;
        }

        private void StartFireRateLockTimer()
        {
            _fireRateLock = true;
            DOTweenDelayedCall.DelayedCall(ResetFireRateTimer, 1f / _bulletsPerSecond);
        }

        private void ResetFireRateTimer()
        {
            if (_resetFireRateDelayedCall != null)
            {
                DOTweenDelayedCall.KillDelayedCall(_resetFireRateDelayedCall);
                _resetFireRateDelayedCall = null;
            }

            _fireRateLock = false;
        }


        // ----------------------------------------------------------------------------------
        // ========================== Pause Control ============================
        // ----------------------------------------------------------------------------------

        public void OnPaused(bool paused)
        {
            List<BulletController> allBullets = _activeBullets + _queuedBullets as List<BulletController>;
            for (int i = allBullets.Count - 1; i >= 0; i--)
            {
                OnPaused(allBullets[i], paused);
            }
        }

        public void OnPaused(BulletController bullet, bool paused)
        {
            // Stop/Resume bullet movement
            bullet.SetMovementActive(!paused);

            // Stop/Resume bullet lifecycle
            bullet.SetLifecycleActive(!paused);

            // Stop/Resume bullet particles and trail
            bullet.SetParticlesActive(!paused);

        }

        // ----------------------------------------------------------------------------------
        // ========================== Editor Stuff ============================
        // ----------------------------------------------------------------------------------


#if UNITY_EDITOR
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 0)]
        [Button("Gun Settings")]
        public void GoToSettings()
        {
            settings.GunSettingsSO.MenuItem_SceneSettings();
        }
#endif
    }
}
