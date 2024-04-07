using System;
using System.Collections;
using Attributes.WithinParent;
using Character.Player;
using Character.Player.Events;
using Common;
using Farm.FarmResources;
using Misc;
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
    public class ResourcesGatherer : MonoBehaviour, IInteractable, IStopInteractable, IDisplayIcon, IInteractDisplayProgress
    {
        public Observable<float> CurrentClampedProgress { get; private set; } = new();
        public float MaxClampedProgress { get; private set; }
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
            MaxClampedProgress = 1f;
            transform.SetPositionAndRotation(position, rotation);
            GameObject visualGameObject = Instantiate(resourceSO.VisualObject, _visualSpawnPoint, false);
            SetCanGatherIfPlayerHasRequiredTool();
            _resourcesGathererInitializeEvent.Call(this, new ResourcesGathererInitializeEventArgs(visualGameObject));
        }

        public void Interact(IVisitable initiator)
        {
            if (!TryGatherResources(out GatheredResource gatheredResource)) return;

            CoroutineHandler.StartAndAssignIfNull(this, ref _delayGatheringResourcesRoutine,
                DelayGatheringResourcesRoutine(gatheredResource));
        }

        public void StopInteract()
        {
            StopGathering();
        }

        private void StopGathering()
        {
            CoroutineHandler.ClearAndStopCoroutine(this, ref _delayGatheringResourcesRoutine);
            
            _gatheringStateChangedEvent.Call(this, new GatheringStateChangedEventArgs(false));
            _player.Events.GatheringEvent.Call(this,
                new PlayerGatheringEventArgs(false, false, _resourceSO.ResourceToGather, transform));
        }

        private IEnumerator DelayGatheringResourcesRoutine(GatheredResource gatheredResource)
        {
            float delayTime = _playerTool.CalculateReducedGatherTime();
            float gatherTime = _playerTool.CalculateReducedGatherTime();

            _gatheringStateChangedEvent.Call(this, new GatheringStateChangedEventArgs(true));
            _player.Events.GatheringEvent.Call(this,
                new PlayerGatheringEventArgs(true, false, _resourceSO.ResourceToGather, transform, gatherTime));
            
            yield return new WaitForSeconds(delayTime);
            
            Gather(gatheredResource);
        }
        
        private bool TryGatherResources(out GatheredResource gatheredResource)
        {
            gatheredResource = new GatheredResource();

            if (!_canGather) return false;

            int quantityGathered = _playerTool.CalculateRandomQuantityBasedOnLevel();

            if (quantityGathered <= 0) return false;
            
            gatheredResource = new GatheredResource(_resourceSO.ResourceToGather, quantityGathered);
            
            return true;
        }
        
        private void IncreaseTimesInteracted()
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
        
        private void CleanUpResourceIfFullyGathered()
        {
            if (_timesInteracted != _resourceSO.InteractAmountToDestroy) return;
            
            _boxCollider.Disable();
            _fullyGatheredEvent.Call(this);
            _player.Events.GatheringEvent.Call(this,
                new PlayerGatheringEventArgs(false, true, _resourceSO.ResourceToGather, transform));
        }
        
        private void TryAddResourceToInventory(GatheredResource gatheredResource)
        {
            _playerInventory.AddResourceQuantity(gatheredResource.Type, gatheredResource.Quantity);
        }

        private void TriggerGatheredResourceEvent(GatheredResource gatheredResource)
        {
            _gatheredResourceEvent.Call(this,
                new GatheredResourceEventArgs(gatheredResource.Quantity, _timesInteracted,
                    _resourceSO.InteractAmountToDestroy));
        }
        
        private void AttemptToGatherCollectableIfDirt(GatheredResource gatheredResource)
        {
            if (gatheredResource.Type == ResourceType.Dirt && TryGatherCollectable(out CollectableSO collectable))
            {
                _player.Events.FoundCollectableEvent.Call(this);
            }
        }
        
        private void UpdateProgressBar()
        {
            CurrentClampedProgress.Value = (float)_timesInteracted / _resourceSO.InteractAmountToDestroy;
        }

        private void Gather(GatheredResource gatheredResource)
        {
            StopGathering();
            IncreaseTimesInteracted();
            TryAddResourceToInventory(gatheredResource);
            TriggerGatheredResourceEvent(gatheredResource);
            UpdateProgressBar();
            CleanUpResourceIfFullyGathered();
            AttemptToGatherCollectableIfDirt(gatheredResource);
        }

        public class Factory : PlaceholderFactory<ResourcesGatherer> { }
    }
}