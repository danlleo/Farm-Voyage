using System;
using System.Collections;
using System.Collections.Generic;
using Attributes.WithinParent;
using Character.Player;
using DG.Tweening;
using Farm.FarmResources;
using Farm.Tool;
using Farm.Tool.ConcreteTools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    [DisallowMultipleComponent]
    public class GameplayUI : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField, WithinParent] private TextMeshProUGUI _dirtQuantityText;
        [SerializeField, WithinParent] private TextMeshProUGUI _woodQuantityText;
        [SerializeField, WithinParent] private TextMeshProUGUI _rockQuantityText;

        [Space(10)] 
        [SerializeField, WithinParent] private Image _waterCanBarImage;
        
        [Header("Settings")]
        [SerializeField, Range(0.1f, 1f)] private float _lerpQuantityTimeInSeconds;
        [SerializeField, Range(0.1f, 1f)] private float _timeToFillWaterCanBarInSeconds = 0.2f;
        
        private Dictionary<TextMeshProUGUI, int> _resourceTextQuantityDictionary;
        
        private PlayerInventory _playerInventory;
        private WaterCan _waterCan;
        
        private Coroutine _quantityTextLerpingRoutine;
        
        [Inject]
        private void Construct(PlayerInventory playerInventory)
        {
            _playerInventory = playerInventory;
        }

        private void Awake()
        {
            InitializeRecourseTextQuantityDictionary();
            InitializeResourcesQuantityText();
        }

        private void OnEnable()
        {
            _playerInventory.OnResourceQuantityChanged += PlayerInventory_OnResourceQuantityChanged;

            if (!_playerInventory.TryGetTool(out WaterCan waterCan)) return;
            
            _waterCan = waterCan;
            _waterCan.OnWaterAmountChanged += WaterCan_OnWaterAmountChanged;
        }

        private void OnDisable()
        {
            _playerInventory.OnResourceQuantityChanged -= PlayerInventory_OnResourceQuantityChanged;

            if (!_playerInventory.TryGetTool(out WaterCan _)) return;
            _waterCan.OnWaterAmountChanged -= WaterCan_OnWaterAmountChanged;
        }

        private void InitializeResourcesQuantityText()
        {
            _dirtQuantityText.text = $"{_resourceTextQuantityDictionary[_dirtQuantityText]}";
            _woodQuantityText.text = $"{_resourceTextQuantityDictionary[_woodQuantityText]}";
            _rockQuantityText.text = $"{_resourceTextQuantityDictionary[_rockQuantityText]}";
        }

        private void InitializeRecourseTextQuantityDictionary()
        {
            _resourceTextQuantityDictionary = new Dictionary<TextMeshProUGUI, int>
            {
                { _dirtQuantityText, _playerInventory.GetResourceQuantity(ResourceType.Dirt) },
                { _woodQuantityText, _playerInventory.GetResourceQuantity(ResourceType.Wood) },
                { _rockQuantityText, _playerInventory.GetResourceQuantity(ResourceType.Rock) }
            };
        }

        private void UpdateResourceQuantityQuantityText(ResourceType resourceType, int quantity)
        {
            switch (resourceType)
            {
                case ResourceType.Rock:
                    InterpolateQuantityText(_rockQuantityText, quantity);
                    _resourceTextQuantityDictionary[_rockQuantityText] = quantity;
                    break;
                case ResourceType.Wood:
                    InterpolateQuantityText(_woodQuantityText, quantity);
                    _resourceTextQuantityDictionary[_woodQuantityText] = quantity;
                    break;
                case ResourceType.Dirt:
                    InterpolateQuantityText(_dirtQuantityText, quantity);
                    _resourceTextQuantityDictionary[_dirtQuantityText] = quantity;
                    break;
                case ResourceType.Water:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
            }
        }

        private void InterpolateQuantityText(TextMeshProUGUI textMeshProUGUI, int quantity)
        {
            if (_quantityTextLerpingRoutine != null)
                StopCoroutine(_quantityTextLerpingRoutine);

            _quantityTextLerpingRoutine = StartCoroutine(LerpQuantityRoutine(textMeshProUGUI, quantity));
        }
        
        private IEnumerator LerpQuantityRoutine(TextMeshProUGUI textMeshProUGUI, int quantity)
        {
            int startQuantity = _resourceTextQuantityDictionary[textMeshProUGUI];
            float time = 0f;

            while (time < _lerpQuantityTimeInSeconds)
            {
                time += Time.deltaTime;
                float t = time / _lerpQuantityTimeInSeconds;
                textMeshProUGUI.text = $"{(int)Mathf.Lerp(startQuantity, quantity, t)}";
                
                yield return null;
            }
        }

        private void UpdateWaterCanBarFilledAmount(int timesCanWater, int maxTimesCanWater)
        {
            float percent = (float)timesCanWater / maxTimesCanWater;

            _waterCanBarImage.DOKill();
            _waterCanBarImage.DOFillAmount(percent, _timeToFillWaterCanBarInSeconds);
        }
        
        private void PlayerInventory_OnResourceQuantityChanged(ResourceType resourceType, int quantity)
        {
            UpdateResourceQuantityQuantityText(resourceType, quantity);
        }
        
        private void WaterCan_OnWaterAmountChanged(int timesCanWater, int maxTimesCanWater)
        {
            UpdateWaterCanBarFilledAmount(timesCanWater, maxTimesCanWater);
        }
    }
}