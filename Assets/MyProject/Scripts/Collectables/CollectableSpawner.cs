using collectable.settings;
using System.Collections.Generic;
using UnityEngine;
using utils;

namespace collectable
{
    public class CollectableSpawner : MonoBehaviour
    {
        private List<Collectable> _activeCollectables = new List<Collectable>();
        private PoolController<Collectable> _queuedCollectables;

        public int CollectablesActive => _activeCollectables.Count;

        private bool _initialized = false;

        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            _queuedCollectables = new PoolController<Collectable>(20, prefab: CollectableSettingsSO.Instance.DefaultPrefab);
        }

        public void Spawn(Vector2 position)
        {
            Collectable newCollectable = _queuedCollectables.Dequeue();
            newCollectable.gameObject.SetActive(true);
            newCollectable.transform.SetPositionAndRotation(position, Quaternion.identity);

            _activeCollectables.Add(newCollectable);
        }

        public void DestroyAll()
        {
            for (int i = _activeCollectables.Count - 1; i >= 0; i--)
            {
                Destroy(_activeCollectables[i]);
            }
        }

        public void Destroy(Collectable collectable)
        {
            _activeCollectables.Remove(collectable);
            _queuedCollectables.Enqueue(collectable);

            collectable.gameObject.SetActive(false);
        }
    }
}
