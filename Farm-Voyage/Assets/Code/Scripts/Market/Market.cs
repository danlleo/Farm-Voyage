using System;
using Character.Player;
using UnityEngine;
using Zenject;

namespace Market
{
    [RequireComponent(typeof(StartedShoppingEvent))]
    [DisallowMultipleComponent]
    public class Market : MonoBehaviour
    {
        public StartedShoppingEvent StartedShoppingEvent { get; private set; }

        private Player _player;

        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }
        
        private void Awake()
        {
            StartedShoppingEvent = GetComponent<StartedShoppingEvent>();
        }

        private void OnEnable()
        {
            StartedShoppingEvent.OnStartedShopping += StartedShoppingEvent_OnStartedShopping;
        }

        private void OnDisable()
        {
            StartedShoppingEvent.OnStartedShopping -= StartedShoppingEvent_OnStartedShopping;
        }

        private void StartedShoppingEvent_OnStartedShopping(object sender, EventArgs e)
        {
            _player.PlayerShoppingEvent.Call(this);
        }
    }
}
