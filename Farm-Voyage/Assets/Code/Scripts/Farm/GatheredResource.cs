namespace Farm
{
    public struct GatheredResource
    {
        public readonly ResourceType Type;
        public readonly int Quantity;

        public GatheredResource(ResourceType type, int quantity)
        {
            Type = type;
            Quantity = quantity;
        }
    }
}