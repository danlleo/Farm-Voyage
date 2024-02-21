using Farm;
using Farm.FarmResources;
using Farm.ResourceGatherer;
using UnityEngine;
using Utilities;

namespace Level
{
    public sealed class GatherableResourcesSpawner
    {
        private ResourceSO[] _resourceSOArray;
        private Transform _spawnPointsContainer;

        private ResourcesGatherer.Factory _gatherableResourcesFactory;

        public GatherableResourcesSpawner(ResourcesGatherer.Factory gatherableResourcesFactory, ResourceSO[] resourceSOArray, Transform spawnPointsContainer)
        {
            _gatherableResourcesFactory = gatherableResourcesFactory;
            _resourceSOArray = resourceSOArray;
            _spawnPointsContainer = spawnPointsContainer;
            SpawnAll();
        }

        private void SpawnAll()
        {
            var gatherableResourcesContainer = new GameObject("GatherableResourcesContainer");
            
            foreach (Transform point in _spawnPointsContainer.transform)
            {
                ResourceSO randomResourceSO = _resourceSOArray.GetRandomItem();
                ResourcesGatherer resourcesGatherer = _gatherableResourcesFactory.Create();
                resourcesGatherer.transform.SetParent(gatherableResourcesContainer.transform);
                resourcesGatherer.Initialize(randomResourceSO, point.position, Quaternion.identity);
            }
        }
    }
}
