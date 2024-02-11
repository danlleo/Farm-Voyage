using System.Collections.Generic;
using Farm;
using Level;
using UnityEngine;
using Zenject;

namespace Installers
{
    [DisallowMultipleComponent]
    public class LevelBuilderInstaller : MonoInstaller
    {
        [SerializeField] private Day.Day _dayPrefab;
        [SerializeField] private ResourcesGatherer _resourcesGathererPrefab;
        [SerializeField] private ResourceSO[] _resourcesSOArray;
        [SerializeField] private Transform[] _gatherableResourcesSpawnPoints;
        
        public override void InstallBindings()
        {
            BindDay();
            BindGatherableResourcesSpawner();
            BindResourcesGathererFactory();
        }

        private void BindDay()
        {
            Day.Day day = Container.InstantiatePrefabForComponent<Day.Day>(_dayPrefab);

            Container
                .BindInstance(day)
                .AsSingle()
                .NonLazy();
        }

        private void BindResourcesGathererFactory()
        {
            Container
                .BindFactory<ResourcesGatherer, ResourcesGatherer.Factory>()
                .FromComponentInNewPrefab(_resourcesGathererPrefab);
        }

        private void BindGatherableResourcesSpawner()
        {
            IEnumerable<Transform> spawnPointsEnumerable = _gatherableResourcesSpawnPoints;

            Container.Bind<GatherableResourcesSpawner>()
                .AsSingle()
                .WithArguments(_resourcesSOArray, spawnPointsEnumerable)
                .NonLazy();
        }
    }
}