using System;
using System.Collections.Generic;
using System.Linq;
using Farm.Tool;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerInventory : MonoBehaviour
    {
        private readonly HashSet<Tool> _toolsHashSet = new()
        {
            new Axe(3f, 1),
            new Pickaxe(3f, 5),
            new Shovel(3f, 1),
            new Scythe(3f, 1),
            new WaterCan(3f, 1),
        };
        
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
    }
}