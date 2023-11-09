using enemy;
using enemy.settings;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using tools;
using UnityEngine;
using utils;

namespace application
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Bounds _spawnArea = new Bounds(Vector3.up * 35, new Vector3(20, 2, 0));
        private DelayedCall _spawnDelayedCall = null;
        [SerializeField] private Bounds _destroyArea = new Bounds(Vector3.up * (-5), new Vector3(30, 50, 0));

        private PoolController<EnemyController> _queuedEnemies;
        private List<EnemyController> _activeEnemies = new List<EnemyController>();

        public int EnemiesActive => _activeEnemies.Count;

        private bool _initialized = false;

        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            _queuedEnemies = new PoolController<EnemyController>(prefab: EnemySettingsSO.Instance.EnemyDefault);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Helper Methods ============================
        // ----------------------------------------------------------------------------------

        public SpawnPoint GetRandomSpawnPoint()
        {
            return new SpawnPoint()
            {
                Position = _spawnArea.GetRandomPosition(),
                Direction = Vector2.down * Random.Range(_enemiesSpeedRange.x, _enemiesSpeedRange.y)
            };
        }


        // ----------------------------------------------------------------------------------
        // ========================== Routines ============================
        // ----------------------------------------------------------------------------------

        [Header("Normal Spawning")]
        [SerializeField][Range(0.1f, 10f)] private float _enemiesPerSec = 1;
        [SerializeField][MinMaxSlider(3, 20)] private Vector2 _enemiesSpeedRange = new Vector2(3, 20);

        public void StartSpawningEnemies()
        {
            StopSpawningEnemies();
            _spawnDelayedCall = DOTweenDelayedCall.DelayedCall(Spawn, 1f / _enemiesPerSec, loops: -1, loopType: DG.Tweening.LoopType.Incremental);
        }

        public void StopSpawningEnemies()
        {
            if (_spawnDelayedCall != null)
            {
                DOTweenDelayedCall.KillDelayedCall(_spawnDelayedCall);
                _spawnDelayedCall = null;
            }
        }

        public void SetMovementActive(bool active)
        {
            List<EnemyController> allEnemies = _activeEnemies + _queuedEnemies as List<EnemyController>;
            for (int i = allEnemies.Count - 1; i >= 0; i--)
            {
                SetMovementActive(allEnemies[i], active);
            }
        }

        public void SetMovementActive(EnemyController enemy, bool active)
        {
            enemy.SetMovementActive(active);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Normal Enemies ============================
        // ----------------------------------------------------------------------------------

        public void Spawn()
        {
            EnemyController newEnemy = _queuedEnemies.Dequeue();
            newEnemy.gameObject.SetActive(true);

            SpawnPoint spawnPoint = GetRandomSpawnPoint();
            newEnemy.transform.SetPositionAndRotation(spawnPoint.Position, spawnPoint.Rotation);
            newEnemy.SetImpulse(spawnPoint.Direction);

            _activeEnemies.Add(newEnemy);
        }

        public void DestroyAll()
        {
            for (int i = _activeEnemies.Count - 1; i >= 0; i--)
            {
                Destroy(_activeEnemies[i]);
            }
        }

        public void Destroy(EnemyController enemy)
        {
            _activeEnemies.Remove(enemy);
            _queuedEnemies.Enqueue(enemy);

            enemy.gameObject.SetActive(false);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Special Enemies ============================
        // ----------------------------------------------------------------------------------



        // ----------------------------------------------------------------------------------
        // ========================== Screen Boundaries ============================
        // ----------------------------------------------------------------------------------

        private void LateUpdate()
        {
            if (_activeEnemies == null) return;

            // Destroy enemies outside boundaries
            for (int i = _activeEnemies.Count - 1; i >= 0; i--)
            {
                if (!_destroyArea.Contains(_activeEnemies[i].transform.position))
                {
                    Destroy(_activeEnemies[i]);
                }
            }
        }

        // ----------------------------------------------------------------------------------
        // ========================== Gizmos Stuff ============================
        // ----------------------------------------------------------------------------------

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            // Draw Spawn Area
            GizmosExtensions.DrawBounds(_spawnArea, color: Color.green);

            // Draw Constrains area
            GizmosExtensions.DrawBounds(_destroyArea, color: Color.red);
        }
#endif

    }

    public struct SpawnPoint
    {
        public Vector2 Position;
        public Quaternion Rotation => Quaternion.identity;
        public Vector2 Direction;
    }
}
