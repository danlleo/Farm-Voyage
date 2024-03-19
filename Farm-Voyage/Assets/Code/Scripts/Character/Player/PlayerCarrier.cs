using Attributes.WithinParent;
using Farm.Corral;
using Misc;
using Unity.Mathematics;
using UnityEngine;

namespace Character.Player
{
    [RequireComponent(typeof(PlayerCarryingStorageBoxStateChangedEvent))]
    [DisallowMultipleComponent]
    public class PlayerCarrier : MonoBehaviour, IValidate
    {
        public bool IsValid { get; private set; } = true;
        
        [Header("External references")]
        [SerializeField, WithinParent] private Transform _carryPoint;
        
        private StorageBox _storageBox;
        private PlayerCarryingStorageBoxStateChangedEvent _playerCarryingStorageBoxStateChangedEvent;
        
        private void Awake()
        {
            _playerCarryingStorageBoxStateChangedEvent = GetComponent<PlayerCarryingStorageBoxStateChangedEvent>();
        }

        private void OnEnable()
        {
            _playerCarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged +=
                PlayerCarryingStorageBoxStateChangedEvent_OnPlayerCarryingStorageBoxStateChanged;
        }

        private void OnDisable()
        {
            _playerCarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged -=
                PlayerCarryingStorageBoxStateChangedEvent_OnPlayerCarryingStorageBoxStateChanged;
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
        
        private void PlayerCarryingStorageBoxStateChangedEvent_OnPlayerCarryingStorageBoxStateChanged(object sender,
            PlayerCarryingStorageBoxStateChangedEventArgs e)
        {
            if (!e.IsCarrying) return;

            CarryStorageBox(e.StorageBox);
        }
    }
}