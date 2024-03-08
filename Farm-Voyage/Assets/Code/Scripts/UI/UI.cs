using System;
using UI.EmmaShop;
using UI.Workbench;
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
        [SerializeField] private WorkbenchUI _workbenchUI;
        
        private Market.Market _market;
        private global::Workbench.Workbench _workbench;
        
        [Inject]
        private void Construct(Market.Market market, global::Workbench.Workbench workbench)
        {
            _market = market;
            _workbench = workbench;
        }

        private void Awake()
        {
            _gameplayUI.gameObject.SetActive(false);
            _emmaShopUI.gameObject.SetActive(false);
            _workbenchUI.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            SceneTransition.OnAnySceneTransitionEnded += SceneTransition_OnAnySceneTransitionEnded;
            _market.StartedShoppingEvent.OnStartedShopping += Market_OnStartedShopping;
            _workbench.StartedUsingWorkbenchEvent.OnStartedUsingWorkbench += Workbench_OnStartedUsingWorkbench;
            _emmaShopUI.OnClosed += EmmaShopUI_OnClosed;
        }

        private void OnDisable()
        {
            SceneTransition.OnAnySceneTransitionEnded -= SceneTransition_OnAnySceneTransitionEnded;
            _market.StartedShoppingEvent.OnStartedShopping -= Market_OnStartedShopping;
            _workbench.StartedUsingWorkbenchEvent.OnStartedUsingWorkbench -= Workbench_OnStartedUsingWorkbench;
            _emmaShopUI.OnClosed -= EmmaShopUI_OnClosed;
        }

        private void SceneTransition_OnAnySceneTransitionEnded()
        {
            _gameplayUI.gameObject.SetActive(true);
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
    }
}
