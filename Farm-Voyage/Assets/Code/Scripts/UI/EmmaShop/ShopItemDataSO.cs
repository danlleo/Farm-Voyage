using System.Collections.Generic;
using System.Linq;
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
            return (from shopItemResourcePrice in _shopItemResourcesPrices
                where shopItemResourcePrice.ResourceType == resourceType
                select shopItemResourcePrice.Price).FirstOrDefault();
        }
    }
}
