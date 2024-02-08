using System.Collections;
using Attributes.ChildrenOnly;
using Character.Player;
using Common;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Farm
{
    [DisallowMultipleComponent]
    public class ResourcesGatherer : MonoBehaviour, IInteractable
    {
        [Header("External reference")]
        [SerializeField, ChildrenOnly] private Transform _visualSpawnPoint;
        
        private ResourceSO _resourceSO;
        
        private Player _player;
        private Tool _playerTool;
        
        private bool _canGather;

        private Coroutine _delayGatheringResourcesRoutine;

        private int _timesInteracted;
        
        [Inject]
        private void Construct(Player player)
        {
            _player = player;
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
            
            OnResourceGathered(gatheredResource);
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

        private int CalculateQuantityBasedOnToolLevel(Tool tool)
        {
            const int minimumRandomValue = 1;
            const int maximumRandomValue = 5;

            int calculatedQuantity = tool.Level * Random.Range(minimumRandomValue, maximumRandomValue);
            
            return calculatedQuantity;
        }

        private float CalculateTimeToGatherBasedOnToolLevel(Tool tool)
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
            if (_player.TryGetTool(_resourceSO.RequiredTool, out Tool tool))
            {
                _playerTool = tool;
                _canGather = true;
                return;
            }

            _canGather = false;
        }
        
        private void OnResourceGathered(GatheredResource gatheredResource)
        {
            print("Gathered");
            
            StopGathering();
            IncreaseTimeInteracted();
            DestroyIfFullyGathered();
        }
        
        public class Factory : PlaceholderFactory<ResourcesGatherer> { }
    }
}
