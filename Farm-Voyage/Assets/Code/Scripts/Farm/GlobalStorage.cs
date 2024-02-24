using Character.Player;
using Farm.Corral;
using UnityEngine;

namespace Farm
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public class GlobalStorage : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerCarrier playerCarrier)) return;
            if (!playerCarrier.TryGetStorageBox(out StorageBox storageBox)) return;
            
            storageBox.ClearAndPutToInitialPosition();
        }
    }
}
