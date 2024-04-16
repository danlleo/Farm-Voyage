using Cameras;
using Farm;
using Farm.FarmResources;
using Farm.ResourceGatherer;
using Level;
using Misc;
using Timespan;
using Timespan.Quota;
using UI.Icon;
using UnityEngine;
using Zenject;

namespace Installers
{
    [DisallowMultipleComponent]
    public class LevelBuilderInstaller : MonoInstaller, IValidate
    {
        public bool IsValid { get; private set; } = true;
        
        [SerializeField] private Market.Market _marketPrefab;
        [SerializeField] private Workbench.Workbench _workbenchPrefab;
        [SerializeField] private Seller.Seller _sellerPrefab;
        [SerializeField] private GlobalStorage _globalStoragePrefab;
        [SerializeField] private ResourcesGatherer _resourcesGathererPrefab;
        [SerializeField] private ResourceSO[] _resourcesSOArray;
        [SerializeField] private Transform _gatherableResourcesSpawnContainer;
        [SerializeField] private IconManager _iconManagerPrefab;
        [SerializeField] private CameraController _cameraControllerPrefab;
        [SerializeField] private UI.UI _uiPrefab;

        [Space(10)] 
        [SerializeField] private QuotaPlanSettingsSO _quotaPlanSettings;

        [Space(10)] 
        [SerializeField] private TimeSettingsSO _timeSettings;
        
        private void OnValidate()
        {
            IsValid = true;

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

            if (_sellerPrefab == null)
            {
                IsValid = false;
                return;
            }

            if (_globalStoragePrefab == null)
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
            BindEconomy();
            BindQuotaPlan();
            BindTimeManger();
            BindResourcesGathererFactory();
            BindResourcesGathererSpawner();
            BindIconManager();
            BindCameraController();
            BindWorkbench();
            BindMarket();
            BindSeller();
            BindGlobalStorage();
            BindUI();
        }

        private void BindSeller()
        {
            Seller.Seller seller = Container.InstantiatePrefabForComponent<Seller.Seller>(_sellerPrefab);

            Container
                .BindInstance(seller)
                .AsSingle()
                .NonLazy();
        }

        private void BindGlobalStorage()
        {
            GlobalStorage globalStorage = Container.InstantiatePrefabForComponent<GlobalStorage>(_globalStoragePrefab);

            Container
                .BindInstance(globalStorage)
                .AsSingle()
                .NonLazy();
        }
        
        private void BindTimeManger()
        {
            Container
                .BindInterfacesAndSelfTo<TimeManager>()
                .AsSingle()
                .WithArguments(_timeSettings)
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
            Container
                .Bind<GatherableResourcesSpawner>()
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

        private void BindEconomy()
        {
            Container
                .Bind<Economy>()
                .AsSingle()
                .NonLazy();
        }

        private void BindQuotaPlan()
        {
            Container
                .Bind<QuotaPlan>()
                .AsSingle()
                .WithArguments(_quotaPlanSettings)
                .NonLazy();
        }
    }
}
