using System;
using Farm.Plants.Concrete;
using Object = UnityEngine.Object;

namespace Farm.Plants
{
    public sealed class PlantFactory
    {
        private Carrot _carrotPrefab;
        private Tomato _tomatoPrefab;
        private Corn _cornPrefab;
        private Eggplant _eggplantPrefab;
        private Pumpkin _pumpkinPrefab;
        private Turnip _turnipPrefab;

        public PlantFactory(Carrot carrotPrefab, Tomato tomatoPrefab, Corn cornPrefab, Eggplant eggplantPrefab, Pumpkin pumpkinPrefab, Turnip turnipPrefab)
        {
            _carrotPrefab = carrotPrefab;
            _tomatoPrefab = tomatoPrefab;
            _cornPrefab = cornPrefab;
            _eggplantPrefab = eggplantPrefab;
            _pumpkinPrefab = pumpkinPrefab;
            _turnipPrefab = turnipPrefab;
        }

        public Plant Create(PlantType plantType)
        {
            return plantType switch
            {
                PlantType.Tomato => Object.Instantiate(_tomatoPrefab),
                PlantType.Carrot => Object.Instantiate(_carrotPrefab),
                PlantType.Corn => Object.Instantiate(_cornPrefab),
                PlantType.Eggplant => Object.Instantiate(_eggplantPrefab),
                PlantType.Pumpkin => Object.Instantiate(_pumpkinPrefab),
                PlantType.Turnip => Object.Instantiate(_turnipPrefab),
                _ => throw new ArgumentOutOfRangeException(nameof(plantType), plantType, null)
            };
        }
    }
}