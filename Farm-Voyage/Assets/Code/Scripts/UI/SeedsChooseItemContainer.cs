using System.Collections.Generic;
using System.Linq;
using Attributes.WithinParent;
using UnityEngine;

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
        }

        private void OnDisable()
        {
            SeedChooseUIItem.OnAnySelectedSeedChooseItemEvent -= SeedChooseUIItem_OnAnySelectedSeedChooseItem;
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
            
            _seedChooseUIItemsMapping[seedChooseUIItem] = !isSelected;
            
            if (!isSelected)
            {
                seedChooseUIItem.Select();
                return;
            }

            seedChooseUIItem.Deselect();
        }
        
        private void SeedChooseUIItem_OnAnySelectedSeedChooseItem(SeedChooseUIItem seedChooseUIItem)
        {
            DeselectAllWithException(seedChooseUIItem);
            HandleSelectionSingle(seedChooseUIItem);
        }
    }
}
