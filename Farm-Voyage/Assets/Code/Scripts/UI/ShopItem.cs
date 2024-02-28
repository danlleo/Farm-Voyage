using Attributes.WithinParent;
using Character.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI
{
    [DisallowMultipleComponent]
    public abstract class ShopItem : MonoBehaviour, IPointerClickHandler
    {
        protected PlayerInventory PlayerInventory;
        
        [Header("External references")]
        [SerializeField] protected ShopItemPrice ShopItemPrice;
        [SerializeField, WithinParent] private GameObject _lockedContent;
        [SerializeField, WithinParent] private GameObject _unlockedContent;
        
        [Inject]
        private void Construct(PlayerInventory playerInventory)
        {
            PlayerInventory = playerInventory;
        }

        protected abstract void OnPurchase();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!HasEnoughResources())
            {
                return;
            }
            
            Purchase();
        }

        private void Purchase()
        {
            foreach (ShopItemResourcePrice shopItemResourcePrice in ShopItemPrice.ShopItemResourcesPrices)
            {
                PlayerInventory.RemoveResourceQuantity(shopItemResourcePrice.ResourceType,
                    shopItemResourcePrice.Price);
            }
            
            OnPurchase();
        }
        
        private bool HasEnoughResources()
        {
            foreach (ShopItemResourcePrice shopItemResourcesPrice in ShopItemPrice.ShopItemResourcesPrices)
            {
                int resourceQuantity = PlayerInventory.GetResourceQuantity(shopItemResourcesPrice.ResourceType);

                if (resourceQuantity < shopItemResourcesPrice.Price)
                {
                    print(resourceQuantity);
                    return false;
                }
            }

            return true;
        }
    }
}
