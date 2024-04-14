using System.Collections.Generic;
using Character.Player;
using Farm.Tool;
using Farm.Tool.ConcreteTools;
using InputManagers;
using Misc;
using UnityEngine;
using Zenject;

namespace Installers
{
    [DisallowMultipleComponent]
    public class PlayerEssentialsInstaller : MonoInstaller, IValidate
    {
        public bool IsValid { get; private set; } = true;
        
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private PlayerFollowCamera _playerFollowCamera;
        
        private IPlayerInput _playerKeyboardInput;

        private void OnValidate()
        {
            IsValid = true;

            if (_playerKeyboardInput == null)
            {
                IsValid = false;
                return;
            }

            if (_playerPrefab == null)
            {
                IsValid = false;
                return;
            }

            if (_playerSpawnPoint == null)
            {
                IsValid = false;
                return;
            }

            if (_playerFollowCamera == null)
            {
                IsValid = false;
            }
        }

        public override void InstallBindings()
        {
            BindPlayerInputManager();
            BindPlayer();
            BindPlayerInventory();
            BindPlayerFollowCamera();
        }

        private void BindPlayerInventory()
        {
            List<Tool> inventoryTools = new()
            {
                new Axe(3f, 1),
                new Pickaxe(3f, 1),
                new Shovel(3f, 1),
                new WaterCan(3f, 1),
            };

            Container
                .Bind<PlayerInventory>()
                .AsSingle()
                .WithArguments(inventoryTools)
                .NonLazy();
        }

        private void BindPlayerInputManager()
        {
            Container
                .BindInterfacesAndSelfTo<DesktopInput>()
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
    }
}