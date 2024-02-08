using System;
using Character.Player;
using Common;
using UnityEngine;
using Zenject;

namespace Farm.Plants
{
    [RequireComponent(typeof(BoxCollider))]
    [DisallowMultipleComponent]
    public class PlantArea : MonoBehaviour, IInteractable
    {
        [Header("External references")] 
        [SerializeField] private PlantFactory _plantFactory;
        
        [Header("Settings")]
        [SerializeField] private PlantType _plantType;

        private Plant _plant;
        private Player _player;
        private BoxCollider _boxCollider;

        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }
        
        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            EnableCollider();
        }

        public void Interact()
        {
            SpawnPlant();
            DisableCollider();
        }

        public void StopInteract()
        {
            // TODO: corresponding action
        }

        public void ClearPlantArea()
        {
            _plant = null;
            EnableCollider();
        }
        
        private void SpawnPlant()
        {
            Plant plant = _plantFactory.CreatePlant(_plantType);
            plant.Initialize(transform.position, Quaternion.identity, this);
            _plant = plant;
        }

        private void EnableCollider()
        {
            _boxCollider.enabled = true;
        }
        
        private void DisableCollider()
        {
            _boxCollider.enabled = false;
        }
    }
}
