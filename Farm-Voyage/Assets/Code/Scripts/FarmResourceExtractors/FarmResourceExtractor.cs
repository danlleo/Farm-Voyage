using FarmTools;
using UnityEngine;

namespace FarmResourceExtractors
{
    [DisallowMultipleComponent]
    public abstract class FarmResourceExtractor : MonoBehaviour
    {
        public abstract FarmTool RequiredFarmTool();
        protected FarmResource ExtractResource;
        
        public virtual ExtractedFarmResource TryExtractWith(FarmTool requiredTool)
        {
            // TODO: logic here
            return ExtractResource.Extract();
        }
    }
}
