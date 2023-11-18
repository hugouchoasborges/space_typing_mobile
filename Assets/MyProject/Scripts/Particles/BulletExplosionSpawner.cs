using System.Collections.Generic;
using tools;
using UnityEngine;
using utils;

namespace particles
{
    public class BulletExplosionSpawner : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject _prefab;

        private List<ParticleSystem> _activeExplosions = new List<ParticleSystem>();
        private PoolController<ParticleSystem> _queuedExplosions;

        private bool _initialized = false;

        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            _queuedExplosions = new PoolController<ParticleSystem>(10, prefab: _prefab);
        }

        public void Spawn(Vector2 position, Vector2 direction)
        {
            ParticleSystem newParticleSystem = _queuedExplosions.Dequeue();
            newParticleSystem.gameObject.SetActive(true);
            newParticleSystem.transform.SetPositionAndRotation(position, GetRotationFromDirection(direction));

            _activeExplosions.Add(newParticleSystem);

            // Start lifetime cycle
            _lifecycleDelayedCall[newParticleSystem] = DOTweenDelayedCall.DelayedCall(() => OnLifeCycleEnded(newParticleSystem), 5);
        }

        private Dictionary<ParticleSystem, DelayedCall> _lifecycleDelayedCall = new Dictionary<ParticleSystem, DelayedCall>();

        private void OnLifeCycleEnded(ParticleSystem particleSystem)
        {
            if (_lifecycleDelayedCall.ContainsKey(particleSystem) && _lifecycleDelayedCall[particleSystem] != null)
            {
                DOTweenDelayedCall.KillDelayedCall(_lifecycleDelayedCall[particleSystem]);
                _lifecycleDelayedCall[particleSystem] = null;
            }

            Destroy(particleSystem);
        }

        private Quaternion GetRotationFromDirection(Vector2 direction)
        {
            // Normalize the direction vector
            direction.Normalize();

            // Calculate the angle in degrees from the direction vector
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Calculate the pitch angle based on direction.y
            float pitchAngle = Mathf.Asin(direction.y) * Mathf.Rad2Deg;

            // Calculate the quaternion rotation
            Quaternion rotation = Quaternion.Euler(-pitchAngle, angle, 0);

            return rotation;
        }

        public void DestroyAll()
        {
            for (int i = _activeExplosions.Count - 1; i >= 0; i--)
            {
                Destroy(_activeExplosions[i]);
            }
        }

        public void Destroy(ParticleSystem particleSystem)
        {
            _activeExplosions.Remove(particleSystem);
            _queuedExplosions.Enqueue(particleSystem);

            particleSystem.gameObject.SetActive(false);
        }
    }
}
