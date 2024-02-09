using System;
using Attributes.Self;
using Character.Player;
using Farm.Plants;
using UnityEngine;
using Zenject;

namespace Farm.Corral
{
    [RequireComponent(typeof(PlantAreaClearedEvent))]
    [DisallowMultipleComponent]
    public class Corral : MonoBehaviour
    {
        public PlantAreaClearedEvent PlantAreaClearedEvent { get; private set; }
        
        [Header("External references")]
        [SerializeField, Self] private PlantArea[] _plantAreaArray;
        [SerializeField, Self] private StorageBox _storageBox;

        private Player _player;
        private PlantFactory _plantFactory;
        
        [Inject]
        private void Construct(Player player, PlantFactory plantFactory)
        {
            _player = player;
            _plantFactory = plantFactory;
        }

        private void Awake()
        {
            PlantAreaClearedEvent = GetComponent<PlantAreaClearedEvent>();
        }

        private void Start()
        {
            InitializePlantAreaArrayItems();
            InitializeStorageBox();
        }

        private void InitializePlantAreaArrayItems()
        {
            foreach (PlantArea plantArea in _plantAreaArray)
            {
                plantArea.Initialize(this, _player, _plantFactory);
            }
        }

        private void InitializeStorageBox()
        {
            _storageBox.Initialize(this);
        }
    }
}
