using System.Collections;
using Character.Player;
using Common;
using UnityEngine;
using Zenject;

namespace Farm.Plants
{
    [RequireComponent(typeof(BoxCollider))]
    [DisallowMultipleComponent]
    public class PlantArea : MonoBehaviour, IInteractable
    {
        [Header("External references")] 
        [SerializeField] private PlantFactory _plantFactory;
        
        [Header("Settings")]
        [SerializeField] private PlantType _plantType;
        [SerializeField, Range(0.1f, 2f)] private float _timeToDigInSeconds = 1f;
        
        private Plant _plant;
        private Player _player;
        private Tool _playerTool;
        
        private ToolType _shovel;
        
        private BoxCollider _boxCollider;

        private Coroutine _diggingRoutine;

        private bool _canDig;
        
        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }
        
        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _shovel = ToolType.Shovel;
            EnableCollider();
        }

        public void Interact()
        {
            TryAllowingDigging();

            if (!_canDig) return;
            if (_diggingRoutine == null)
                StartCoroutine(DiggingRoutine());
        }

        public void StopInteract()
        {
            StopDigging();
        }

        private IEnumerator DiggingRoutine()
        {
            _player.PlayerDiggingPlantAreaEvent.Call(this, new PlayerDiggingPlantAreaEventArgs(true));
            yield return new WaitForSeconds(_timeToDigInSeconds);
            SpawnPlant();
            DisableCollider();
        }

        private void StopDigging()
        {
            ClearDiggingRoutine();
            _player.PlayerDiggingPlantAreaEvent.Call(this, new PlayerDiggingPlantAreaEventArgs(true));
        }
        
        public void ClearPlantArea()
        {
            _plant = null;
            EnableCollider();
        }

        private void TryAllowingDigging()
        {
            if (!_player.TryGetTool(_shovel, out Tool tool)) return;
            
            _canDig = true;
            _playerTool = tool;
        }
        
        private void SpawnPlant()
        {
            Plant plant = _plantFactory.CreatePlant(_plantType);
            plant.Initialize(transform.position, Quaternion.identity, this);
            _plant = plant;
        }

        private void ClearDiggingRoutine()
        {
            if (_diggingRoutine == null) return;
            
            StopCoroutine(_diggingRoutine);
            _diggingRoutine = null;
        }
        
        private void EnableCollider()
        {
            _boxCollider.enabled = true;
        }
        
        private void DisableCollider()
        {
            _boxCollider.enabled = false;
        }
    }
}
