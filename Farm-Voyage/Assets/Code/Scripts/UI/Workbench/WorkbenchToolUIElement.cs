using Attributes.WithinParent;
using Farm.FarmResources;
using Farm.Tool;
using Level;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Workbench
{
    [DisallowMultipleComponent]
    public class WorkbenchToolUIElement : MonoBehaviour, IPointerClickHandler
    {
        [Header("External references")]
        [SerializeField, WithinParent] private TextMeshProUGUI _nameText;
        [SerializeField, WithinParent] private TextMeshProUGUI _levelText;
        
        [Space(10)]
        [SerializeField, WithinParent] private TextMeshProUGUI _dirtPriceText;
        [SerializeField, WithinParent] private TextMeshProUGUI _rockPriceText;
        [SerializeField, WithinParent] private TextMeshProUGUI _woodPriceText;

        private Economy _economy;
        private Tool _tool;
        
        private string _name;
        private int _level;

        private void OnEnable()
        {
            if (_tool != null)
                _tool.OnLevelUp += Tool_OnLevelUp;
        }

        private void OnDisable()
        {
            _tool.OnLevelUp -= Tool_OnLevelUp;
        }

        private void Start()
        {
            UpdateNameText();
            UpdateLevelText();
            UpdateDisplayPricesTexts();
        }

        public void Initialize(Economy economy, Tool tool, string name, int level)
        {
            _economy = economy;
            _tool = tool;
            _name = name;
            _level = level;

            _tool.OnLevelUp += Tool_OnLevelUp;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_economy.TryPurchaseWithResources(_tool.ResourcesPrices))
            {
                _tool.IncreaseLevel();
            }
            else
            {
                print("Not enough resources");
            }
        }

        private void UpdateNameText()
        {
            _nameText.text = _name;
        }

        private void UpdateLevelText()
        {
            if (_level == Tool.MaxLevel)
            {
                _levelText.text = "Lvl. MAX";
                return;
            }
            
            _levelText.text = $"Lvl. {_level}";
        }

        private void UpdateDisplayPricesTexts()
        {
            _dirtPriceText.text = $"{_tool.GetPriceByRecourseType(ResourceType.Dirt)}";
            _rockPriceText.text = $"{_tool.GetPriceByRecourseType(ResourceType.Rock)}";
            _woodPriceText.text = $"{_tool.GetPriceByRecourseType(ResourceType.Wood)}";
        }
        
        private void Tool_OnLevelUp(int level)
        {
            _level = level;
            UpdateLevelText();
        }
    }
}