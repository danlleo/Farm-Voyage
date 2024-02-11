using Farm.Plants;
using Farm.Plants.Concrete;
using UnityEngine;
using Zenject;

namespace Installers
{
    [DisallowMultipleComponent]
    public class PlantsInstaller : MonoInstaller
    {
        [SerializeField] private Carrot _carrotPrefab;
        [SerializeField] private Tomato _tomatoPrefab;
        [SerializeField] private Corn _cornPrefab;
        [SerializeField] private Eggplant _eggplantPrefab;
        [SerializeField] private Pumpkin _pumpkinPrefab;
        [SerializeField] private Turnip _turnipPrefab;
        
        public override void InstallBindings()
        {
            BindPlantFactory();
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
