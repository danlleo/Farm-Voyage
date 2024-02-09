using System.Collections.Generic;
using Character.Player;
using Farm;
using Farm.Plants;
using Farm.Plants.Concrete;
using InputManagers;
using Level;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameplayEssentialsInstaller : MonoInstaller
    {
        [Header("Player related")]
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private PlayerFollowCamera _playerFollowCamera;

        [Header("Level related")] 
        [SerializeField] private ResourcesGatherer _resourcesGathererPrefab;
        [SerializeField] private ResourceSO[] _resourcesSOArray;
        [SerializeField] private Transform[] _gatherableResourcesSpawnPoints;
        
        [Header("Plant prefabs")]
        [SerializeField] private Carrot _carrotPrefab;
        [SerializeField] private Tomato _tomatoPrefab;
        [SerializeField] private Corn _cornPrefab;
        [SerializeField] private Eggplant _eggplantPrefab;
        [SerializeField] private Pumpkin _pumpkinPrefab;
        [SerializeField] private Turnip _turnipPrefab;
        
        public override void InstallBindings()
        {
            BindPlayerInputManager();
            BindPlayer();
            BindPlayerFollowCamera();
            BindGatherableResourcesSpawner();
            BindResourcesGathererFactory();
            BindPlantFactory();
        }

        private void BindPlayerInputManager()
        {
            PlayerInput playerInput =
                Container.InstantiatePrefabForComponent<PlayerInput>(_playerInput);

            Container
                .BindInstance(playerInput)
                .AsSingle()
                .NonLazy();
        }
        
        private void BindPlayer()
        {
            Player player =
                Container.InstantiatePrefabForComponent<Player>(_playerPrefab, _playerSpawnPoint.position,
                    Quaternion.identity, null);

            Container
                .BindInstance(player)
                .AsSingle()
                .NonLazy();
        }
        
        private void BindPlayerFollowCamera()
        {
            PlayerFollowCamera playerFollowCamera =
                Container.InstantiatePrefabForComponent<PlayerFollowCamera>(_playerFollowCamera);

            Container
                .BindInstance(playerFollowCamera)
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
        
        private void BindPlantFactory()
        {
            Container
                .Bind<PlantFactory>()
                .AsSingle()
                .WithArguments(_carrotPrefab, _tomatoPrefab, _cornPrefab, _eggplantPrefab,
                    _pumpkinPrefab, _turnipPrefab);
        }
    }
}
