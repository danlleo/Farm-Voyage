using Farm.FarmResources;
using UnityEngine;

namespace Farm
{
    [System.Serializable]
    public struct ResourcePrice
    {
        public ResourceType ResourceType;
        [field: Range(1, 100)] public int Price;

        public ResourcePrice(ResourceType resourceType, int price)
        {
            ResourceType = resourceType;
            Price = price;
        }
    }
}