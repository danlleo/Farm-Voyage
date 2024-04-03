using Farm.Plants;

namespace Timespan.Quota
{
    public struct MeetQuotaData
    {
        public readonly PlantType PlantType;
        public readonly int Quantity;

        public MeetQuotaData(PlantType plantType, int quantity)
        {
            PlantType = plantType;
            Quantity = quantity;
        }
    }
}