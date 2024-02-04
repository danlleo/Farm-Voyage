using UnityEngine;

namespace FarmTools
{
    [DisallowMultipleComponent]
    public abstract class FarmTool : MonoBehaviour
    {
        private const int DefaultToolLevel = 1;
        
        public abstract int ToolLevel { get; }
    }
}
