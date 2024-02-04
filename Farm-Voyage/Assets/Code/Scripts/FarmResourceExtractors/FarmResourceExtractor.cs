using Common;
using FarmTools;
using UnityEngine;

namespace FarmResourceExtractors
{
    [DisallowMultipleComponent]
    public abstract class FarmResourceExtractor : MonoBehaviour, IInteractable
    {
        protected abstract FarmTool RequiredFarmTool { get; }
        protected FarmResource ExtractResource { get; }
        
        public void Interact()
        {
            TryExtractWith(RequiredFarmTool);
        }
        
        private ExtractedFarmResource TryExtractWith(FarmTool requiredTool)
        {
            // TODO: logic here
            return ExtractResource.Extract(RequiredFarmTool);
        }
    }
}
