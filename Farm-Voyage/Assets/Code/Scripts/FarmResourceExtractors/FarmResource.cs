using FarmTools;
using UnityEngine;

namespace FarmResourceExtractors
{
    public abstract class FarmResource
    {
        public ExtractedFarmResource Extract(FarmTool farmTool)
        {
            int randomExtractedQuantity = Random.Range(1, 5);
            
            return new ExtractedFarmResource(this, randomExtractedQuantity);
        }
    }
}