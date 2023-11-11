using log;
using System.Collections.Generic;
using UnityEngine;

namespace utils
{
    public class PoolController<T> where T : Component, new()
    {
        //public List<T> ActiveObjects { get; private set; } = new List<T>();
        private Transform _queueParent;
        private Queue<T> _queue;
        private GameObject _prefab;

        public Queue<T> Queue => _queue;

        public PoolController() : this(10) { }
        public PoolController(GameObject prefab) : this(10, prefab) { }

        /// <summary>
        /// Creates a new PollController with a start capacity and an optional prefab
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="prefab"></param>
        public PoolController(int capacity, GameObject prefab = null)
        {
            ELog.Log(ELogType.POOL, "Instantiating {0} {1}{2}", capacity, typeof(T).Name, (prefab == null ? "" : " [prefab=" + prefab.name + "]"));
            _queue = new Queue<T>(capacity);
            _prefab = prefab;

            string queueName = string.Format("{0}_{1}_queue", typeof(T).Name, prefab.name);
            if (_queueParent == null)
            {
                GameObject gObj = GameObject.Find(queueName);
                if (gObj == null)
                    gObj = new GameObject(queueName);

                _queueParent = gObj.transform;
            }

            InstantiateMultiple(capacity);
        }

        /// <summary>
        /// * Update the current prefab
        /// * Remove all prefab instances to the new one
        /// </summary>
        /// <param name="prefab"></param>
        public void UpdatePrefab(GameObject prefab)
        {
            _prefab = prefab;
            // MEDO
        }

        private void InstantiateMultiple(int count)
        {
            for (int i = 0; i < count; i++)
                Instantiate();
        }

        private void Instantiate()
        {
            T obj;
            if (_prefab != null)
            {
                obj = GameObject.Instantiate(_prefab).GetComponent<T>();
            }
            else if (typeof(T) == typeof(GameObject))
            {
                obj = new GameObject(typeof(T).Name) as T;
            }
            else if (typeof(T) == typeof(Component))
            {
                obj = new GameObject(typeof(T).Name).AddComponent(typeof(T)) as T;
            }
            else
            {
                obj = new T();
            }

            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_queueParent);
            Enqueue(obj);
        }

        public void Enqueue(T obj)
        {
            _queue.Enqueue(obj);
        }

        public T[] Dequeue(int count)
        {
            List<T> list = new List<T>();

            for (int i = 0; i < count; i++)
            {
                list.Add(Dequeue());
            }

            return list.ToArray();
        }

        public T Dequeue()
        {
            if (_queue.Count == 0)
            {
                ELog.LogWarning(ELogType.POOL, "Instantiating additional {0}{1}", typeof(T).Name, (_prefab == null ? "" : " [prefab=" + _prefab.name + "]"));
                Instantiate();
            }

            return _queue.Dequeue();
        }


        // ----------------------------------------------------------------------------------
        // ========================== Operator Overloads ============================
        // ----------------------------------------------------------------------------------

        // Overloading the + operator to add items from IEnumerable to the pool
        public static IEnumerable<T> operator +(IEnumerable<T> items, PoolController<T> poolController) => poolController + items;
        public static IEnumerable<T> operator +(PoolController<T> poolController, IEnumerable<T> items)
        {
            IEnumerable<T> result = new List<T>();

            foreach (T item in items)
                ((List<T>)result).Add(item);

            foreach (T item in poolController.Queue)
                ((List<T>)result).Add(item);

            return result;
        }
    }
}
