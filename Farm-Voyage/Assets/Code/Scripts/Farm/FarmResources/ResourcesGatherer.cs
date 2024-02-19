using System;
using System.Collections;
using Attributes.WithinParent;
using Character.Player;
using Common;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Farm.FarmResources
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public class ResourcesGatherer : MonoBehaviour, IInteractable
    {
        [Header("External reference")]
        [SerializeField, WithinParent] private Transform _visualSpawnPoint;
        [SerializeField] private CollectableSO[] _collectableSOArray;
        [SerializeField, Range(1f, 100f)] private float _chanceToGetCollectable;
        
        private ResourceSO _resourceSO;
        
        private Player _player;
        private PlayerInventory _playerInventory;
        private Tool.Tool _playerTool;
        
        private bool _canGather;

        private Coroutine _delayGatheringResourcesRoutine;

        private int _timesInteracted;
        
        [Inject]
        private void Construct(Player player, PlayerInventory playerInventory)
        {
            _player = player;
            _playerInventory = playerInventory;
        }

        public void Initialize(ResourceSO resourceSO, Vector3 position, Quaternion rotation)
        {
            _resourceSO = resourceSO;
            transform.SetPositionAndRotation(position, rotation);
            Instantiate(resourceSO.VisualObject, _visualSpawnPoint, false);
            SetCanGatherIfPlayerHasRequiredTool();
        }
        
        public void Interact()
        {
            if (!TryGatherResources(out GatheredResource gatheredResource)) return;

            _delayGatheringResourcesRoutine ??= StartCoroutine(DelayGatheringResourcesRoutine(gatheredResource));
        }

        public void StopInteract()
        {
            StopGathering();
        }

        private void StopGathering()
        {
            if (_delayGatheringResourcesRoutine != null)
                StopCoroutine(_delayGatheringResourcesRoutine);
            
            _delayGatheringResourcesRoutine = null;
            _player.PlayerGatheringEvent.Call(this, new PlayerGatheringEventArgs(false, _resourceSO.ResourceToGather));
        }

        private IEnumerator DelayGatheringResourcesRoutine(GatheredResource gatheredResource)
        {
            float delayTime = CalculateTimeToGatherBasedOnToolLevel(_playerTool);
            float gatherTime = CalculateTimeToGatherBasedOnToolLevel(_playerTool);
            
            _player.PlayerGatheringEvent.Call(this,
                new PlayerGatheringEventArgs(true, _resourceSO.ResourceToGather, gatherTime));
            
            yield return new WaitForSeconds(delayTime);
            
            Gather(gatheredResource);
        }
        
        private bool TryGatherResources(out GatheredResource gatheredResource)
        {
            gatheredResource = new GatheredResource();

            if (!_canGather) return false;
    
            int quantityGathered = CalculateQuantityBasedOnToolLevel(_playerTool);

            if (quantityGathered <= 0) return false;
            
            gatheredResource = new GatheredResource(_resourceSO.ResourceToGather, quantityGathered);
            
            return true;
        }

        private int CalculateQuantityBasedOnToolLevel(Tool.Tool tool)
        {
            const int minimumRandomValue = 1;
            const int maximumRandomValue = 5;

            int calculatedQuantity = tool.Level * Random.Range(minimumRandomValue, maximumRandomValue);
            
            return calculatedQuantity;
        }

        private float CalculateTimeToGatherBasedOnToolLevel(Tool.Tool tool)
        {
            float calculatedTimeReducer = tool.Level == 1 ? 0 : 0.5f * tool.Level;
            float calculatedTime = tool.TimeToGather - calculatedTimeReducer;
            
            return calculatedTime;
        }
        
        private void IncreaseTimeInteracted()
        {
            _timesInteracted++;
        }

        private void DestroyIfFullyGathered()
        {
            if (_timesInteracted != _resourceSO.InteractAmountToDestroy) return;
            
            Destroy(gameObject);
        }

        private void SetCanGatherIfPlayerHasRequiredTool()
        {
            Type toolType = _resourceSO.RequiredToolType;
            
            if (_playerInventory.TryGetToolOfType(toolType, out Tool.Tool tool))
            {
                _playerTool = tool;
                _canGather = true;
                
                return;
            }

            _canGather = false;
        }

        private bool TryCollectCollectable(out CollectableSO collectable)
        {
            // Check if the chance check passes
            if (!(Random.Range(0f, 100f) <= _chanceToGetCollectable))
            {
                collectable = null;
                return false;
            }

            if (_collectableSOArray is not { Length: > 0 }) 
            {
                collectable = null;
                return false;
            }
            
            int randomIndex = Random.Range(0, _collectableSOArray.Length);
            collectable = _collectableSOArray[randomIndex];

            return true;
        }
        
        private void Gather(GatheredResource gatheredResource)
        {
            StopGathering();
            IncreaseTimeInteracted();
            DestroyIfFullyGathered();

            _playerInventory.AddResourceQuantity(gatheredResource.Type, gatheredResource.Quantity);
            
            if (gatheredResource.Type != ResourceType.Dirt) return;
            
            if (TryCollectCollectable(out CollectableSO collectable))
            {
                _player.PlayerFoundCollectableEvent.Call(this);
            }
        }
        
        public class Factory : PlaceholderFactory<ResourcesGatherer> { }
    }
}
