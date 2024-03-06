using System.Collections.Generic;
using System.Linq;
using DataPersistence.ConcreteSerializers;
using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DataPersistence
{
    public class SaveLoadSystem : PersistentSingleton<SaveLoadSystem>
    {
        [SerializeField] public GameData Data;

        private IDataService _dataService;

        protected override void Awake()
        {
            base.Awake();
            _dataService = new FileDataService(new JsonSerializer());
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneManager_OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneManager_OnSceneLoaded;
        }

        public void NewGame()
        {
            Data = new GameData
            {
                Name = "New Game",
                CurrentDay = 1
            };
        }

        public void SaveGame()
        {
            _dataService.Save(Data);
        }

        public void LoadGame(string gameName)
        {
            Data = _dataService.Load(gameName);
        }

        public void DeleteGame(string gameName)
        {
            _dataService.Delete(gameName);
        }

        private void Bind<T, TData>(TData data) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            T entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();

            if (entity == null) return;

            if (data == null)
            {
                data = new TData { ID = entity.ID };
            }
            
            entity.Bind(data);
        }

        private void Bind<T, TData>(List<TData> datas) where T : MonoBehaviour, IBind<TData>
            where TData : ISaveable, new()
        {
            T[] entities = FindObjectsByType<T>(FindObjectsSortMode.None);

            foreach (T entity in entities)
            {
                TData data = datas.FirstOrDefault(d => d.ID == entity.ID);
                
                if (data == null)
                {
                    data = new TData { ID = entity.ID };
                    datas.Add(data);
                }
                
                entity.Bind(data);
            }
        }
        
        private void SceneManager_OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Bind<DummyClass, DummyClassData>(Data.DummyClassData);
        }
    }
}