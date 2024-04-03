using System;
using System.Collections.Generic;
using System.Linq;
using Farm.FarmResources;
using Farm.FarmResources.ConcreteFarmResources;
using Farm.Plants;
using Farm.Plants.Seeds;
using Farm.Plants.Seeds.ConcreteSeeds;
using Farm.Tool;
using Farm.Tool.ConcreteTools;
using UnityEngine;

namespace Character.Player
{
    public sealed class PlayerInventory
    {
        public event Action<ResourceType, int> OnResourceQuantityChanged;
        public event Action<SeedType, int> OnSeedQuantityChanged;
        
        private readonly List<Tool> _toolsList;
        private readonly List<FarmResource> _farmResourcesList;
        private readonly List<Seed> _seedsList;

        private readonly Dictionary<PlantType, int> _plantQuantitiesMapping = new();
        
        private Seed _selectedSeed;
        
        public PlayerInventory(List<Tool> toolsList)
        {
            _toolsList = toolsList;
            _farmResourcesList = new List<FarmResource>
            {
                new Dirt(10, ResourceType.Dirt),
                new Rock(10, ResourceType.Rock),
                new Wood(10, ResourceType.Wood),
            };
            _seedsList = new List<Seed>
            {
                new CarrotSeed(SeedType.Carrot,  PlantType.Carrot, 10),
                new PumpkinSeed(SeedType.Pumpkin,  PlantType.Pumpkin, 10),
                new EggplantSeed(SeedType.Eggplant, PlantType.Eggplant, 10),
                new TurnipSeed(SeedType.Turnip, PlantType.Turnip, 10),
                new CornSeed(SeedType.Corn, PlantType.Corn, 10),
                new TomatoSeed(SeedType.Tomato, PlantType.Tomato, 10),
            };
        }
        
        public bool TryGetTool<T>(out T tool) where T : Tool
        {
            tool = _toolsList.OfType<T>().FirstOrDefault();
            return tool != null;
        }

        public IEnumerable<Tool> GetAllTools()
        {
            return _toolsList;
        }
        
        public bool TryGetToolOfType(Type toolType, out Tool foundTool)
        {
            foreach (Tool tool in _toolsList.Where(tool => tool.GetType() == toolType))
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
                    _toolsList.Add(new Shovel(1f, 1));
                    break;
                case ToolType.Axe:
                    _toolsList.Add(new Axe(1f, 1));
                    break;
                case ToolType.Pickaxe:
                    _toolsList.Add(new Pickaxe(1f, 1));
                    break;
                case ToolType.WaterCan:
                    _toolsList.Add(new WaterCan(1f, 1));
                    break;
                case ToolType.Scythe:
                    _toolsList.Add(new Scythe(1f, 1));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(toolType), toolType, null);
            }
        }
        
        public int GetResourceQuantity(ResourceType resourceType)
        {
            return (from farmResource in _farmResourcesList
                where farmResource.Type == resourceType
                select farmResource.Quantity).FirstOrDefault();
        }
        
        public void AddResourceQuantity(ResourceType resourceType, int quantity)
        {
            foreach (FarmResource farmResource in _farmResourcesList.Where(farmResource => farmResource.Type == resourceType))
            {
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
            foreach (FarmResource farmResource in _farmResourcesList.Where(farmResource => farmResource.Type == resourceType))
            {
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

        public void SetSelectedSeed(SeedType seedType)
        {
            foreach (Seed seed in _seedsList)
            {
                if (seedType == SeedType.Default)
                {
                    _selectedSeed = null;
                    return;
                }
                
                if (seed.SeedType != seedType) continue;
                
                _selectedSeed = seed;
                return;
            }
        }

        public bool TryGetSelectedSeed(out Seed selectedSeed)
        {
            if (_selectedSeed == null)
            {
                selectedSeed = null;
                return false;
            }

            selectedSeed = _selectedSeed;
            return true;
        }
        
        public int GetSeedsQuantity(SeedType seedType)
        {
            return (from seed in _seedsList where seed.SeedType == seedType select seed.Quantity).FirstOrDefault();
        }
        
        public void AddSeedQuantity(SeedType seedType, int quantity)
        {
            foreach (Seed seed in _seedsList.Where(seed => seed.SeedType == seedType))
            {
                if (quantity < 0)
                {
                    Debug.LogError("Quantity can't be less than a null");
                    quantity = 0;
                }
                
                seed.AddQuantity(quantity);
                return;
            }
        }
        
        public void RemoveSeedQuantity(SeedType seedType, int quantity)
        {
            foreach (Seed seed in _seedsList)
            {
                if (seed.SeedType != seedType) continue;
                
                if (quantity < 0)
                {
                    Debug.LogError("Quantity can't be less than a null");
                    quantity = 0;
                }

                if (GetSeedsQuantity(seed.SeedType) < quantity)
                {
                    Debug.LogError("Remove quantity is bigger than resource quantity");
                    quantity = 0;
                }
                
                seed.RemoveQuantity(quantity);
                OnSeedQuantityChanged?.Invoke(seedType, seed.Quantity);
                return;
            }
        }

        public bool HasEnoughSeedQuantity(SeedType seedType, int quantity)
        {
            foreach (Seed seed in _seedsList.Where(seed => seed.SeedType == seedType))
            {
                if (seed.SeedType == SeedType.Default) 
                    return false;
                
                return seed.Quantity >= quantity;
            }

            return false;
        }

        public int GetPlantQuantity(PlantType plantType)
        {
            return _plantQuantitiesMapping.GetValueOrDefault(plantType, 0);
        }
        
        public void AddPlant(PlantType plantType, int addQuantity)
        {
            if (addQuantity < 0)
            {
                addQuantity = 0;
                Debug.LogWarning("Add quantity is less than zero!");
            }
            
            if (_plantQuantitiesMapping.TryGetValue(plantType, out int storedQuantity))
            {
                _plantQuantitiesMapping[plantType] = storedQuantity + addQuantity;
                return;
            }

            _plantQuantitiesMapping.Add(plantType, addQuantity);
        }
        
        public void RemovePlantQuantity(PlantType plantType, int removeQuantity)
        {
            if (removeQuantity < 0)
            {
                Debug.LogWarning("Remove quantity is less than zero!");
                return;
            }

            if (_plantQuantitiesMapping.TryGetValue(plantType, out int storedQuantity))
            {
                int newQuantity = storedQuantity - removeQuantity;

                switch (newQuantity)
                {
                    case > 0:
                        _plantQuantitiesMapping[plantType] = newQuantity;
                        break;
                    case 0:
                        _plantQuantitiesMapping.Remove(plantType);
                        break;
                    case < 0:
                        Debug.LogWarning(
                            "You're trying to remove quantity that is way more than actual stored quantity");
                        break;
                }
            }
            else
            {
                Debug.LogWarning("Plant type not found in dictionary!");
            }
        }

        public bool HasEnoughPlantQuantity(PlantType plantType, int quantity)
        {
            if (!_plantQuantitiesMapping.TryGetValue(plantType, out int storedQuantity)) return false;
            return storedQuantity >= quantity;
        }
    }
}