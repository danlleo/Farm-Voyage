using System;
using UnityEngine;

namespace Character.Emma
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public class Emma : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private Market.Market _market;

        private void OnEnable()
        {
            _market.StartedShoppingEvent.OnStartedShopping += ShoppingEvent_OnStartedShopping;
        }

        private void OnDisable()
        {
            _market.StartedShoppingEvent.OnStartedShopping -= ShoppingEvent_OnStartedShopping;
        }

        private void ShoppingEvent_OnStartedShopping(object sender, EventArgs e)
        {
            // TODO: related
        }
    }
}
