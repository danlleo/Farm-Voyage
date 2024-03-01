using Attributes.WithinParent;
using Character.Player;
using Farm.FarmResources;
using Level;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.EmmaShop
{
    [DisallowMultipleComponent]
    public abstract class ShopItemUIElement : MonoBehaviour, IPointerClickHandler
    {
        protected PlayerInventory PlayerInventory;
        
        [Header("External references")]
        [SerializeField] protected ShopItemDataSO ShopItemData;
        [SerializeField, WithinParent] private GameObject _lockedContent;
        [SerializeField, WithinParent] private GameObject _unlockedContent;
        [SerializeField, WithinParent] private ShopItemVisualContainer _shopItemVisualContainer;

        private Economy _economy;
        
        [Inject]
        private void Construct(PlayerInventory playerInventory, Economy economy)
        {
            PlayerInventory = playerInventory;
            _economy = economy;
        }

        private void Awake()
        {
            UpdateShopItemContainerVisuals();
        }

        protected abstract void OnPurchase();

        public void OnPointerClick(PointerEventData eventData)
        {
            Purchase();
        }

        private void Purchase()
        {
            if (_economy.TryPurchaseWithResources(ShopItemData.ShopItemResourcePrices))
            {
                OnPurchase();
            }
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
