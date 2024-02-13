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
        private readonly HashSet<Tool> _toolsList = new()
        {
            new Axe(3f, 1),
            new Pickaxe(3f, 5),
            new Shovel(3f, 1),
            new Scythe(3f, 1),
            new WaterCan(3f, 1),
        };
        
        public bool TryGetTool<T>(out T tool) where T : Tool
        {
            // Check if T is exactly the Tool class and not a derived class
            if (typeof(T) == typeof(Tool))
            {
                throw new ArgumentException("T must be a derived class of Tool, not Tool itself.");
            }
            
            foreach (T playerTool in _toolsList.OfType<T>())
            {
                tool = playerTool;
                return true;
            }

            tool = null;
            return false;
        }
    }
}