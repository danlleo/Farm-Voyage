using System.Collections.Generic;
using System.Linq;
using Attributes.WithinParent;
using InputManagers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace UI
{
    [DisallowMultipleComponent]
    public class SeedsChooseItemContainer : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField, WithinParent] private SeedChooseUIItem _seedChooseUIItemTomato;
        [SerializeField, WithinParent] private SeedChooseUIItem _seedChooseUIItemCarrot;
        [SerializeField, WithinParent] private SeedChooseUIItem _seedChooseUIItemEggplant;
        [SerializeField, WithinParent] private SeedChooseUIItem _seedChooseUIItemPumpkin;
        [SerializeField, WithinParent] private SeedChooseUIItem _seedChooseUIItemTurnip;
        [SerializeField, WithinParent] private SeedChooseUIItem _seedChooseUIItemCorn;

        private Dictionary<SeedChooseUIItem, bool> _seedChooseUIItemsMapping;

        private void Awake()
        {
            InitializeDictionary();
        }
        
        private void OnEnable()
        {
            SeedChooseUIItem.OnAnySelectedSeedChooseItemEvent += SeedChooseUIItem_OnAnySelectedSeedChooseItem;
            PlayerPCInput.OnAnySeedSelected += PlayerPCInput_OnAnySeedSelected;
        }

        private void OnDisable()
        {
            SeedChooseUIItem.OnAnySelectedSeedChooseItemEvent -= SeedChooseUIItem_OnAnySelectedSeedChooseItem;
            PlayerPCInput.OnAnySeedSelected -= PlayerPCInput_OnAnySeedSelected;
        }
        
        private void InitializeDictionary()
        {
            _seedChooseUIItemsMapping = new Dictionary<SeedChooseUIItem, bool>
            {
                { _seedChooseUIItemTomato, false },
                { _seedChooseUIItemCarrot, false },
                { _seedChooseUIItemEggplant, false },
                { _seedChooseUIItemPumpkin, false },
                { _seedChooseUIItemTurnip, false },
                { _seedChooseUIItemCorn, false }
            };
        }

        private void DeselectAllWithException(SeedChooseUIItem seedChooseUIItem)
        {
            foreach (KeyValuePair<SeedChooseUIItem, bool> mapping in _seedChooseUIItemsMapping.ToList())
            {
                if (seedChooseUIItem == mapping.Key) continue;
                
                _seedChooseUIItemsMapping[mapping.Key] = false;
                mapping.Key.Deselect();
            }
        }

        private void HandleSelectionSingle(SeedChooseUIItem seedChooseUIItem)
        {
            if (!_seedChooseUIItemsMapping.TryGetValue(seedChooseUIItem, out bool isSelected)) return;
            
            DeselectAllWithException(seedChooseUIItem);
            
            _seedChooseUIItemsMapping[seedChooseUIItem] = !isSelected;
            
            if (!isSelected)
            {
                seedChooseUIItem.Select();
                return;
            }

            seedChooseUIItem.Deselect();
        }
        
        private void SelectSeedBySlot(InputControl obj)
        {
            if (obj is not KeyControl key) return;

            switch (key.keyCode)
            {
                case Key.Digit1:
                    HandleSelectionSingle(_seedChooseUIItemTomato);
                    break;
                case Key.Digit2:
                    HandleSelectionSingle(_seedChooseUIItemCarrot);
                    break;
                case Key.Digit3:
                    HandleSelectionSingle(_seedChooseUIItemEggplant);
                    break;
                case Key.Digit4:
                    HandleSelectionSingle(_seedChooseUIItemPumpkin);
                    break;
                case Key.Digit5:
                    HandleSelectionSingle(_seedChooseUIItemTurnip);
                    break;
                case Key.Digit6:
                    HandleSelectionSingle(_seedChooseUIItemCorn);
                    break;
                default:
                    Debug.LogWarning($"Unhandled key: {key.name}");
                    break;
            }
        }
        
        private void SeedChooseUIItem_OnAnySelectedSeedChooseItem(SeedChooseUIItem seedChooseUIItem)
        {
            HandleSelectionSingle(seedChooseUIItem);
        }
        
        private void PlayerPCInput_OnAnySeedSelected(InputControl obj)
        {
            SelectSeedBySlot(obj);
        }
    }
}
