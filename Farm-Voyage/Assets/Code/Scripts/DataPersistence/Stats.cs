using System.Collections.Generic;
using Farm.FarmResources.ConcreteFarmResources;
using Farm.Tool;

namespace DataPersistence
{
    
    [System.Serializable]
    public class Stats
    {
        public int CurrentWeek;
        public int CurrentDay;
        public List<Tool> SavedTools;
        public List<FarmResource> SavedFarmResources;
    }
}