using Farm.Tool;
using UnityEngine;

namespace UI.EmmaShop.ConcreteShopItems
{
    public class ToolShopItem : ShopItem
    {
        [SerializeField] private ToolType _toolToBuyType;
        
        protected override void OnPurchase()
        {
            PlayerInventory.AddTool(_toolToBuyType);
            print("Purchased tool");
        }
    }
}