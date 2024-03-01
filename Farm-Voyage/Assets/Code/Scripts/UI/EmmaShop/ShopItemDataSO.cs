using System.Collections.Generic;
using Farm;
using Farm.FarmResources;
using UnityEngine;

namespace UI.EmmaShop
{
    [CreateAssetMenu(fileName = "ShopItem_", menuName = "Scriptable Objects/UI/ShopItem")]
    public class ShopItemDataSO : ScriptableObject
    {
        [field:SerializeField] public Sprite Icon { get; private set; }
        public IEnumerable<ResourcePrice> ShopItemResourcePrices => _shopItemResourcesPrices;
        public bool CanPurchaseMultiple { get; private set; }
        
        [SerializeField] private List<ResourcePrice> _shopItemResourcesPrices;

        public int GetPriceByRecourseType(ResourceType resourceType)
        {
            foreach (ResourcePrice shopItemResourcePrice in  _shopItemResourcesPrices)
            {
                if (shopItemResourcePrice.ResourceType == resourceType)
                {
                    return shopItemResourcePrice.Price;
                }
            }

            return 0;
        }
    }
}
