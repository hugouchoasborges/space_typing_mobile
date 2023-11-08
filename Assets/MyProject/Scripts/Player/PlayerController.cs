using gun;
using log;
using System.Collections.Generic;
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
            SetCollisionMask(_collisionMask);
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
        [SerializeField] private LayerMask _collisionMask;
        private LayerMask _collisionMaskLayer;

        public void SetCollisionMask(LayerMask mask)
        {
            _collisionMask = mask;
            _collisionMaskLayer = (int)Mathf.Log(_collisionMask.value, 2);
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
            if (collision.gameObject.layer != _collisionMaskLayer) return;

            OnPlayerHit();
        }

        private void OnPlayerHit()
        {
            // MEDO: Play Particles

            // MEDO: Destroy player
            fsm.FSM.DispatchGameEvent(fsm.FSMControllerType.ALL, fsm.FSMStateType.ALL, fsm.FSMEventType.PLAYER_DESTROYED, this);
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

        public void Shoot()
        {
            _currentGun?.Fire();
        }
    }
}