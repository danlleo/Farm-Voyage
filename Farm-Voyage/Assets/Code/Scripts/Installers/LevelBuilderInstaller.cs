using System.Collections.Generic;
using Cameras;
using Farm;
using Farm.FarmResources;
using Level;
using Misc;
using UI.Icon;
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
        [SerializeField] private Transform _gatherableResourcesSpawnContainer;
        [SerializeField] private IconManager _iconManagerPrefab;
        [SerializeField] private CameraController _cameraControllerPrefab;
        
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

            if (_gatherableResourcesSpawnContainer == null)
            {
                IsValid = false;
                return;
            }

            if (_iconManagerPrefab == null)
            {
                IsValid = false;
                return;
            }

            if (_cameraControllerPrefab == null)
            {
                IsValid = false;
                return;
            }
        }

        public override void InstallBindings()
        {
            BindDay();
            BindResourcesGathererFactory();
            BindResourcesGathererSpawner();
            BindIconManager();
            BindCameraController();
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
            Container.Bind<GatherableResourcesSpawner>()
                .AsSingle()
                .WithArguments(_resourcesSOArray, _gatherableResourcesSpawnContainer)
                .NonLazy();
        }
        
        private void BindIconManager()
        {
            IconManager iconManager = Container.InstantiatePrefabForComponent<IconManager>(_iconManagerPrefab);

            Container
                .BindInstance(iconManager)
                .AsSingle()
                .NonLazy();
        }
        
        private void BindCameraController()
        {
            CameraController cameraController =
                Container.InstantiatePrefabForComponent<CameraController>(_cameraControllerPrefab);

            Container
                .BindInstance(cameraController)
                .AsSingle();
        }
    }
}
