using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Character.Player;
using Misc;

namespace DataPersistence
{
    [DisallowMultipleComponent]
    public class SaveManager : Singleton<SaveManager>
    {
        private Player _player;

        protected override void Awake()
        {
            base.Awake();
            _player = FindObjectOfType<Player>();
        }

        public void Save()
        {
            // Create a file or open a file to save to
            FileStream fileStream = new(Application.persistentDataPath + "/Game.dat", FileMode.OpenOrCreate);

            try
            {
                // Binary Formatter -- allows us to write data to a file
                BinaryFormatter binaryFormatter = new();

                // Serialization method to WRITE to the file
                binaryFormatter.Serialize(fileStream, _player.Stats);
            }
            catch (SerializationException e)
            {
                Debug.Log($"There was an issue serializing this data: {e.Message}");
            }
            finally
            {
                fileStream.Close();
            }
        }

        public void Load()
        {
            FileStream fileStream = new(Application.persistentDataPath + "/Game.dat", FileMode.Open);

            try
            {
                BinaryFormatter binaryFormatter = new();
                _player.Stats = (Stats)binaryFormatter.Deserialize(fileStream);
            }
            catch (SerializationException e)
            {
                Debug.Log($"Error Deserializing Data: {e.Message}");
            }
            finally
            {
                fileStream.Close();
            }
        }
    }
}