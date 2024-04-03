using Farm.Plants;
using UnityEngine;
using UnityEngine.Serialization;

namespace Timespan.Quota
{
    [System.Serializable]
    public struct QuotaItem
    {
        public PlantType PlantType => _plantType;
        public int QuantityMinimal => _quantityMinimal;
        public int QuantityMaximum => _quantityMaximum;

        [FormerlySerializedAs("_seed")] [SerializeField] private PlantType _plantType;
        [SerializeField, Min(1)] private int _quantityMinimal;
        [SerializeField, Min(1)] private int _quantityMaximum;
    }
}