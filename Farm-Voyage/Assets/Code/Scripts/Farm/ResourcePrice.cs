using Farm.FarmResources;
using UnityEngine;

namespace Farm
{
    [System.Serializable]
    public struct ResourcePrice
    {
        [field: SerializeField] public ResourceType ResourceType { get; private set; }
        [field: SerializeField, Range(1, 100)] public int Price { get; private set; }
        
        public ResourcePrice(ResourceType resourceType, int price)
        {
            ResourceType = resourceType;
            Price = price;
        }
    }
}