using System;
using UnityEngine;
using Zenject;

namespace UI
{
    [DisallowMultipleComponent]
    public class UI : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private GameplayUI _gameplayUI;
        [SerializeField] private EmmaShopUI _emmaShopUI;

        private Market.Market _market;

        [Inject]
        private void Construct(Market.Market market)
        {
            _market = market;
        }
        
        private void OnEnable()
        {
            _market.StartedShoppingEvent.OnStartedShopping += Market_OnStartedShopping;
            _emmaShopUI.OnClosed += EmmaShopUI_OnClosed;
        }
        
        private void OnDisable()
        {
            _market.StartedShoppingEvent.OnStartedShopping -= Market_OnStartedShopping;
            _emmaShopUI.OnClosed -= EmmaShopUI_OnClosed;
        }

        private void Market_OnStartedShopping(object sender, EventArgs e)
        {
            _gameplayUI.gameObject.SetActive(false);
            _emmaShopUI.gameObject.SetActive(true);
        }
        
        private void EmmaShopUI_OnClosed()
        {
            _emmaShopUI.gameObject.SetActive(false);
            _gameplayUI.gameObject.SetActive(true);
            _market.StoppedShoppingEvent.Call(this);
        }
    }
}
