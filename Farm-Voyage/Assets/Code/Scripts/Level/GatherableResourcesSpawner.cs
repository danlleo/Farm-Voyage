using System.Collections.Generic;
using Farm;
using UnityEngine;
using Utilities;

namespace Level
{
    public class GatherableResourcesSpawner
    {
        private ResourcesGatherer[] _resourcesGathererPrefabsArray;
        private IEnumerable<Transform> _spawnPointsArray;
        
        public GatherableResourcesSpawner(ResourcesGatherer[] resourcesGathererPrefabsArray, IEnumerable<Transform> spawnPointsArray)
        {
            _resourcesGathererPrefabsArray = resourcesGathererPrefabsArray;
            _spawnPointsArray = spawnPointsArray;
            
            SpawnAll();
        }

        private void SpawnAll()
        {
            foreach (Transform point in _spawnPointsArray)
            {
                ResourcesGatherer randomResourcesGatherer = _resourcesGathererPrefabsArray.GetRandomItem();
                Object.Instantiate(randomResourcesGatherer, point.position, Quaternion.identity);
            }
        }
    }
}
