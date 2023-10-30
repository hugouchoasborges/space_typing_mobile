using log;
using Sirenix.OdinInspector;
using System;
using tools;
using UnityEngine;
using utils;

namespace gun
{
    public class GunController : MonoBehaviour
    {
        [ShowInInspector] private PoolController<BulletController> _bullets;

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

            _bullets = new PoolController<BulletController>(_bullet);
        }

        /// <summary>
        /// Set the current bullet to a new one
        /// </summary>
        public void SetBullet(GameObject bullet)
        {
            _bullet = bullet;
            _bullets?.UpdatePrefab(_bullet);
        }

        // ========================== Fire ============================

        public void Fire()
        {
            // Control fire rate
            if (!CanFire())
                return;

            //ELog.Log_CurrentMethod(ELogType.GUN);

            // Get Bullet from pool
            BulletController bullet = _bullets.Dequeue();

            //bullet.transform.SetParent(transform, false);
            bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
            bullet.Fire(_impulse / 100f, OnTargetHit, OnBulletDestroyed);
            OnFire();
        }

        private void OnTargetHit(GameObject target)
        {
            OnTargetHitCallback?.Invoke(target);
        }

        private void OnBulletDestroyed(BulletController bullet)
        {
            _bullets.Enqueue(bullet);
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
