using System;
using System.Collections;
using Attributes.WithinParent;
using Character;
using Character.Player;
using Common;
using Farm.FarmResources;
using UI.Icon;
using UnityEngine;
using Utilities;
using Zenject;
using Random = UnityEngine.Random;

namespace Farm.ResourceGatherer
{
    [SelectionBase]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(GatheredResourceEvent))]
    [RequireComponent(typeof(FullyGatheredEvent))]
    [RequireComponent(typeof(GatheringStateChangedEvent))]
    [RequireComponent(typeof(ResourcesGathererInitializeEvent))]
    [DisallowMultipleComponent]
    public class ResourcesGatherer : MonoBehaviour, IInteractable, IStopInteractable, IDisplayIcon
    {
        [field:SerializeField] public IconSO Icon { get; private set; }
        public Guid ID { get; } = Guid.NewGuid();

        [Header("External reference")]
        [SerializeField, WithinParent] private Transform _visualSpawnPoint;
        [SerializeField] private CollectableSO[] _collectableSOArray;
        [SerializeField, Range(1f, 100f)] private float _chanceToGetCollectable;
        
        private GatheredResourceEvent _gatheredResourceEvent;
        private FullyGatheredEvent _fullyGatheredEvent;
        private GatheringStateChangedEvent _gatheringStateChangedEvent;
        private ResourcesGathererInitializeEvent _resourcesGathererInitializeEvent;
        
        private BoxCollider _boxCollider;
        
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

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _gatheredResourceEvent = GetComponent<GatheredResourceEvent>();
            _fullyGatheredEvent = GetComponent<FullyGatheredEvent>();
            _gatheringStateChangedEvent = GetComponent<GatheringStateChangedEvent>();
            _resourcesGathererInitializeEvent = GetComponent<ResourcesGathererInitializeEvent>();
        }

        public void Initialize(ResourceSO resourceSO, Vector3 position, Quaternion rotation)
        {
            _resourceSO = resourceSO;
            transform.SetPositionAndRotation(position, rotation);
            GameObject visualGameObject = Instantiate(resourceSO.VisualObject, _visualSpawnPoint, false);
            SetCanGatherIfPlayerHasRequiredTool();
            _resourcesGathererInitializeEvent.Call(this, new ResourcesGathererInitializeEventArgs(visualGameObject));
        }

        public void Interact(ICharacter initiator)
        {
            if (!TryGatherResources(out GatheredResource gatheredResource)) return;

            CoroutineHandler.StartAndAssignIfNull(this, ref _delayGatheringResourcesRoutine,
                DelayGatheringResourcesRoutine(gatheredResource));
        }

        public void StopInteract(Player player)
        {
            StopGathering();
        }

        private void StopGathering()
        {
            CoroutineHandler.ClearAndStopCoroutine(this, ref _delayGatheringResourcesRoutine);
            
            _gatheringStateChangedEvent.Call(this, new GatheringStateChangedEventArgs(false));
            _player.PlayerEvents.PlayerGatheringEvent.Call(this,
                new PlayerGatheringEventArgs(false, false, _resourceSO.ResourceToGather, transform));
        }

        private IEnumerator DelayGatheringResourcesRoutine(GatheredResource gatheredResource)
        {
            float delayTime = _playerTool.CalculateTimeToGatherBasedOnLevel();
            float gatherTime = _playerTool.CalculateTimeToGatherBasedOnLevel();

            _gatheringStateChangedEvent.Call(this, new GatheringStateChangedEventArgs(true));
            _player.PlayerEvents.PlayerGatheringEvent.Call(this,
                new PlayerGatheringEventArgs(true, false, _resourceSO.ResourceToGather, transform, gatherTime));
            
            yield return new WaitForSeconds(delayTime);
            
            Gather(gatheredResource);
        }
        
        private bool TryGatherResources(out GatheredResource gatheredResource)
        {
            gatheredResource = new GatheredResource();

            if (!_canGather) return false;

            int quantityGathered = _playerTool.CalculateQuantityBasedOnLevel();

            if (quantityGathered <= 0) return false;
            
            gatheredResource = new GatheredResource(_resourceSO.ResourceToGather, quantityGathered);
            
            return true;
        }
        
        private void IncreaseTimeInteracted()
        {
            _timesInteracted++;
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

        private bool TryGatherCollectable(out CollectableSO collectable)
        {
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
        
        private void DestroyIfFullyGathered()
        {
            if (_timesInteracted != _resourceSO.InteractAmountToDestroy) return;
            
            _boxCollider.Disable();
            _fullyGatheredEvent.Call(this);
            _player.PlayerEvents.PlayerGatheringEvent.Call(this,
                new PlayerGatheringEventArgs(false, true, _resourceSO.ResourceToGather, transform));
        }
        
        private void Gather(GatheredResource gatheredResource)
        {
            StopGathering();
            IncreaseTimeInteracted();
            
            _playerInventory.AddResourceQuantity(gatheredResource.Type, gatheredResource.Quantity);
            _gatheredResourceEvent.Call(this,
                new GatheredResourceEventArgs(gatheredResource.Quantity, _timesInteracted,
                    _resourceSO.InteractAmountToDestroy));
            
            DestroyIfFullyGathered();
            
            if (gatheredResource.Type != ResourceType.Dirt) return;
            
            if (TryGatherCollectable(out CollectableSO collectable))
            {
                _player.PlayerEvents.PlayerFoundCollectableEvent.Call(this);
            }
        }
        
        public class Factory : PlaceholderFactory<ResourcesGatherer> { }
    }
}