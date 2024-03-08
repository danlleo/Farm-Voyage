using Misc;
using Zenject;

namespace Installers
{
    public class GameEssentialsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindAsyncProcessor();
            BindSceneTransition();
        }

        private void BindAsyncProcessor()
        {
            AsyncProcessor asyncProcessor = Container.InstantiateComponentOnNewGameObject<AsyncProcessor>();
            
            Container
                .BindInstance(asyncProcessor)
                .AsSingle()
                .NonLazy();
        }

        private void BindSceneTransition()
        {
            Container
                .Bind<SceneTransition>()
                .AsSingle()
                .NonLazy();
        }
    }
}