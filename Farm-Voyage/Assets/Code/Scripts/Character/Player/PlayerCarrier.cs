using Attributes.WithinParent;
using Character.Player.Events;
using Farm.Corral;
using Misc;
using Unity.Mathematics;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerCarrier : MonoBehaviour, IValidate
    {
        public bool IsValid { get; private set; } = true;
        
        [Header("External references")]
        [SerializeField, WithinParent] private Transform _carryPoint;
        [SerializeField] private Player _player;
        
        private StorageBox _storageBox;
        
        private void OnEnable()
        {
            _player.Events.CarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged +=
                CarryingStorageBoxStateChangedEventOnCarryingStorageBoxStateChanged;
        }

        private void OnDisable()
        {
            _player.Events.CarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged -=
                CarryingStorageBoxStateChangedEventOnCarryingStorageBoxStateChanged;
        }

        private void OnValidate()
        {
            IsValid = true;

            if (_carryPoint == null)
            {
                IsValid = false;
            }
        }
        
        public bool TryGetStorageBox(out StorageBox storageBox)
        {
            if (_storageBox == null)
            {
                storageBox = null;
                return false;
            }

            storageBox = _storageBox;
            return true;
        }
        
        private void CarryStorageBox(StorageBox storageBox)
        {
            if (_storageBox != null) return;

            storageBox.transform.SetParent(_carryPoint);
            storageBox.transform.SetLocalPositionAndRotation(Vector3.zero, quaternion.identity);

            _storageBox = storageBox;
        }
        
        private void CarryingStorageBoxStateChangedEventOnCarryingStorageBoxStateChanged(object sender,
            PlayerCarryingStorageBoxStateChangedEventArgs e)
        {
            if (!e.IsCarrying) return;

            CarryStorageBox(e.StorageBox);
        }
    }
}