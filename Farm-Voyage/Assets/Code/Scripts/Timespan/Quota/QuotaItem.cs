using Farm.Plants.Seeds;
using UnityEngine;

namespace Timespan.Quota
{
    [System.Serializable]
    public struct QuotaItem
    {
        public SeedType Seed => _seed;
        public int QuantityMinimal => _quantityMinimal;
        public int QuantityMaximum => _quantityMaximum;

        [SerializeField] private SeedType _seed;
        [SerializeField, Min(1)] private int _quantityMinimal;
        [SerializeField, Min(1)] private int _quantityMaximum;
    }
}