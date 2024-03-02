using System;
using System.Collections.Generic;
using Attributes.WithinParent;
using DG.Tweening;
using Misc;
using Misc.ObjectPool;
using UI;
using UnityEngine;

namespace Farm.ResourceGatherer
{
    [RequireComponent(typeof(GatheredResourceEvent))]
    [RequireComponent(typeof(FullyGatheredEvent))]
    [RequireComponent(typeof(GatheringStateChangedEvent))]
    [RequireComponent(typeof(ResourcesGathererInitializeEvent))]
    [DisallowMultipleComponent]
    public class ResourceGathererAnimator : MonoBehaviour
    {
        private static readonly int s_progressAmount = Shader.PropertyToID("_ProgressAmount");
        private static readonly int s_flashIntensity = Shader.PropertyToID("_FlashIntensity");
        
        [Header("External references")]
        [SerializeField, WithinParent] private MeshRenderer _progressCircle;
        
        [Header("Settings")]
        [SerializeField] private float _circleProgressMoveDurationInSeconds = .2f;
        [SerializeField] private float _gatherAnimationDurationInSeconds = .25f;
        
        private GatheredResourceEvent _gatheredResourceEvent;
        private FullyGatheredEvent _fullyGatheredEvent;
        private GatheringStateChangedEvent _gatheringStateChangedEvent;
        private ResourcesGathererInitializeEvent _resourcesGathererInitializeEvent;
        
        private Coroutine _gatherAnimationRoutine;

        private Material _circleProgressMaterial;
        
        private readonly List<Material> _allMaterials = new();
        
        private void Awake()
        {
            _gatheredResourceEvent = GetComponent<GatheredResourceEvent>();
            _fullyGatheredEvent = GetComponent<FullyGatheredEvent>();
            _gatheringStateChangedEvent = GetComponent<GatheringStateChangedEvent>();
            _resourcesGathererInitializeEvent = GetComponent<ResourcesGathererInitializeEvent>();
            
            _circleProgressMaterial = _progressCircle.material;
            
            ToggleProgressCircle(false);
        }

        private void OnEnable()
        {
            _resourcesGathererInitializeEvent.OnResourcesGathererInitialize += ResourcesGathererInitializeEvent_OnResourcesGathererInitialize;
            _gatheredResourceEvent.OnGatheredResource += GatheredResourceEvent_OnGatheredResource;
            _gatheringStateChangedEvent.OnGatheringStateChanged += GatheringStateChangedEvent_OnGatheringStateChanged;
            _fullyGatheredEvent.OnFullyGathered += FullyGatheredEvent_OnFullyGathered;
        }

        private void OnDisable()
        {
            _resourcesGathererInitializeEvent.OnResourcesGathererInitialize -= ResourcesGathererInitializeEvent_OnResourcesGathererInitialize;
            _gatheredResourceEvent.OnGatheredResource -= GatheredResourceEvent_OnGatheredResource;
            _gatheringStateChangedEvent.OnGatheringStateChanged -= GatheringStateChangedEvent_OnGatheringStateChanged;
            _fullyGatheredEvent.OnFullyGathered -= FullyGatheredEvent_OnFullyGathered;
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

            transform.DOShakeScale(_gatherAnimationDurationInSeconds, 0.5f);
            
            float halfDuration = _gatherAnimationDurationInSeconds / 2;

            foreach (Material material in _allMaterials)
            {
                material.DOFloat(1f, s_flashIntensity, halfDuration)
                    .OnComplete(() => material.DOFloat(0f, s_flashIntensity, halfDuration));
            }
        }
        
        private void UpdateProgressCircle(int timesInteracted, int interactAmountToDestroy)
        {
            float normalizedProgress = (float)timesInteracted / interactAmountToDestroy;

            _circleProgressMaterial.DOFloat(normalizedProgress, s_progressAmount, _circleProgressMoveDurationInSeconds);
        }
        
        private void ToggleProgressCircle(bool isActive)
        {
            _progressCircle.enabled = isActive;
        }
        
        private void PlayPopupTextAnimation(int quantity)
        {
            PopupText popupText = ObjectPoolManager.SpawnObject(GameResources.Retrieve.PopupText, transform.position,
                transform.rotation, PoolType.GameObject);
            popupText.Initialize(quantity, () => ObjectPoolManager.ReturnObjectToPool(popupText));
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
        
        private void ResourcesGathererInitializeEvent_OnResourcesGathererInitialize(object sender, ResourcesGathererInitializeEventArgs e)
        {
            GetMaterialsFromVisual(e.VisualGameObject);
        }
        
        private void GatheredResourceEvent_OnGatheredResource(object sender, GatheredResourceEventArgs e)
        {
            PlayGatherAnimation();
            PlayPopupTextAnimation(e.GatheredQuantity);
            UpdateProgressCircle(e.TimesInteractedAmount, e.InteractAmountToDestroy);
        }
        
        private void GatheringStateChangedEvent_OnGatheringStateChanged(object sender, GatheringStateChangedEventArgs e)
        {
            ToggleProgressCircle(e.IsGathering);
        }
        
        private void FullyGatheredEvent_OnFullyGathered(object sender, EventArgs e)
        {
            PlayDestroyAnimation();
        }
    }
}