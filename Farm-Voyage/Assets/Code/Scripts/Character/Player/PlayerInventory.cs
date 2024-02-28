using System;
using System.Collections.Generic;
using System.Linq;
using Farm.FarmResources;
using Farm.FarmResources.ConcreteFarmResources;
using Farm.Plants.Seeds;
using Farm.Plants.Seeds.ConcreteSeeds;
using Farm.Tool;
using UnityEngine;

namespace Character.Player
{
    public class PlayerInventory
    {
        public event Action<ResourceType, int> OnResourceQuantityChanged; 
        
        private readonly HashSet<Tool> _toolsHashSet;
        private readonly HashSet<FarmResource> _farmResourcesHashSet;
        private readonly HashSet<Seed> _seedsHashSet;
        
        public PlayerInventory(HashSet<Tool> toolsHashSet)
        {
            _toolsHashSet = toolsHashSet;
            _farmResourcesHashSet = new HashSet<FarmResource>
            {
                new Dirt(0, ResourceType.Dirt),
                new Rock(0, ResourceType.Rock),
                new Wood(0, ResourceType.Wood),
            };
            _seedsHashSet = new HashSet<Seed>
            {
                new CarrotSeed(SeedType.Carrot, 0),
                new PumpkinSeed(SeedType.Pumpkin, 0),
                new EggplantSeed(SeedType.Eggplant, 0),
                new TurnipSeed(SeedType.Turnip, 0),
                new CornSeed(SeedType.Corn, 0),
                new CarrotSeed(SeedType.Carrot, 0),
                new TomatoSeed(SeedType.Tomato, 10),
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

        public void AddTool(ToolType toolType)
        {
            switch (toolType)
            {
                case ToolType.Shovel:
                    _toolsHashSet.Add(new Shovel(1f, 5));
                    break;
                case ToolType.Axe:
                    _toolsHashSet.Add(new Axe(1f, 5));
                    break;
                case ToolType.Pickaxe:
                    _toolsHashSet.Add(new Pickaxe(1f, 5));
                    break;
                case ToolType.WaterCan:
                    _toolsHashSet.Add(new WaterCan(1f, 5));
                    break;
                case ToolType.Scythe:
                    _toolsHashSet.Add(new Scythe(1f, 5));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(toolType), toolType, null);
            }
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

        public void RemoveResourceQuantity(ResourceType resourceType, int quantity)
        {
            foreach (FarmResource farmResource in _farmResourcesHashSet)
            {
                if (farmResource.Type != resourceType) continue;
                
                if (quantity < 0)
                {
                    Debug.LogError("Quantity can't be less than a null");
                    quantity = 0;
                }

                if (GetResourceQuantity(farmResource.Type) < quantity)
                {
                    Debug.LogError("Remove quantity is bigger than resource quantity");
                    quantity = 0;
                }
                
                farmResource.RemoveQuantity(quantity);
                OnResourceQuantityChanged?.Invoke(resourceType, farmResource.Quantity);
                return;
            }
        }

        public int GetSeedsQuantity(SeedType seedType)
        {
            foreach (Seed seed in _seedsHashSet)
            {
                if (seed.SeedType != seedType) continue;

                return seed.Quantity;
            }

            return 0;
        }
        
        public void AddSeedQuantity(SeedType seedType, int quantity)
        {
            foreach (Seed seed in _seedsHashSet)
            {
                if (seed.SeedType != seedType) continue;
                
                if (quantity < 0)
                {
                    Debug.LogError("Quantity can't be less than a null");
                    quantity = 0;
                }
                
                seed.AddQuantity(quantity);
                return;
            }
        }

        public bool HasEnoughSeedQuantity(SeedType seedType, int quantity)
        {
            foreach (Seed seed in _seedsHashSet)
            {
                if (seed.SeedType != seedType) continue;

                return seed.Quantity >= quantity;
            }

            return false;
        }
    }
}