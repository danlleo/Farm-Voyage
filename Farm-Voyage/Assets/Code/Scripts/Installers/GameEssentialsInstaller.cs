using Misc;
using Zenject;

namespace Installers
{
    public class GameEssentialsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindAsyncProcessor();
        }

        private void BindAsyncProcessor()
        {
            AsyncProcessor asyncProcessor = Container.InstantiateComponentOnNewGameObject<AsyncProcessor>();
            
            Container
                .BindInstance(asyncProcessor)
                .AsSingle()
                .NonLazy();
        }
    }
}