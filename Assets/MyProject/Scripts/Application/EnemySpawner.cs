using enemy;
using enemy.settings;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using tools;
using UnityEngine;
using UnityEngine.UI;
using utils;

namespace application
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private RectTransform _spawnArea;
        private DelayedCall _spawnDelayedCall = null;

        private PoolController<EnemyController> _queuedEnemies;
        private List<EnemyController> _activeEnemies;

        private void Awake()
        {
            _queuedEnemies = new PoolController<EnemyController>(prefab: EnemySettingsSO.Instance.EnemyDefault);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Helper Methods ============================
        // ----------------------------------------------------------------------------------

        public SpawnPoint GetRandomSpawnPoint()
        {
            return new SpawnPoint()
            {
                Position = new Vector2(
                    _spawnArea.position.x + Random.Range(_spawnArea.rect.min.x, _spawnArea.rect.max.x),
                    _spawnArea.position.y + Random.Range(_spawnArea.rect.min.y, _spawnArea.rect.max.y)
                    ),
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
            _spawnDelayedCall = DOTweenDelayedCall.DelayedCall(SpawnEnemy, 1f / _enemiesPerSec, loops: -1, loopType: DG.Tweening.LoopType.Incremental);
        }

        public void StopSpawningEnemies()
        {
            if (_spawnDelayedCall != null)
            {
                DOTweenDelayedCall.KillDelayedCall(_spawnDelayedCall);
                _spawnDelayedCall = null;
            }
        }

        // ----------------------------------------------------------------------------------
        // ========================== Normal Enemies ============================
        // ----------------------------------------------------------------------------------

        public void SpawnEnemy()
        {
            EnemyController newEnemy = _queuedEnemies.Dequeue();
            newEnemy.gameObject.SetActive(true);

            SpawnPoint spawnPoint = GetRandomSpawnPoint();
            newEnemy.transform.SetPositionAndRotation(spawnPoint.Position, spawnPoint.Rotation);
            newEnemy.SetImpulse(spawnPoint.Direction);
        }

        public void DestroyEnemy(EnemyController enemy)
        {
            _activeEnemies.Remove(enemy);
            _queuedEnemies.Enqueue(enemy);

            enemy.gameObject.SetActive(false);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Special Enemies ============================
        // ----------------------------------------------------------------------------------

    }

    public struct SpawnPoint
    {
        public Vector2 Position;
        public Quaternion Rotation => Quaternion.identity;
        public Vector2 Direction;
    }
}
