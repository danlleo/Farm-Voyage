﻿using Cameras;
using Farm.FarmResources;
using Farm.ResourceGatherer;
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
        [SerializeField] private Market.Market _marketPrefab;
        [SerializeField] private Workbench.Workbench _workbenchPrefab;
        [SerializeField] private ResourcesGatherer _resourcesGathererPrefab;
        [SerializeField] private ResourceSO[] _resourcesSOArray;
        [SerializeField] private Transform _gatherableResourcesSpawnContainer;
        [SerializeField] private IconManager _iconManagerPrefab;
        [SerializeField] private CameraController _cameraControllerPrefab;
        [SerializeField] private UI.UI _uiPrefab;
        
        private void OnValidate()
        {
            IsValid = true;

            if (_dayPrefab == null)
            {
                IsValid = false;
                return;
            }

            if (_marketPrefab == null)
            {
                IsValid = false;
                return;
            }

            if (_workbenchPrefab == null)
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
            BindWorkbench();
            BindMarket();
            BindUI();
        }

        private void BindDay()
        {
            Day.Day day = Container.InstantiatePrefabForComponent<Day.Day>(_dayPrefab);

            Container
                .BindInstance(day)
                .AsSingle()
                .NonLazy();
        }

        private void BindMarket()
        {
            Market.Market market = Container.InstantiatePrefabForComponent<Market.Market>(_marketPrefab);

            Container
                .BindInstance(market)
                .AsSingle()
                .NonLazy();
        }

        private void BindWorkbench()
        {
            Workbench.Workbench workbench =
                Container.InstantiatePrefabForComponent<Workbench.Workbench>(_workbenchPrefab);

            Container
                .BindInstance(workbench)
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
        
        private void BindUI()
        {
            UI.UI ui = Container.InstantiatePrefabForComponent<UI.UI>(_uiPrefab);

            Container
                .BindInstance(ui)
                .AsSingle()
                .NonLazy();
        }
    }
}
