using System.Collections.Generic;
using Farm;
using UnityEngine;
using Utilities;
using Zenject;

namespace Level
{
    public sealed class GatherableResourcesSpawner
    {
        private ResourceSO[] _resourceSOArray;
        private IEnumerable<Transform> _spawnPointsArray;

        private ResourcesGatherer.Factory _gatherableResourcesFactory;

        [Inject]
        private void Construct(ResourcesGatherer.Factory gatherableResourcesFactory)
        {
            _gatherableResourcesFactory = gatherableResourcesFactory;
            SpawnAll();
        }

        public GatherableResourcesSpawner(ResourceSO[] resourceSOArray, IEnumerable<Transform> spawnPointsArray)
        {
            _resourceSOArray = resourceSOArray;
            _spawnPointsArray = spawnPointsArray;
        }

        private void SpawnAll()
        {
            foreach (Transform point in _spawnPointsArray)
            {
                ResourceSO randomResourceSO = _resourceSOArray.GetRandomItem();
                ResourcesGatherer randomResourcesGatherer = _gatherableResourcesFactory.Create();
                randomResourcesGatherer.Initialize(randomResourceSO, point.position, Quaternion.identity);
            }
        }
    }
}
