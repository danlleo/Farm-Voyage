using System;
using System.Collections.Generic;
using System.Linq;
using Farm.Tool;

namespace Character.Player
{
    public class PlayerInventory
    {
        private readonly HashSet<Tool> _toolsHashSet;
        
        public PlayerInventory(HashSet<Tool> toolsHashSet)
        {
            _toolsHashSet = toolsHashSet;
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
    }
}