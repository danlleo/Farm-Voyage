using Farm.Plants.Seeds;
using UnityEngine;

namespace UI.EmmaShop.ConcreteShopItems
{
    public class SeedShopItemUIElement : ShopItemUIElement
    {
        [SerializeField] private SeedType _seedToBuyType;
        [SerializeField, Range(1, 8)] private int _quantityToBuy;
        
        protected override void OnPurchase()
        {
            PlayerInventory.AddSeedQuantity(_seedToBuyType, _quantityToBuy);
            print("Purchased seed");
        }
    }
}