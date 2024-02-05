using System.Collections;
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
        [Header("Settings")]
        [SerializeField] private ToolType _requiredTool;
        [SerializeField] private ResourceType _resourceToGather;
        [SerializeField, Range(1, 5)] private int _interactAmountToDestroy = 1;
        
        private Player _player;
        private Tool _playerTool;
        
        private bool _canGather;

        private Coroutine _delayGatheringResourcesRoutine;

        private int _timesInteracted;
        
        [Inject]
        private void Construct(Player player)
        {
            _player = player;

            foreach (Tool tool in _player.ToolsList)
            {
                if (tool.Type != _requiredTool) continue;
                
                _playerTool = tool;
                break;
            }

            _canGather = _playerTool != null;
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
            _player.PlayerGatheringEvent.Call(this, new PlayerGatheringEventArgs(false));
        }

        private IEnumerator DelayGatheringResourcesRoutine(GatheredResource gatheredResource)
        {
            float delayTime = CalculateTimeToGatherBasedOnToolLevel(_playerTool);
            float gatherTime = CalculateTimeToGatherBasedOnToolLevel(_playerTool);
            
            _player.PlayerGatheringEvent.Call(this,
                new PlayerGatheringEventArgs(true, gatherTime));
            
            yield return new WaitForSeconds(delayTime);
            
            OnResourceGathered(gatheredResource);
        }
        
        private bool TryGatherResources(out GatheredResource gatheredResource)
        {
            gatheredResource = new GatheredResource();

            if (!_canGather) return false;
    
            int quantityGathered = CalculateQuantityBasedOnToolLevel(_playerTool);

            if (quantityGathered <= 0) return false;
            
            gatheredResource = new GatheredResource(_resourceToGather, quantityGathered);
            
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
            if (_timesInteracted != _interactAmountToDestroy) return;
            
            Destroy(gameObject);
        }
        
        private void OnResourceGathered(GatheredResource gatheredResource)
        {
            print("Gathered");
            
            StopGathering();
            IncreaseTimeInteracted();
            DestroyIfFullyGathered();
        }
    }
}
