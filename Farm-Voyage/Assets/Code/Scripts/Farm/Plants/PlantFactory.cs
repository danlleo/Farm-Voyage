using System;
using Farm.Plants.ConcretePlants;
using Object = UnityEngine.Object;

namespace Farm.Plants
{
    public sealed class PlantFactory
    {
        private CarrotPlant _carrotPlantPrefab;
        private TomatoPlant _tomatoPlantPrefab;
        private CornPlant _cornPlantPrefab;
        private EggplantPlant _eggplantPlantPrefab;
        private PumpkinPlant _pumpkinPlantPrefab;
        private TurnipPlant _turnipPlantPrefab;

        public PlantFactory(CarrotPlant carrotPlantPrefab, TomatoPlant tomatoPlantPrefab, CornPlant cornPlantPrefab,
            EggplantPlant eggplantPlantPrefab, PumpkinPlant pumpkinPlantPrefab, TurnipPlant turnipPlantPrefab)
        {
            _carrotPlantPrefab = carrotPlantPrefab;
            _tomatoPlantPrefab = tomatoPlantPrefab;
            _cornPlantPrefab = cornPlantPrefab;
            _eggplantPlantPrefab = eggplantPlantPrefab;
            _pumpkinPlantPrefab = pumpkinPlantPrefab;
            _turnipPlantPrefab = turnipPlantPrefab;
        }

        public Plant Create(PlantType plantType)
        {
            return plantType switch
            {
                PlantType.Tomato => Object.Instantiate(_tomatoPlantPrefab),
                PlantType.Carrot => Object.Instantiate(_carrotPlantPrefab),
                PlantType.Corn => Object.Instantiate(_cornPlantPrefab),
                PlantType.Eggplant => Object.Instantiate(_eggplantPlantPrefab),
                PlantType.Pumpkin => Object.Instantiate(_pumpkinPlantPrefab),
                PlantType.Turnip => Object.Instantiate(_turnipPlantPrefab),
                _ => throw new ArgumentOutOfRangeException(nameof(plantType), plantType, null)
            };
        }
    }
}