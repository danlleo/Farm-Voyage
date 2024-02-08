using System;
using Farm.Plants.Concrete;
using UnityEngine;

namespace Farm.Plants
{
    [DisallowMultipleComponent]
    public class PlantFactory : MonoBehaviour
    {
        [Header("Plant prefabs")]
        [SerializeField] private Carrot _carrotPrefab;
        [SerializeField] private Tomato _tomatoPrefab;
        [SerializeField] private Corn _cornPrefab;
        [SerializeField] private Eggplant _eggplantPrefab;
        [SerializeField] private Pumpkin _pumpkinPrefab;
        [SerializeField] private Turnip _turnipPrefab;

        public Plant CreatePlant(PlantType plantType)
        {
            return plantType switch
            {
                PlantType.Tomato => Instantiate(_tomatoPrefab),
                PlantType.Carrot => Instantiate(_carrotPrefab),
                PlantType.Corn => Instantiate(_cornPrefab),
                PlantType.Eggplant => Instantiate(_eggplantPrefab),
                PlantType.Pumpkin => Instantiate(_pumpkinPrefab),
                PlantType.Turnip => Instantiate(_turnipPrefab),
                _ => throw new ArgumentOutOfRangeException(nameof(plantType), plantType, null)
            };
        }
    }
}