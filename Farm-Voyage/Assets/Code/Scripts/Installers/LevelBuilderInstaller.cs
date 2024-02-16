using System.Collections.Generic;
using Farm;
using Level;
using Misc;
using UnityEngine;
using Zenject;

namespace Installers
{
    [DisallowMultipleComponent]
    public class LevelBuilderInstaller : MonoInstaller, IValidate
    {
        public bool IsValid { get; private set; } = true;
        
        [SerializeField] private Day.Day _dayPrefab;
        [SerializeField] private ResourcesGatherer _resourcesGathererPrefab;
        [SerializeField] private ResourceSO[] _resourcesSOArray;
        [SerializeField] private Transform[] _gatherableResourcesSpawnPoints;

        private void OnValidate()
        {
            IsValid = true;

            if (_dayPrefab == null)
            {
                IsValid = false;
                return;
            }

            if (_resourcesGathererPrefab == null)
            {
                IsValid = false;
                return;
            }

            if (_resourcesSOArray.Length == 0)
            {
                IsValid = false;
                return;
            }

            if (_gatherableResourcesSpawnPoints.Length != 0) return;
            
            IsValid = false;
        }

        public override void InstallBindings()
        {
            BindDay();
            BindResourcesGathererFactory();
            BindResourcesGathererSpawner();
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
        
        private void BindResourcesGathererSpawner()
        {
            IEnumerable<Transform> spawnPointsEnumerable = _gatherableResourcesSpawnPoints;
            
            Container.Bind<GatherableResourcesSpawner>()
                .AsSingle()
                .WithArguments(_resourcesSOArray, spawnPointsEnumerable)
                .NonLazy();
        }
    }
}