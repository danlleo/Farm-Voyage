namespace FarmResourceExtractors
{
    public struct ExtractedFarmResource
    {
        public readonly FarmResource ExtractedResource;
        public readonly int ExtractedQuantity;

        public ExtractedFarmResource(FarmResource extractedResource, int extractedQuantity)
        {
            ExtractedResource = extractedResource;
            ExtractedQuantity = extractedQuantity;
        }
    }
}