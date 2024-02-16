using System.Collections.Generic;
using Farm;
using UnityEngine;
using Utilities;

namespace Level
{
    public sealed class GatherableResourcesSpawner
    {
        private ResourceSO[] _resourceSOArray;
        private IEnumerable<Transform> _spawnPointsArray;

        private ResourcesGatherer.Factory _gatherableResourcesFactory;

        public GatherableResourcesSpawner(ResourcesGatherer.Factory gatherableResourcesFactory, ResourceSO[] resourceSOArray, IEnumerable<Transform> spawnPointsArray)
        {
            _gatherableResourcesFactory = gatherableResourcesFactory;
            _resourceSOArray = resourceSOArray;
            _spawnPointsArray = spawnPointsArray;
            SpawnAll();
        }

        private void SpawnAll()
        {
            foreach (Transform point in _spawnPointsArray)
            {
                ResourceSO randomResourceSO = _resourceSOArray.GetRandomItem();
                ResourcesGatherer resourcesGatherer = _gatherableResourcesFactory.Create();
                resourcesGatherer.Initialize(randomResourceSO, point.position, Quaternion.identity);
            }
        }
    }
}
