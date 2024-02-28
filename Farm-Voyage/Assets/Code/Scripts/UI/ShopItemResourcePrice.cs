using Farm.FarmResources;
using UnityEngine;

namespace UI
{
    [System.Serializable]
    public struct ShopItemResourcePrice
    {
        public ResourceType ResourceType;
        [field: Range(1, 100)] public int Price;
    }
}