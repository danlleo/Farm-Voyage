using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Misc.ObjectPool
{
    [DisallowMultipleComponent]
    public class ObjectPoolManager : MonoBehaviour
    {
        public static List<PooledObjectInfo> ObjectPools = new();

        private static GameObject _particleSystemsEmpty;
        private static GameObject _gameObjectsEmpty;
        
        private GameObject _objectPoolEmptyHolder;

        private GameResources _gameResources;
        
        private void Awake()
        {
            SetupEmptyContainers();
        }

        public static T SpawnObject<T>(T objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation,
            PoolType poolType = PoolType.None) where T : Component
        {
            // Find the pool based on the prefab's name
            PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.gameObject.name);

            // If the pool doesn't exist, then create it
            if (pool == null)
            {
                pool = new PooledObjectInfo { LookupString = objectToSpawn.gameObject.name };
                ObjectPools.Add(pool);
            }

            // Check for any inactive objects in the pool
            GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

            if (spawnableObject == null)
            {
                GameObject parentObject = SetParentObject(poolType);
                spawnableObject = Instantiate(objectToSpawn.gameObject, spawnPosition, spawnRotation);
                spawnableObject.name = objectToSpawn.gameObject.name;

                if (parentObject != null)
                {
                    spawnableObject.transform.SetParent(parentObject.transform);
                }
            }
            else
            {
                spawnableObject.transform.position = spawnPosition;
                spawnableObject.transform.rotation = spawnRotation;
                pool.InactiveObjects.Remove(spawnableObject);
                spawnableObject.SetActive(true);
            }

            return spawnableObject.GetComponent<T>();
        }

        public static T SpawnObject<T>(T objectToSpawn, Transform parent) where T : Component
        {
            PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.gameObject.name);

            if (pool == null)
            {
                pool = new PooledObjectInfo { LookupString = objectToSpawn.gameObject.name };
                ObjectPools.Add(pool);
            }

            GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

            if (spawnableObject == null)
            {
                spawnableObject = Instantiate(objectToSpawn.gameObject, parent);
                spawnableObject.name = objectToSpawn.gameObject.name;
            }
            else
            {
                pool.InactiveObjects.Remove(spawnableObject);
                spawnableObject.SetActive(true);
            }

            spawnableObject.transform.SetParent(parent);
            return spawnableObject.GetComponent<T>();
        }

        public static void ReturnObjectToPool<T>(T obj) where T : Component
        {
            PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == obj.gameObject.name);

            if (pool == null)
            {
                Debug.LogWarning($"Trying to release an object that is not pooled: {obj.gameObject.name}");
            }
            else
            {
                obj.gameObject.SetActive(false);
                pool.InactiveObjects.Add(obj.gameObject);
            }
        }

        private static GameObject SetParentObject(PoolType poolType)
        {
            return poolType switch
            {
                PoolType.ParticleSystem => _particleSystemsEmpty,
                PoolType.GameObject => _gameObjectsEmpty,
                PoolType.None => null,
                _ => throw new ArgumentOutOfRangeException(nameof(poolType), poolType, null)
            };
        }
        
        private void SetupEmptyContainers()
        {
            _objectPoolEmptyHolder = new GameObject("Pooled Objects");

            _particleSystemsEmpty = new GameObject("Particle Effects");
            _particleSystemsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
            
            _gameObjectsEmpty = new GameObject("Particle Effects");
            _gameObjectsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
        }
    }
}