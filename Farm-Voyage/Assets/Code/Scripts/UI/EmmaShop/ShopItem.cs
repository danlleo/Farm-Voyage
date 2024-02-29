using Attributes.WithinParent;
using Character.Player;
using Farm.FarmResources;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.EmmaShop
{
    [DisallowMultipleComponent]
    public abstract class ShopItem : MonoBehaviour, IPointerClickHandler
    {
        protected PlayerInventory PlayerInventory;
        
        [Header("External references")]
        [SerializeField] protected ShopItemDataSO ShopItemData;
        [SerializeField, WithinParent] private GameObject _lockedContent;
        [SerializeField, WithinParent] private GameObject _unlockedContent;
        [SerializeField, WithinParent] private ShopItemVisualContainer _shopItemVisualContainer;
        
        [Inject]
        private void Construct(PlayerInventory playerInventory)
        {
            PlayerInventory = playerInventory;
        }

        private void Awake()
        {
            UpdateShopItemContainerVisuals();
        }

        protected abstract void OnPurchase();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!HasEnoughResources())
            {
                print("Not enough resources");
                return;
            }
            
            Purchase();
        }

        private void Purchase()
        {
            foreach (ShopItemResourcePrice shopItemResourcePrice in ShopItemData.ShopItemResourcePrices)
            {
                PlayerInventory.RemoveResourceQuantity(shopItemResourcePrice.ResourceType,
                    shopItemResourcePrice.Price);
            }
            
            OnPurchase();
        }
        
        private bool HasEnoughResources()
        {
            foreach (ShopItemResourcePrice shopItemResourcesPrice in ShopItemData.ShopItemResourcePrices)
            {
                int resourceQuantity = PlayerInventory.GetResourceQuantity(shopItemResourcesPrice.ResourceType);

                if (resourceQuantity < shopItemResourcesPrice.Price)
                {
                    return false;
                }
            }

            return true;
        }
        
        private void UpdateShopItemContainerVisuals()
        {
            Sprite sprite = ShopItemData.Icon;
            
            int dirtPrice = ShopItemData.GetPriceByRecourseType(ResourceType.Dirt);
            int woodPrice = ShopItemData.GetPriceByRecourseType(ResourceType.Wood);
            int rockPrice = ShopItemData.GetPriceByRecourseType(ResourceType.Rock);
            
            _shopItemVisualContainer.Initialize(sprite, dirtPrice, woodPrice, rockPrice);
        }
    }
}
