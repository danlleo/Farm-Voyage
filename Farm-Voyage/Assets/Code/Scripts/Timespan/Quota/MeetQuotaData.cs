using Farm.Plants.Seeds;

namespace Timespan.Quota
{
    public struct MeetQuotaData
    {
        public readonly SeedType Seed;
        public readonly int Quantity;

        public MeetQuotaData(SeedType seed, int quantity)
        {
            Seed = seed;
            Quantity = quantity;
        }
    }
}