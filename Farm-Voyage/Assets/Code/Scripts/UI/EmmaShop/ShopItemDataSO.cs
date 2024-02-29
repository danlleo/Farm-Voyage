using System.Collections.Generic;
using Farm.FarmResources;
using UnityEngine;

namespace UI.EmmaShop
{
    [CreateAssetMenu(fileName = "ShopItem_", menuName = "Scriptable Objects/UI/ShopItem")]
    public class ShopItemDataSO : ScriptableObject
    {
        [field:SerializeField] public Sprite Icon { get; private set; }
        public IEnumerable<ShopItemResourcePrice> ShopItemResourcePrices => _shopItemResourcesPrices;
        
        [SerializeField] private List<ShopItemResourcePrice> _shopItemResourcesPrices;

        public int GetPriceByRecourseType(ResourceType resourceType)
        {
            foreach (ShopItemResourcePrice shopItemResourcePrice in  _shopItemResourcesPrices)
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
