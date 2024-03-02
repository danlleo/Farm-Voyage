using System;
using Attributes.WithinParent;
using Character.Player;
using Farm.Plants.Seeds;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [DisallowMultipleComponent]
    public class SeedChooseItem : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<SeedChooseItem> OnAnySelectedSeedChooseItemEvent;
        
        [field:SerializeField] public SeedType SeedType { get; private set; }

        [Header("External references")]
        [SerializeField, WithinParent] private TextMeshProUGUI _seedQuantityText;
        [SerializeField, WithinParent] private Image _backgroundImage;
        [SerializeField] private Sprite _defaultBackgroundSprite;
        [SerializeField] private Sprite _selectedBackgroundSprite;
        
        private PlayerInventory _playerInventory;

        private bool _isSelected;

        private void OnEnable()
        {
            OnAnySelectedSeedChooseItemEvent += SeedChooseItem_OnAnySelectedSeedChooseItem;
        }

        private void OnDisable()
        {
            OnAnySelectedSeedChooseItemEvent -= SeedChooseItem_OnAnySelectedSeedChooseItem;
        }

        public void Initialize(PlayerInventory playerInventory, int seedAmount)
        {
            _playerInventory = playerInventory;
            _seedQuantityText.text = $"{seedAmount}";
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ToggleSelect();
        }

        private void ToggleSelect()
        {
            _isSelected = !_isSelected;

            if (_isSelected)
            {
                OnAnySelectedSeedChooseItemEvent?.Invoke(this);
                _backgroundImage.sprite = _selectedBackgroundSprite;
                _playerInventory.SetSelectedSeed(SeedType);
                return;
            }
            
            _backgroundImage.sprite = _defaultBackgroundSprite;
            _playerInventory.SetSelectedSeed(SeedType.Default);
        }

        private void Deselect()
        {
            _isSelected = false;
            _backgroundImage.sprite = _defaultBackgroundSprite;
            _playerInventory.SetSelectedSeed(SeedType.Default);
        }
        
        private void SeedChooseItem_OnAnySelectedSeedChooseItem(SeedChooseItem seedChooseItem)
        {
            if (ReferenceEquals(this, seedChooseItem)) return;
            
            Deselect();
        }
    }
}
