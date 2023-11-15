using core;
using gun;
using System.Collections.Generic;
using tools;
using UnityEngine;

namespace player
{
    public class PlayerController : MonoBehaviour
    {
        private List<GunController> _guns = new List<GunController>();
        private GunController _currentGun => _guns.Count == 0 ? null : _guns[0];

        [Header("Components")]
        [SerializeField] private Transform _gunSlot;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            SetCollisionMasks(_enemyCollisionMask, _collectableCollisionMask);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Initialization ============================
        // ----------------------------------------------------------------------------------

        public void AddGun(params GunController[] guns)
        {
            _guns.AddRange(guns);
            foreach (var gun in guns)
            {
                gun.transform.SetParent(_gunSlot, false);
                gun.OnTargetHitCallback = OnTargetHit;
            }
        }


        // ----------------------------------------------------------------------------------
        // ========================== Skins ============================
        // ----------------------------------------------------------------------------------

        public void ApplySkin(Sprite skin)
        {
            _spriteRenderer.sprite = skin;
        }


        // ----------------------------------------------------------------------------------
        // ========================== Callbacks\Events ============================
        // ----------------------------------------------------------------------------------

        [Header("Collision")]
        [SerializeField] private LayerMask _enemyCollisionMask;
        private List<int> _enemyCollisionMaskLayers;

        [SerializeField] private LayerMask _collectableCollisionMask;
        private List<int> _collectableCollisionMaskLayers;

        public void SetCollisionMasks(LayerMask enemyMask, LayerMask collectableMask)
        {
            _enemyCollisionMask = enemyMask;
            _enemyCollisionMaskLayers = _enemyCollisionMask.GetMaskIndexes() as List<int>;

            _collectableCollisionMask = collectableMask;
            _collectableCollisionMaskLayers = _collectableCollisionMask.GetMaskIndexes() as List<int>;
        }

        private void OnTargetHit(GameObject target)
        {
            // MEDO: Calculate points earned based com a PointSystemSO configuration page
            // GetPointsForEnemy(target)

            // MEDO: Update GUI
            // Locator.ApplicationController.UpdateGUI();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_enemyCollisionMaskLayers.Contains(collision.gameObject.layer))
            {
                // Collided with enemy
                OnPlayerHit();
            }
            else if (_collectableCollisionMaskLayers.Contains(collision.gameObject.layer))
            {
                OnPlayerCollect(collision.gameObject);
            }

        }

        private void OnPlayerHit()
        {
            // MEDO: Play Particles

            // MEDO: Destroy player
            fsm.FSM.DispatchGameEventAll(fsm.FSMEventType.PLAYER_DESTROYED, this);
            Locator.ApplicationController.PlayAudioClip(sound.ESoundType.PLAYER_DEATH);
        }

        private void OnPlayerCollect(GameObject gObj)
        {
            fsm.FSM.DispatchGameEventAll(fsm.FSMEventType.REQUEST_PLAYER_COLLECT, gObj);
            Locator.ApplicationController.PlayAudioClip(sound.ESoundType.PLAYER_COLLECT);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Controls ============================
        // ----------------------------------------------------------------------------------


        // ========================== Movement ============================


        public void Move(Vector2 delta)
        {
            transform.position += (Vector3)delta;
        }


        // ========================== Shooting ============================

        public void ActivatePowerUp()
        {
            _currentGun?.ActivatePowerUp();
            Locator.ApplicationController.PlayAudioClip(sound.ESoundType.PLAYER_POWERUP);
        }

        public void Shoot()
        {
            if (_currentGun != null && _currentGun.Fire())
                Locator.ApplicationController.PlayAudioClip(sound.ESoundType.PLAYER_SHOOT);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Pause System ============================
        // ----------------------------------------------------------------------------------

        public void OnPaused()
        {
            _currentGun?.OnPaused(true);
        }

        public void OnResumed()
        {
            _currentGun?.OnPaused(false);
        }
    }
}