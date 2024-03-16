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

        private void OnEnable()
        {
            UpdateSeedQuantityText(_playerInventory.GetSeedsQuantity(SeedType));
            
            if (_playerInventory != null)
            {
                _playerInventory.OnSeedQuantityChanged += PlayerInventory_OnSeedQuantityChanged;
            }
        }

        private void OnDisable()
        {
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
            OnAnySelectedSeedChooseItemEvent?.Invoke(this);
        }

        public void Select()
        {
            _backgroundImage.sprite = _selectedBackgroundSprite;
            _playerInventory.SetSelectedSeed(SeedType);
        }
        
        public void Deselect()
        {
            _backgroundImage.sprite = _defaultBackgroundSprite;
            _playerInventory.SetSelectedSeed(SeedType.Default);
        }

        private void UpdateSeedQuantityText(int quantity)
        {
            _seedQuantityText.text = $"{quantity}";
        }
        
        private void PlayerInventory_OnSeedQuantityChanged(SeedType seedType, int quantity)
        {
            if (seedType != SeedType) return;
            
            UpdateSeedQuantityText(quantity);
        }
    }
}
