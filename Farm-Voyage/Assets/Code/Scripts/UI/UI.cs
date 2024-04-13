using Attributes.WithinParent;
using Character.Player;
using Character.Player.Locomotion;
using Misc;
using UI.EmmaShop;
using UI.Seller;
using UI.Workbench;
using UnityEngine;
using Zenject;

namespace UI
{
    [DisallowMultipleComponent]
    public class UI : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField, WithinParent] private GameplayUI _gameplayUI;
        [SerializeField, WithinParent] private EmmaShopUI _emmaShopUI;
        [SerializeField, WithinParent] private WorkbenchUI _workbenchUI;
        [SerializeField, WithinParent] private WellUI _wellUI;
        [SerializeField, WithinParent] private SellerUI _sellerUI;
        [SerializeField, WithinParent] private ActionProgressBarUI _actionProgressBarUI;
        
        private Player _player;
        private Market.Market _market;
        private global::Workbench.Workbench _workbench;
        private global::Seller.Seller _seller;
        
        [Inject]
        private void Construct(Player player, Market.Market market, global::Workbench.Workbench workbench,
            global::Seller.Seller seller)
        {
            _player = player;
            _market = market;
            _workbench = workbench;
            _seller = seller;
        }

        private void Awake()
        {
            _gameplayUI.gameObject.SetActive(false);
            _emmaShopUI.gameObject.SetActive(false);
            _workbenchUI.gameObject.SetActive(false);
            _wellUI.gameObject.SetActive(false);
            _sellerUI.gameObject.SetActive(false);
            _actionProgressBarUI.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            SceneTransition.OnAnySceneTransitionEnded += SceneTransition_OnAnySceneTransitionEnded;
            PlayerInteract.OnAnyInteractDisplayProgressSpotted += PlayerInteract_OnAnyInteractDisplayProgressSpotted;
            PlayerInteract.OnAnyInteractDisplayProgressLost += PlayerInteract_OnAnyInteractDisplayProgressLost;
            _market.ShoppingStateChangedEvent.OnShoppingStateChanged += Market_OnShoppingStateChanged;
            _workbench.UsingWorkbenchStateChangedEvent.OnUsingWorkbenchStateChanged +=
                Workbench_OnUsingWorkbenchStateChanged;
            _workbenchUI.OnClosed += WorkbenchUI_OnClosed;
            _emmaShopUI.OnClosed += EmmaShopUI_OnClosed;
            _player.Events.ExtractingWaterStateChangedEvent.OnPlayerExtractingWaterStateChanged +=
                Player_OnExtractingWaterStateChanged;
            _seller.SellingStateChangedEvent.OnSellingStateChanged += Seller_OnSellingStateChanged;
            _sellerUI.OnClosed += SellerUI_OnClosed;
        }

        private void OnDisable()
        {
            SceneTransition.OnAnySceneTransitionEnded -= SceneTransition_OnAnySceneTransitionEnded;
            PlayerInteract.OnAnyInteractDisplayProgressSpotted -= PlayerInteract_OnAnyInteractDisplayProgressSpotted;
            PlayerInteract.OnAnyInteractDisplayProgressLost -= PlayerInteract_OnAnyInteractDisplayProgressLost;
            _market.ShoppingStateChangedEvent.OnShoppingStateChanged -= Market_OnShoppingStateChanged;
            _workbench.UsingWorkbenchStateChangedEvent.OnUsingWorkbenchStateChanged -=
                Workbench_OnUsingWorkbenchStateChanged;
            _workbenchUI.OnClosed -= WorkbenchUI_OnClosed;
            _emmaShopUI.OnClosed -= EmmaShopUI_OnClosed;
            _player.Events.ExtractingWaterStateChangedEvent.OnPlayerExtractingWaterStateChanged -=
                Player_OnExtractingWaterStateChanged;
            _seller.SellingStateChangedEvent.OnSellingStateChanged -= Seller_OnSellingStateChanged;
            _sellerUI.OnClosed -= SellerUI_OnClosed;
        }

        private void SceneTransition_OnAnySceneTransitionEnded()
        {
            _gameplayUI.gameObject.SetActive(true);
        }

        private void PlayerInteract_OnAnyInteractDisplayProgressSpotted(Observable<float> currentClampedProgress,
            float maxClampedProgress)
        {
            if (_actionProgressBarUI.gameObject.activeSelf) return;

            _actionProgressBarUI.gameObject.SetActive(true);
            _actionProgressBarUI.StartProgress(currentClampedProgress, maxClampedProgress);
        }

        private void PlayerInteract_OnAnyInteractDisplayProgressLost()
        {
            if (!_actionProgressBarUI.gameObject.activeSelf)
                return;
            
            _actionProgressBarUI.gameObject.SetActive(false);
        }
        
        private void Market_OnShoppingStateChanged(bool isShopping)
        {
            if (!isShopping) return;
            
            _gameplayUI.gameObject.SetActive(false);
            _emmaShopUI.gameObject.SetActive(true);
        }
        
        private void Workbench_OnUsingWorkbenchStateChanged(bool isUsingWorkbench)
        {
            if (!isUsingWorkbench) return;
            
            _gameplayUI.gameObject.SetActive(false);
            _workbenchUI.gameObject.SetActive(true);
        }
        
        private void WorkbenchUI_OnClosed()
        {
            _workbenchUI.gameObject.SetActive(false);
            _gameplayUI.gameObject.SetActive(true);
            _workbench.UsingWorkbenchStateChangedEvent.Call(false);
        }
        
        private void EmmaShopUI_OnClosed()
        {
            _emmaShopUI.gameObject.SetActive(false);
            _gameplayUI.gameObject.SetActive(true);
            _market.ShoppingStateChangedEvent.Call(false);
        }
        
        private void Player_OnExtractingWaterStateChanged(bool isExtractingWater)
        {
            if (!isExtractingWater)
            {
                _gameplayUI.gameObject.SetActive(false);
                _wellUI.gameObject.SetActive(true);

                return;
            }
            
            _gameplayUI.gameObject.SetActive(true);
            _wellUI.gameObject.SetActive(false);
        }
        
        private void Seller_OnSellingStateChanged(bool isSelling)
        {
            if (!isSelling) return;
            
            _gameplayUI.gameObject.SetActive(false);
            _sellerUI.gameObject.SetActive(true);
        }
        
        private void SellerUI_OnClosed()
        {
            _sellerUI.gameObject.SetActive(false);
            _gameplayUI.gameObject.SetActive(true);
            _seller.SellingStateChangedEvent.Call(false);
        }
    }
}
