using System.Collections.Generic;
using Character.Player;
using Farm;
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
        [SerializeField] private ResourcesGatherer[] _resourcesGathererPrefabs;
        [SerializeField] private Transform[] _gatherableResourcesSpawnPoints;
        
        public override void InstallBindings()
        {
            BindPlayerInputManager();
            BindPlayer();
            BindPlayerFollowCamera();
            BindGatherableResourcesSpawner();
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
        
        private void BindGatherableResourcesSpawner()
        {
            IEnumerable<Transform> spawnPointsEnumerable = _gatherableResourcesSpawnPoints;
            Container
                .Bind<GatherableResourcesSpawner>()
                .AsSingle()
                .WithArguments(_resourcesGathererPrefabs, spawnPointsEnumerable)
                .NonLazy();
        }
    }
}
