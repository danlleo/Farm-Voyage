using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Misc;
using Misc.ObjectPool;
using UI;
using UnityEngine;

namespace Farm.ResourceGatherer
{
    [RequireComponent(typeof(GatheredResourceEvent))]
    [RequireComponent(typeof(FullyGatheredEvent))]
    [DisallowMultipleComponent]
    public class ResourceGathererAnimator : MonoBehaviour
    {
        private GatheredResourceEvent _gatheredResourceEvent;
        private FullyGatheredEvent _fullyGatheredEvent;
        
        private Coroutine _gatherAnimationRoutine;

        private void Awake()
        {
            _gatheredResourceEvent = GetComponent<GatheredResourceEvent>();
            _fullyGatheredEvent = GetComponent<FullyGatheredEvent>();
        }

        private void OnEnable()
        {
            _gatheredResourceEvent.OnGatheredResource += GatheredResourceEvent_OnGatheredResource;
            _fullyGatheredEvent.OnFullyGathered += FullyGatheredEvent_OnFullyGathered;
        }

        private void OnDisable()
        {
            _gatheredResourceEvent.OnGatheredResource -= GatheredResourceEvent_OnGatheredResource;
            _fullyGatheredEvent.OnFullyGathered -= FullyGatheredEvent_OnFullyGathered;
        }
        
        private void PlayDestroyAnimation()
        {
            Sequence destroyAnimationSequence = DOTween.Sequence();
            destroyAnimationSequence.Append(transform.DOScale(transform.localScale * 1.25f, .35f));
            destroyAnimationSequence.Append(transform.DOScale(Vector3.zero, .35f));
            destroyAnimationSequence.OnComplete(() => Destroy(gameObject));
        }

        private void PlayGatherAnimation(float duration, List<Material> allMaterials)
        {
            if (_gatherAnimationRoutine != null)
                StopCoroutine(_gatherAnimationRoutine);

            _gatherAnimationRoutine = StartCoroutine(GatherAnimationRoutine(duration, allMaterials));
        }
        
        private IEnumerator GatherAnimationRoutine(float duration, List<Material> allMaterials)
        {
            transform.DOShakeScale(duration, 0.5f);

            int flashIntensity = Shader.PropertyToID("_FlashIntensity");
            
            float timer = 0f;
            float durationHalfWay = duration / 2;

            while (timer <= duration)
            {
                foreach (Material material in allMaterials)
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
        
        private void PlayPopupTextAnimation(int quantity)
        {
            PopupText popupText = ObjectPoolManager.SpawnObject(GameResources.Retrieve.PopupText, transform.position,
                transform.rotation, PoolType.GameObject);
            popupText.Initialize(quantity, () => ObjectPoolManager.ReturnObjectToPool(popupText));
        }
        
        private void GatheredResourceEvent_OnGatheredResource(object sender, GatheredResourceEventArgs e)
        {
            PlayGatherAnimation(0.25f, e.AllMaterials);
            PlayPopupTextAnimation(e.GatheredQuantity);
        }
        
        private void FullyGatheredEvent_OnFullyGathered(object sender, EventArgs e)
        {
            PlayDestroyAnimation();
        }
    }
}