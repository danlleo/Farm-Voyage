using System;
using System.Collections.Generic;
using Character.Player;
using Farm.Corral;
using Farm.Plants;
using UnityEngine;
using Zenject;

namespace Farm
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public class GlobalStorage : MonoBehaviour
    {
        public event Action OnStored;
        
        private PlayerInventory _playerInventory;

        private bool _hasPlayed;
        
        [Inject]
        private void Construct(PlayerInventory playerInventory)
        {
            _playerInventory = playerInventory;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // if (!other.TryGetComponent(out PlayerCarrier playerCarrier)) return;
            // if (!playerCarrier.TryGetStorageBox(out StorageBox storageBox)) return;

            if (_hasPlayed) return;
            
            KeepHarvestedPlants();
        }

        private void KeepHarvestedPlants()
        {
            // foreach (KeyValuePair<PlantType, int> pair in storageBox.ReadPlantsQuantities())
            // {
            //     _playerInventory.AddPlant(pair.Key, pair.Value);
            // }
            //
            // storageBox.ClearAndPutToInitialPosition();
            _hasPlayed = true;
            OnStored?.Invoke();
        }
    }
}
