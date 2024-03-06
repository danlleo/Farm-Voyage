using System.Collections.Generic;

namespace DataPersistence
{
    public interface IDataService
    {
        public void Save(GameData data, bool overwrite = true);
        public GameData Load(string name);
        public void Delete(string name);
        public void DeleteAll();
        public IEnumerable<string> ListSaves();
    }
}