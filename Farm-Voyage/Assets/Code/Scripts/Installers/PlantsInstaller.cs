using Farm.Plants;
using Farm.Plants.ConcretePlants;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Installers
{
    [DisallowMultipleComponent]
    public class PlantsInstaller : MonoInstaller, IValidate
    {
        public bool IsValid { get; private set; }
        
        [FormerlySerializedAs("_carrotPrefab")] [SerializeField] private CarrotPlant _carrotPlantPrefab;
        [FormerlySerializedAs("_tomatoPrefab")] [SerializeField] private TomatoPlant _tomatoPlantPrefab;
        [FormerlySerializedAs("_cornPrefab")] [SerializeField] private CornPlant _cornPlantPrefab;
        [FormerlySerializedAs("_eggplantPrefab")] [SerializeField] private EggplantPlant _eggplantPlantPrefab;
        [FormerlySerializedAs("_pumpkinPrefab")] [SerializeField] private PumpkinPlant _pumpkinPlantPrefab;
        [FormerlySerializedAs("_turnipPrefab")] [SerializeField] private TurnipPlant _turnipPlantPrefab;

        private void OnValidate()
        {
            IsValid = true;

            if (_carrotPlantPrefab == null)
            {
                IsValid = false;
                return;
            }
            
            if (_tomatoPlantPrefab == null)
            {
                IsValid = false;
                return;
            }
            
            if (_cornPlantPrefab == null)
            {
                IsValid = false;
                return;
            }
            
            if (_eggplantPlantPrefab == null)
            {
                IsValid = false;
                return;
            }
            
            if (_pumpkinPlantPrefab == null)
            {
                IsValid = false;
                return;
            }
            
            if (_turnipPlantPrefab == null)
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
                .WithArguments(_carrotPlantPrefab, _tomatoPlantPrefab, _cornPlantPrefab, _eggplantPlantPrefab,
                    _pumpkinPlantPrefab, _turnipPlantPrefab);
        }
    }
}
