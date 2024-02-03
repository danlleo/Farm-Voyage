using InputManagers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameplayEssentialsInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInput _playerInput;
        
        public override void InstallBindings()
        {
            BindPlayerInputManager();
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
    }
}
