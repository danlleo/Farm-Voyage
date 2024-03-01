using System;
using System.Collections;
using System.Collections.Generic;
using Attributes.WithinParent;
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
    [DisallowMultipleComponent]
    public class ResourcesGatherer : MonoBehaviour, IInteractable, IDisplayIcon
    {
        public GatheredResourceEvent GatheredResourceEvent { get; private set; }
        public FullyGatheredEvent FullyGatheredEvent { get; private set; }
        
        [field:SerializeField] public IconSO Icon { get; private set; }
        
        [Header("External reference")]
        [SerializeField, WithinParent] private Transform _visualSpawnPoint;
        [SerializeField] private CollectableSO[] _collectableSOArray;
        [SerializeField, Range(1f, 100f)] private float _chanceToGetCollectable;

        private BoxCollider _boxCollider;
        
        private ResourceSO _resourceSO;
        
        private Player _player;
        private PlayerInventory _playerInventory;
        private Tool.Tool _playerTool;
        
        private bool _canGather;
        
        private Coroutine _delayGatheringResourcesRoutine;
        
        private int _timesInteracted;

        private readonly List<Material> _allMaterials = new();

        [Inject]
        private void Construct(Player player, PlayerInventory playerInventory)
        {
            _player = player;
            _playerInventory = playerInventory;
        }

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            GatheredResourceEvent = GetComponent<GatheredResourceEvent>();
            FullyGatheredEvent = GetComponent<FullyGatheredEvent>();
        }

        public void Initialize(ResourceSO resourceSO, Vector3 position, Quaternion rotation)
        {
            _resourceSO = resourceSO;
            transform.SetPositionAndRotation(position, rotation);
            GameObject visualGameObject = Instantiate(resourceSO.VisualObject, _visualSpawnPoint, false);
            SetCanGatherIfPlayerHasRequiredTool();
            GetMaterialsFromVisual(visualGameObject);
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
        
        private void GetMaterialsFromVisual(GameObject visualGameObject)
        {
            Renderer[] renderers = visualGameObject.GetComponentsInChildren<Renderer>(true);

            foreach (Renderer renderer in renderers)
            {
                foreach (Material mat in renderer.materials)
                {
                    _allMaterials.Add(mat);
                }
            }
        }

        private void StopGathering()
        {
            if (_delayGatheringResourcesRoutine != null)
                StopCoroutine(_delayGatheringResourcesRoutine);
            
            _delayGatheringResourcesRoutine = null;
            _player.PlayerGatheringEvent.Call(this,
                new PlayerGatheringEventArgs(false, false, _resourceSO.ResourceToGather, transform));
        }

        private IEnumerator DelayGatheringResourcesRoutine(GatheredResource gatheredResource)
        {
            float delayTime = _playerTool.CalculateTimeToGatherBasedOnLevel();
            float gatherTime = _playerTool.CalculateTimeToGatherBasedOnLevel();

            _player.PlayerGatheringEvent.Call(this,
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
        
        private void DestroyIfFullyGathered()
        {
            if (_timesInteracted != _resourceSO.InteractAmountToDestroy) return;
            
            _boxCollider.Disable();
            FullyGatheredEvent.Call(this);
            _player.PlayerGatheringEvent.Call(this,
                new PlayerGatheringEventArgs(false, true, _resourceSO.ResourceToGather, transform));
        }
        
        private void Gather(GatheredResource gatheredResource)
        {
            GatheredResourceEvent.Call(this, new GatheredResourceEventArgs(gatheredResource.Quantity, _allMaterials));
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
