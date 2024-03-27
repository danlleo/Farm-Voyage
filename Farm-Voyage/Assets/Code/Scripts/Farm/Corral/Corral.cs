using System;
using Attributes.WithinParent;
using Character.Player;
using Farm.Plants;
using Misc;
using UnityEngine;
using Zenject;

namespace Farm.Corral
{
    [RequireComponent(typeof(BoxCollider))]
    [DisallowMultipleComponent]
    public class Corral : MonoBehaviour, IValidate
    {
        public bool IsValid { get; private set; } = true;
        
        [Header("External references")] 
        [SerializeField] private CorralCardinalDirection _corralCardinalDirection;
        [SerializeField, WithinParent] private PlantArea[] _plantAreaArray;
        [SerializeField, WithinParent] private StorageBox _storageBox;
        
        private Player _player;
        private PlantFactory _plantFactory;
        private PlayerFollowCamera _playerFollowCamera;
        
        [Inject]
        private void Construct(Player player, PlantFactory plantFactory, PlayerFollowCamera playerFollowCamera)
        {
            _player = player;
            _plantFactory = plantFactory;
            _playerFollowCamera = playerFollowCamera;
        }
        
        private void Start()
        {
            InitializePlantAreaArrayItems();
            InitializeStorageBox();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player _)) return;

            HandleCameraIn();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Player _)) return;

            HandleCameraOut();
        }

        private void OnValidate()
        {
            IsValid = true;
            
            if (_plantAreaArray.Length == 0)
            {
                IsValid = false;
                return;
            }

            if (_storageBox == null)
            {
                IsValid = false;
            }
        }
        
        private void HandleCameraIn()
        {
            switch (_corralCardinalDirection)
            {
                case CorralCardinalDirection.North:
                    _playerFollowCamera.ZoomIn();
                    break;
                case CorralCardinalDirection.East:
                    _playerFollowCamera.ZoomIn();
                    _playerFollowCamera.RotateCameraTowardsAngles(new Vector2(55f, 220f));
                    break;
                case CorralCardinalDirection.South:
                    _playerFollowCamera.ZoomIn();
                    break;
                case CorralCardinalDirection.West:
                    _playerFollowCamera.ZoomIn();
                    _playerFollowCamera.RotateCameraTowardsAngles(new Vector2(55f, -220f));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void HandleCameraOut()
        {
            switch (_corralCardinalDirection)
            {
                case CorralCardinalDirection.North:
                    _playerFollowCamera.ZoomOut();
                    break;
                case CorralCardinalDirection.East:
                    _playerFollowCamera.ZoomOut();
                    _playerFollowCamera.ResetCameraRotation();
                    break;
                case CorralCardinalDirection.South:
                    _playerFollowCamera.ZoomOut();
                    break;
                case CorralCardinalDirection.West:
                    _playerFollowCamera.ZoomOut();
                    _playerFollowCamera.ResetCameraRotation();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void InitializePlantAreaArrayItems()
        {
            foreach (PlantArea plantArea in _plantAreaArray)
            {
                plantArea.Initialize(_player, _plantFactory);
            }
        }

        private void InitializeStorageBox()
        {
            _storageBox.Initialize(this, _player);
        }
    }
}
