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
        private PlayerInventory _playerInventory;

        [Inject]
        private void Construct(PlayerInventory playerInventory)
        {
            _playerInventory = playerInventory;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerCarrier playerCarrier)) return;
            if (!playerCarrier.TryGetStorageBox(out StorageBox storageBox)) return;
            
            KeepHarvestedPlants(storageBox);
        }

        private void KeepHarvestedPlants(StorageBox storageBox)
        {
            foreach (KeyValuePair<PlantType, int> pair in storageBox.ReadPlantsQuantities())
            {
                _playerInventory.AddPlant(pair.Key, pair.Value);
            }

            storageBox.ClearAndPutToInitialPosition();
        }
    }
}
