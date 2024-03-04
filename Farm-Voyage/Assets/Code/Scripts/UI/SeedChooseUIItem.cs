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
    public class SeedChooseUIItem : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<SeedChooseUIItem> OnAnySelectedSeedChooseItemEvent;
        
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

            UpdateSeedQuantityText(_playerInventory.GetSeedsQuantity(SeedType));
            
            if (_playerInventory != null)
            {
                _playerInventory.OnSeedQuantityChanged += PlayerInventory_OnSeedQuantityChanged;
            }
        }

        private void OnDisable()
        {
            OnAnySelectedSeedChooseItemEvent -= SeedChooseItem_OnAnySelectedSeedChooseItem;
            
            if (_playerInventory != null)
            {
                _playerInventory.OnSeedQuantityChanged -= PlayerInventory_OnSeedQuantityChanged;
            }
        }

        public void Initialize(PlayerInventory playerInventory)
        {
            _playerInventory = playerInventory;
            _playerInventory.OnSeedQuantityChanged += PlayerInventory_OnSeedQuantityChanged;
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

        private void UpdateSeedQuantityText(int quantity)
        {
            _seedQuantityText.text = $"{quantity}";
        }
        
        private void SeedChooseItem_OnAnySelectedSeedChooseItem(SeedChooseUIItem seedChooseUIItem)
        {
            if (ReferenceEquals(this, seedChooseUIItem)) return;
            
            Deselect();
        }
        
        private void PlayerInventory_OnSeedQuantityChanged(SeedType seedType, int quantity)
        {
            if (seedType != SeedType) return;
            if (quantity == 0) 
                Deselect();
            
            UpdateSeedQuantityText(quantity);
        }
    }
}
