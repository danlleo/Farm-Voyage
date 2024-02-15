using Farm.Plants;
using Farm.Plants.Concrete;
using Misc;
using UnityEngine;
using Zenject;

namespace Installers
{
    [DisallowMultipleComponent]
    public class PlantsInstaller : MonoInstaller, IValidate
    {
        public bool IsValid { get; private set; }
        
        [SerializeField] private Carrot _carrotPrefab;
        [SerializeField] private Tomato _tomatoPrefab;
        [SerializeField] private Corn _cornPrefab;
        [SerializeField] private Eggplant _eggplantPrefab;
        [SerializeField] private Pumpkin _pumpkinPrefab;
        [SerializeField] private Turnip _turnipPrefab;

        private void OnValidate()
        {
            IsValid = true;

            if (_carrotPrefab == null)
            {
                IsValid = false;
                return;
            }
            
            if (_tomatoPrefab == null)
            {
                IsValid = false;
                return;
            }
            
            if (_cornPrefab == null)
            {
                IsValid = false;
                return;
            }
            
            if (_eggplantPrefab == null)
            {
                IsValid = false;
                return;
            }
            
            if (_pumpkinPrefab == null)
            {
                IsValid = false;
                return;
            }
            
            if (_turnipPrefab == null)
            {
                IsValid = false;
            }
        }

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
