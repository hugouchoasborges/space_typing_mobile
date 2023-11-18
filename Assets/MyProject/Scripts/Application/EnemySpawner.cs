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
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Bounds _spawnArea = new Bounds(Vector3.up * 35, new Vector3(40, 4, 0));
        [SerializeField] private Bounds _destinationArea = new Bounds(Vector3.down * 35, new Vector3(40, 4, 0));
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

            _queuedEnemies = new PoolController<EnemyController>(prefab: _prefab);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Helper Methods ============================
        // ----------------------------------------------------------------------------------

        public SpawnPoint GetRandomSpawnPoint()
        {
            Vector2 from = _spawnArea.GetRandomPosition();
            Vector2 to = _destinationArea.GetRandomPosition();
            float speed = Random.Range(_enemiesSpeedRange.x, _enemiesSpeedRange.y);

            return new SpawnPoint()
            {
                Position = from,
                Direction = GetDirectionNormalized(from, to) * speed
            };
        }

        private Vector2 GetDirectionNormalized(Vector2 from, Vector2 to)
        {
            return (to - from).normalized;
        }


        // ----------------------------------------------------------------------------------
        // ========================== Routines ============================
        // ----------------------------------------------------------------------------------

        [Header("Spawning")]
        [SerializeField][MinMaxSlider(0f, 5f, true)] private Vector2 _startDelay = new Vector2(0f, 3f);
        [SerializeField][Range(0.1f, 10f)] private float _enemiesPerSec = 1;
        [SerializeField][MinMaxSlider(3, 20, true)] private Vector2 _enemiesSpeedRange = new Vector2(3, 20);

        private DelayedCall _startingDelayedCall;

        public void StartSpawningEnemies()
        {
            StopStarting();
            _startingDelayedCall = DOTweenDelayedCall.DelayedCall(DoStartSpawningEnemies, _startDelay.GetRandomFloat());
        }

        private void StopStarting()
        {
            if (_startingDelayedCall != null)
            {
                DOTweenDelayedCall.KillDelayedCall(_startingDelayedCall);
                _startingDelayedCall = null;
            }
        }

        private void DoStartSpawningEnemies()
        {
            StopStarting();
            StopSpawningEnemies();
            _spawnDelayedCall = DOTweenDelayedCall.DelayedCall(Spawn, 1f / _enemiesPerSec, loops: -1, loopType: DG.Tweening.LoopType.Incremental);

            StartDifficultyRoutine();
        }

        public void StopSpawningEnemies()
        {
            if (_spawnDelayedCall != null)
            {
                DOTweenDelayedCall.KillDelayedCall(_spawnDelayedCall);
                _spawnDelayedCall = null;
            }
        }

        public void SetEnemiesActive(bool active)
        {
            List<EnemyController> allEnemies = _activeEnemies + _queuedEnemies as List<EnemyController>;
            for (int i = allEnemies.Count - 1; i >= 0; i--)
            {
                SetEnemyActive(allEnemies[i], active);
            }
        }

        public void SetEnemyActive(EnemyController enemy, bool active)
        {
            enemy.SetActive(active);
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

            if (_requestedIncreaseDifficulty)
                DoIncreaseDifficulty();
        }

        [Button("Destroy All")]
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
        // ========================== Difficulty Progression ============================
        // ----------------------------------------------------------------------------------

        [Header("Difficulty Progression")]
        [SerializeField][MinMaxSlider(.1f, 10f, true)] private Vector2 _enemiesPerSecRange;
        [SerializeField][Range(0f, 1f)] private float _enemiesPerSecIncreaseRate;
        private bool _requestedIncreaseDifficulty;

        private DelayedCall _difficultyProgressionDelayedCall;

        private void StartDifficultyRoutine()
        {
            if (_difficultyProgressionDelayedCall != null) return;

            _enemiesPerSec = _enemiesPerSecRange.x;
            _difficultyProgressionDelayedCall = DOTweenDelayedCall.DelayedCall(IncreaseDifficulty, 6f, loops: -1, loopType: DG.Tweening.LoopType.Incremental);
        }

        private void IncreaseDifficulty()
        {
            if (_requestedIncreaseDifficulty) return;

            _enemiesPerSec += _enemiesPerSecIncreaseRate;
            _enemiesPerSec = Mathf.Clamp(_enemiesPerSec, _enemiesPerSecRange.x, _enemiesPerSecRange.y);

            _requestedIncreaseDifficulty = true;
        }

        private void DoIncreaseDifficulty()
        {
            _requestedIncreaseDifficulty = false;
            StartSpawningEnemies();
        }

        public void StopDifficultyRoutine()
        {
            if (_difficultyProgressionDelayedCall != null)
            {
                DOTweenDelayedCall.KillDelayedCall(_difficultyProgressionDelayedCall);
                _difficultyProgressionDelayedCall = null;
            }
        }

        [Button("Reset Difficulty")]
        private void ResetDifficulty()
        {
            StopDifficultyRoutine();
            StartDifficultyRoutine();
        }


        // ----------------------------------------------------------------------------------
        // ========================== Screen Boundaries ============================
        // ----------------------------------------------------------------------------------

        private void LateUpdate()
        {
            if (_activeEnemies == null) return;

            // Destroy enemies outside boundaries
            EnemyController[] currentEnemies = _activeEnemies.ToArray();
            for (int i = currentEnemies.Length - 1; i >= 0; i--)
            {
                if (!_destroyArea.Contains(currentEnemies[i].transform.position))
                {
                    Destroy(currentEnemies[i]);
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

            // Draw Destination Area
            GizmosExtensions.DrawBounds(_destinationArea, color: Color.blue);

            // Draw Constrains area
            GizmosExtensions.DrawBounds(_destroyArea, color: Color.red);
        }

        private void OnValidate()
        {
            StartSpawningEnemies();
        }
#endif


        // ----------------------------------------------------------------------------------
        // ========================== Pause System ============================
        // ----------------------------------------------------------------------------------

        public void SetPaused(bool paused)
        {
            // Pause/Resume delayed calls
            if (_difficultyProgressionDelayedCall != null) _difficultyProgressionDelayedCall.Sequence.timeScale = paused ? 0 : 1;
            if (_spawnDelayedCall != null) _spawnDelayedCall.Sequence.timeScale = paused ? 0 : 1;
            if (_startingDelayedCall != null) _startingDelayedCall.Sequence.timeScale = paused ? 0 : 1;

            // Pause enemies
            SetEnemiesActive(!paused);
        }
    }

    public struct SpawnPoint
    {
        public Vector2 Position;
        public Quaternion Rotation => Quaternion.identity;
        public Vector2 Direction;
    }
}
