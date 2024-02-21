using System;
using System.Collections;
using System.Collections.Generic;
using Attributes.WithinParent;
using Character.Player;
using Common;
using DG.Tweening;
using Misc;
using Misc.ObjectPool;
using UI;
using UI.Icon;
using UnityEngine;
using Utilities;
using Zenject;
using Random = UnityEngine.Random;

namespace Farm.FarmResources
{
    [SelectionBase]
    [RequireComponent(typeof(BoxCollider))]
    [DisallowMultipleComponent]
    public class ResourcesGatherer : MonoBehaviour, IInteractable, IDisplayIcon
    {
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
        private Coroutine _gatherAnimationRoutine;
        
        private int _timesInteracted;

        private readonly List<Material> _allMaterials = new();
        
        // TODO: CLEAN THIS MESS
        
        [Inject]
        private void Construct(Player player, PlayerInventory playerInventory)
        {
            _player = player;
            _playerInventory = playerInventory;
        }

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        public void Initialize(ResourceSO resourceSO, Vector3 position, Quaternion rotation)
        {
            _resourceSO = resourceSO;
            transform.SetPositionAndRotation(position, rotation);
            GameObject visualGameObject = Instantiate(resourceSO.VisualObject, _visualSpawnPoint, false);
            SetCanGatherIfPlayerHasRequiredTool();
            GetMaterialsFromVisual(visualGameObject);
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
            PlayDestroyAnimation();
        }

        private void PlayDestroyAnimation()
        {
            Sequence destroyAnimationSequence = DOTween.Sequence();
            destroyAnimationSequence.Append(transform.DOScale(transform.localScale * 1.25f, .35f));
            destroyAnimationSequence.Append(transform.DOScale(Vector3.zero, .35f));
            destroyAnimationSequence.OnComplete(() => Destroy(gameObject));
        }

        private void PlayGatherAnimation()
        {
            if (_gatherAnimationRoutine != null)
                StopCoroutine(_gatherAnimationRoutine);

            _gatherAnimationRoutine = StartCoroutine(GatherAnimationRoutine(.25f));
        }
        
        private IEnumerator GatherAnimationRoutine(float duration)
        {
            transform.DOShakeScale(duration, 0.5f);

            int flashIntensity = Shader.PropertyToID("_FlashIntensity");
            
            float timer = 0f;
            float durationHalfWay = duration / 2;

            while (timer <= duration)
            {
                foreach (Material material in _allMaterials)
                {
                    timer += Time.deltaTime;
                    
                    float t = timer / durationHalfWay;
                    float value = timer < durationHalfWay 
                        ? Mathf.Lerp(0, 1, t) 
                        : Mathf.Lerp(1, 0, t);
                    
                    material.SetFloat(flashIntensity, value);
                }

                yield return null;
            }
        }
        
        private void Gather(GatheredResource gatheredResource)
        {
            PlayGatherAnimation();
            StopGathering();
            IncreaseTimeInteracted();
            DestroyIfFullyGathered();

            _playerInventory.AddResourceQuantity(gatheredResource.Type, gatheredResource.Quantity);

            PopupText popupText = ObjectPoolManager.SpawnObject(GameResources.Retrieve.PopupText, transform.position,
                transform.rotation, PoolType.GameObject);
            popupText.Initialize(gatheredResource.Quantity, () => ObjectPoolManager.ReturnObjectToPool(popupText));
            
            if (gatheredResource.Type != ResourceType.Dirt) return;
            
            if (TryCollectCollectable(out CollectableSO collectable))
            {
                _player.PlayerFoundCollectableEvent.Call(this);
            }
        }
        
        public class Factory : PlaceholderFactory<ResourcesGatherer> { }
    }
}
