using Character.Player;
using Farm.Corral;
using UnityEngine;

namespace Farm
{
    [DisallowMultipleComponent]
    public class GlobalStorage : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player player)) return;
            if (!player.TryGetStorageBox(out StorageBox storageBox)) return;
            
            storageBox.ClearAndPutToInitialPosition();
        }
    }
}
