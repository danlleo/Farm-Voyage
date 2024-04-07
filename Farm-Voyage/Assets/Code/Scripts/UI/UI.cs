using System;
using Attributes.WithinParent;
using Character.Player;
using Character.Player.Events;
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
            _market.StartedShoppingEvent.OnStartedShopping += Market_OnStartedShopping;
            _workbench.StartedUsingWorkbenchEvent.OnStartedUsingWorkbench += Workbench_OnStartedUsingWorkbench;
            _emmaShopUI.OnClosed += EmmaShopUI_OnClosed;
            _player.Events.ExtractingWaterEvent.OnPlayerExtractingWater += Player_OnExtractingWater;
            _seller.StartedSellingEvent.OnStartedSelling += Seller_OnStartedSelling;
        }

        private void OnDisable()
        {
            SceneTransition.OnAnySceneTransitionEnded -= SceneTransition_OnAnySceneTransitionEnded;
            PlayerInteract.OnAnyInteractDisplayProgressSpotted -= PlayerInteract_OnAnyInteractDisplayProgressSpotted;
            PlayerInteract.OnAnyInteractDisplayProgressLost -= PlayerInteract_OnAnyInteractDisplayProgressLost;
            _market.StartedShoppingEvent.OnStartedShopping -= Market_OnStartedShopping;
            _workbench.StartedUsingWorkbenchEvent.OnStartedUsingWorkbench -= Workbench_OnStartedUsingWorkbench;
            _emmaShopUI.OnClosed -= EmmaShopUI_OnClosed;
            _player.Events.ExtractingWaterEvent.OnPlayerExtractingWater -= Player_OnExtractingWater;
            _seller.StartedSellingEvent.OnStartedSelling -= Seller_OnStartedSelling;
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
        
        private void Market_OnStartedShopping(object sender, EventArgs e)
        {
            _gameplayUI.gameObject.SetActive(false);
            _emmaShopUI.gameObject.SetActive(true);
        }
        
        private void Workbench_OnStartedUsingWorkbench(object sender, EventArgs e)
        {
            _gameplayUI.gameObject.SetActive(false);
            _workbenchUI.gameObject.SetActive(true);
        }
        
        private void EmmaShopUI_OnClosed()
        {
            _emmaShopUI.gameObject.SetActive(false);
            _gameplayUI.gameObject.SetActive(true);
            _market.StoppedShoppingEvent.Call(this);
        }
        
        private void Player_OnExtractingWater(object sender, PlayerExtractingWaterEventArgs e)
        {
            if (e.IsExtracting)
            {
                _gameplayUI.gameObject.SetActive(false);
                _wellUI.gameObject.SetActive(true);

                return;
            }
            
            _gameplayUI.gameObject.SetActive(true);
            _wellUI.gameObject.SetActive(false);
        }
        
        private void Seller_OnStartedSelling(object sender, EventArgs e)
        {
            _gameplayUI.gameObject.SetActive(false);
            _sellerUI.gameObject.SetActive(true);
        }
    }
}
