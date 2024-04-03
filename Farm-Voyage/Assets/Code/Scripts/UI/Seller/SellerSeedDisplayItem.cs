using Character.Player;
using Farm.Plants;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Seller
{
    [DisallowMultipleComponent]
    public class SellerSeedDisplayItem : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private Image _seedImage;
        [SerializeField] private Image _progressBarImage;
        [SerializeField] private TextMeshProUGUI _quantityText;

        private int _targetQuantity;

        private PlayerInventory _playerInventory;

        public void Initialize(PlayerInventory playerInventory, PlantType plantType, Sprite seedSprite,
            int targetQuantity)
        {
            _playerInventory = playerInventory;
            _seedImage.sprite = seedSprite;
            _targetQuantity = targetQuantity;

            UpdateProgress(playerInventory.GetPlantQuantity(plantType));
        }

        private void UpdateProgress(int quantity)
        {
            UpdateProgressBar(quantity);
            UpdateSeedQuantityText(quantity);
        }
        
        private void UpdateProgressBar(int quantity)
        {
            _progressBarImage.fillAmount = (float)quantity / _targetQuantity;
        }
            
        private void UpdateSeedQuantityText(int quantity)
        {
            _quantityText.text = $"{quantity}/{_targetQuantity}";
        }
    }
}