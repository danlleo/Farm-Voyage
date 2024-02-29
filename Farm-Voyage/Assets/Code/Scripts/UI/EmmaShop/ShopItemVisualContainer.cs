using Attributes.WithinParent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EmmaShop
{
    [DisallowMultipleComponent]
    public class ShopItemVisualContainer : MonoBehaviour
    {
        [Header("External references")] 
        [SerializeField] private Image _image;
        [SerializeField, WithinParent] private TextMeshProUGUI _dirtPriceText;
        [SerializeField, WithinParent] private TextMeshProUGUI _woodPriceText;
        [SerializeField, WithinParent] private TextMeshProUGUI _rockPriceText;

        public void Initialize(Sprite icon, int dirtPrice, int woodPrice, int rockPrice)
        {
            _image.sprite = icon;
            _dirtPriceText.text = $"{dirtPrice}";
            _woodPriceText.text = $"{woodPrice}";
            _rockPriceText.text = $"{rockPrice}";
        }
    }
}
