using Dummy;

namespace DataPersistence
{
    [System.Serializable]
    public class GameData
    {
        public string Name;
        public int CurrentDay;
        public DummyClassData DummyClassData;
    }
}