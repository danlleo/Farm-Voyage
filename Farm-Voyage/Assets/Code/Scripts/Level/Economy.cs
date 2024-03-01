using System.Collections.Generic;
using Character.Player;
using Farm;
using Zenject;

namespace Level
{
    public class Economy
    {
        private PlayerInventory _playerInventory;

        [Inject]
        private void Construct(PlayerInventory playerInventory)
        {
            _playerInventory = playerInventory;
        }
        
        public bool TryPurchaseWithResources(IEnumerable<ResourcePrice> resourcePrices)
        {
            if (!HasEnoughResources(resourcePrices)) 
                return false;
            
            foreach (ResourcePrice shopItemResourcePrice in resourcePrices)
            {
                _playerInventory.RemoveResourceQuantity(shopItemResourcePrice.ResourceType,
                    shopItemResourcePrice.Price);
            }

            return true;
        }
        
        private bool HasEnoughResources(IEnumerable<ResourcePrice> resourcePrices)
        {
            foreach (ResourcePrice shopItemResourcesPrice in resourcePrices)
            {
                int resourceQuantity = _playerInventory.GetResourceQuantity(shopItemResourcesPrice.ResourceType);

                if (resourceQuantity < shopItemResourcesPrice.Price)
                {
                    return false;
                }
            }

            return true;
        }
    }
}