using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DataPersistence
{
    public class FileDataService : IDataService
    {
        private ISerializer _serializer;
        private string _dataPath;
        private string _fileExtension;

        public FileDataService(ISerializer serializer)
        {
            _serializer = serializer;
            _dataPath = Application.persistentDataPath;
            _fileExtension = "json";
        }

        public void Save(GameData data, bool overwrite = true)
        {
            string fileLocation = GetPathToFile(data.Name);

            if (!overwrite && File.Exists(fileLocation))
            {
                throw new IOException(
                    $"The file `{data.Name}.{_fileExtension}` already exists and cannot be overwritten");
            }

            File.WriteAllText(fileLocation, _serializer.Serialize(data));
        }

        public GameData Load(string name)
        {
            string fileLocation = GetPathToFile(name);

            if (!File.Exists(fileLocation))
            {
                throw new IOException($"No persisted GameData with name `{name}`");
            }

            string data = File.ReadAllText(fileLocation);

            return _serializer.Deserialize<GameData>(data);
        }

        public void Delete(string name)
        {
            string fileLocation = GetPathToFile(name);
            
            if (!File.Exists(fileLocation))
            {
                throw new IOException($"No persisted GameData with name `{name}`");
            }
            
            File.Delete(fileLocation);
        }

        public void DeleteAll()
        {
            foreach (string filePath in Directory.GetFiles(_dataPath))
            {
                File.Delete(filePath);
            }
        }

        public IEnumerable<string> ListSaves()
        {
            foreach (string path in Directory.GetFiles(_dataPath))
            {
                if (Path.GetExtension(path) == _fileExtension)
                {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }

        private string GetPathToFile(string fileName)
        {
            return Path.Combine(_dataPath, string.Concat(fileName, ".", _fileExtension));
        }
    }
}