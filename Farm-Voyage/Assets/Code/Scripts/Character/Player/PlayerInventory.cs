using System;
using System.Collections.Generic;
using System.Linq;
using Farm.FarmResources;
using Farm.FarmResources.ConcreteFarmResources;
using Farm.Tool;
using UnityEngine;

namespace Character.Player
{
    public class PlayerInventory
    {
        public event Action<ResourceType, int> OnResourceQuantityChanged; 
        
        private readonly HashSet<Tool> _toolsHashSet;
        private readonly HashSet<FarmResource> _farmResourcesHashSet;
        
        public PlayerInventory(HashSet<Tool> toolsHashSet)
        {
            _toolsHashSet = toolsHashSet;
            _farmResourcesHashSet = new HashSet<FarmResource>
            {
                new Dirt(0, ResourceType.Dirt),
                new Rock(0, ResourceType.Rock),
                new Wood(0, ResourceType.Wood),
            };
        }
        
        public bool TryGetTool<T>(out T tool) where T : Tool
        {
            tool = _toolsHashSet.OfType<T>().FirstOrDefault();
            return tool != null;
        }
        
        public bool TryGetToolOfType(Type toolType, out Tool foundTool)
        {
            foreach (Tool tool in _toolsHashSet.Where(tool => tool.GetType() == toolType))
            {
                foundTool = tool;
                return true;
            }

            foundTool = null;
            return false;
        }

        public int GetResourceQuantity(ResourceType resourceType)
        {
            foreach (FarmResource farmResource in _farmResourcesHashSet)
            {
                if (farmResource.Type != resourceType) continue;
                
                return farmResource.Quantity;
            }

            return 0;
        }
        
        public void AddResourceQuantity(ResourceType resourceType, int quantity)
        {
            foreach (FarmResource farmResource in _farmResourcesHashSet)
            {
                if (farmResource.Type != resourceType) continue;
                
                if (quantity < 0)
                {
                    Debug.LogError("Quantity can't be less than a null");
                    quantity = 0;
                }
                
                farmResource.AddQuantity(quantity);
                OnResourceQuantityChanged?.Invoke(resourceType, farmResource.Quantity);
                return;
            }
        }
    }
}