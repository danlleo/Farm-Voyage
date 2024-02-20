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

        public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
        {
            PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

            // If the pool doesn't exist (we didn't find it), then create it
            if (pool == null)
            {
                pool = new PooledObjectInfo { LookupString = objectToSpawn.name };
                ObjectPools.Add(pool);
            }
            
            // Check if there are any inactive objects in the pool, meaning that there's something in the pool we can use
            GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

            if (spawnableObject == null)
            {
                GameObject parentObject = SetParentObject(poolType);
                
                // If there are no inactive objects, create a new one
                spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
                spawnableObject.name = objectToSpawn.name;

                if (parentObject != null)
                {
                    spawnableObject.transform.SetParent(parentObject.transform);
                }
            }
            else
            {
                // If there is inactive object, reactivate it
                spawnableObject.transform.position = spawnPosition;
                spawnableObject.transform.rotation = spawnRotation;
                pool.InactiveObjects.Remove(spawnableObject);
                spawnableObject.SetActive(true);
            }

            return spawnableObject;
        }

        public static GameObject SpawnObject(GameObject objectToSpawn, Transform parent)
        {
            PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

            if (pool == null)
            {
                pool = new PooledObjectInfo { LookupString = objectToSpawn.name };
                ObjectPools.Add(pool);
            }
            
            GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

            if (spawnableObject == null)
            {
                spawnableObject = Instantiate(objectToSpawn, parent);
                spawnableObject.name = objectToSpawn.name;
            }
            else
            {
                pool.InactiveObjects.Remove(spawnableObject);
                spawnableObject.SetActive(true);
            }

            return spawnableObject;
        }
        
        public static void ReturnObjectToPool(GameObject obj)
        {
            PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == obj.name);

            if (pool == null)
            {
                Debug.LogWarning($"Trying to release an object that is not pooled: {obj.name}");
            }
            else
            {
                obj.SetActive(false);
                pool.InactiveObjects.Add(obj);
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