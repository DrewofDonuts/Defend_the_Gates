using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Etheral
{
    //REMEMBER TO RESET SETTINGS FOR CERTAIN OBJECTS, SUCH AS PROJECTILES

    public class ObjectPoolManager : MonoBehaviour
    {
        static ObjectPoolManager _instance;
        public static ObjectPoolManager Instance => _instance;


        // Dictionary<Type, object> pools = new();
        Dictionary<string, object> pools = new();


        void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
        }

        //Defaults to Manager as parent
        public ObjectPool<T> GetPool<T>(T prefab, int defaultCapacity, int maxSize = 100)
            where T : MonoBehaviour, IAmPoolObject<T>
        {
            return GetPool(prefab, transform, defaultCapacity, maxSize);
        }

        //Allows for custom parent
        public ObjectPool<T> GetPool<T>(T prefab, Transform parent, int defaultCapacity, int maxSize = 100)
            where T : MonoBehaviour, IAmPoolObject<T>
        {
            Type type = typeof(T);
            string key = GetPoolKey(prefab);

            if (!pools.ContainsKey(key) || pools[key] == null)
            {
                ObjectPool<T> pool = new ObjectPool<T>(createFunc: () =>
                    {
                        T obj = Instantiate(prefab, parent);
                        obj.PoolKey = key;
                        return obj;
                    },
                    
                    
                    actionOnGet: (obj) => obj.gameObject.SetActive(true),
                    actionOnRelease: (obj) => obj.gameObject.SetActive(false),
                    actionOnDestroy: (obj) => Destroy(obj.gameObject),
                    defaultCapacity: defaultCapacity,
                    maxSize: maxSize
                );

                pools[key] = pool;
            }

            return (ObjectPool<T>)pools[key];
        }

        public T GetObject<T>(T prefab, int defaultCapacity = 20) where T : MonoBehaviour, IAmPoolObject<T>
        {
            var pool = GetPool(prefab, defaultCapacity);
            return pool.Get();
        }

        public T GetObject<T>(T prefab, Transform parent, int defaultCapacity = 20)
            where T : MonoBehaviour, IAmPoolObject<T>
        {
            var pool = GetPool(prefab, parent, defaultCapacity);
            return pool.Get();
        }

        public T GetObject<T>(T prefab, Vector3 position, Quaternion rotation, int defaultCapacity = 20)
            where T : MonoBehaviour, IAmPoolObject<T>
        {
            var pool = GetPool(prefab, defaultCapacity);
            T obj = pool.Get();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }


        public IEnumerator ReleaseObject<T>(T obj, float timeBeforeRelease) where T : MonoBehaviour, IAmPoolObject<T>
        {
            yield return new WaitForSeconds(timeBeforeRelease);
            ReleaseObject(obj);
        }

        public void ReleaseObject<T>(T obj) where T : MonoBehaviour, IAmPoolObject<T>
        {
            string key = GetPoolKey(obj);


            if (pools.ContainsKey(key))
            {
                var pool = (ObjectPool<T>)pools[key];
                
                Debug.Log($"Releasing {obj.name} to pool {key}");
                pool.Release(obj);
            }
            else
            {
                Debug.LogWarning($"No pool exists for type {key}. Destroying the object instead.");
                Destroy(obj.gameObject);
            }
        }
        
        

        string GetPoolKey<T>(T prefab) where T : MonoBehaviour, IAmPoolObject<T>
        {
            return prefab.PoolKey;
            // return $"{typeof(T)}_{prefab.PoolKey}";
        }

#if UNITY_EDITOR

        [ContextMenu("Check Pool Data")]
        void CheckPoolData()
        {
            foreach (var pool in pools)
            {
                Debug.Log($"Pool: {pool.Key}");
                Debug.Log($"Pool Count: {(ObjectPool<MonoBehaviour>)pool.Value}");
            }

        }

#endif
    }
}