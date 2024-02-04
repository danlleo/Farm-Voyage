using UnityEngine;

namespace FarmResourceExtractors
{
    public abstract class FarmResource
    {
        public ExtractedFarmResource Extract()
        {
            int randomExtractedQuantity = Random.Range(1, 5);
            
            return new ExtractedFarmResource(this, randomExtractedQuantity);
        }
    }
}