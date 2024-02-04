using Character.Player;
using Common;
using UnityEngine;
using Zenject;

namespace Farm
{
    [DisallowMultipleComponent]
    public class ResourcesGatherer : MonoBehaviour, IInteractable
    {
        [Header("Settings")]
        [SerializeField] private ToolType _requiredTool;
        [SerializeField] private ResourceType _resourceToGather;
        
        private Player _player;
        private Tool _playerTool;
        
        private bool _canGather;
        
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
            if (TryGatherResources(out GatheredResource gatheredResource))
            {
                // TODO: Finish it
            }
        }

        private bool TryGatherResources(out GatheredResource gatheredResource)
        {
            gatheredResource = new GatheredResource();
    
            if (!_canGather)
            {
                Debug.Log("You do not have the required tool to gather this resource.");
                return false;
            }
    
            int quantityGathered = CalculateQuantityBasedOnTool(_playerTool);
    
            if (quantityGathered > 0)
            {
                gatheredResource = new GatheredResource(_resourceToGather, quantityGathered);
                Debug.Log($"Successfully gathered {gatheredResource.Quantity} of {_resourceToGather}.");
        
                return true;
            }

            Debug.Log("Failed to gather the resource.");
            return false;
        }

        private int CalculateQuantityBasedOnTool(Tool tool)
        {
            int minimumRandomValue = 1;
            int maximumRandomValue = 5;
            
            return tool.Level * Random.Range(minimumRandomValue, maximumRandomValue);
        }
    }
}
