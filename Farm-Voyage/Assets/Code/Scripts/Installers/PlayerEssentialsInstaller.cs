using Character.Player;
using InputManagers;
using UnityEngine;
using Zenject;

namespace Installers
{
    [DisallowMultipleComponent]
    public class PlayerEssentialsInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private PlayerFollowCamera _playerFollowCamera;

        public override void InstallBindings()
        {
            BindPlayerInputManager();
            BindPlayer();
            BindPlayerFollowCamera();
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
    }
}