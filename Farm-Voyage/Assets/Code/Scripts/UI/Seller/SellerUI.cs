using System;
using System.Collections.Generic;
using Character.Player;
using DG.Tweening;
using Farm.Plants;
using Timespan.Quota;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Seller
{
    [RequireComponent(typeof(CanvasGroup))]
    [DisallowMultipleComponent]
    public class SellerUI : MonoBehaviour
    {
        private const float StartFadeValue = 0f;
        public event Action OnClosed;
        
        [Header("External references")] 
        [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;
        [SerializeField] private SellerSeedDisplayItem _sellerSeedDisplayItemPrefab;
        [SerializeField] private Button _closeButton;
        
        [Space(10)] 
        [SerializeField] private Sprite _tomatoSprite;
        [SerializeField] private Sprite _turnipSprite;
        [SerializeField] private Sprite _cornSprite;
        [SerializeField] private Sprite _eggplantSprite;
        [SerializeField] private Sprite _carrotSprite;
        [SerializeField] private Sprite _pumpkinSprite;

        [Header("Settings")]
        [SerializeField, Min(0)] private float _timeToFadeInSeconds = 0.35f;
        [SerializeField, Range(0f, 1f)] private float _endFadeValue = 1f;
        
        private CanvasGroup _canvasGroup;
        
        private IEnumerable<SellerSeedDisplayItem> _sellerSeedDisplayItems;

        private QuotaPlan _quotaPlan;
        private PlayerInventory _playerInventory;
        
        [Inject]
        private void Construct(QuotaPlan quotaPlan, PlayerInventory playerInventory)
        {
            _quotaPlan = quotaPlan;
            _playerInventory = playerInventory;
        }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(CloseUI);
            PlayFadeAnimation(_endFadeValue);
        }
        
        private void OnDisable()
        {
            _closeButton.onClick.RemoveAllListeners();
            KillFadeAnimation();
        }

        private void Start()
        {
            InitializeQuotaPlanItems();
        }

        private void InitializeQuotaPlanItems()
        {
            if (_sellerSeedDisplayItems != null) return;
            
            IEnumerable<MeetQuotaData> displayedQuotaDataItems = _quotaPlan.ReadQuotaPlan();

            foreach (MeetQuotaData meetQuotaData in displayedQuotaDataItems)
            {
                SellerSeedDisplayItem sellerSeedDisplayItem =
                    Instantiate(_sellerSeedDisplayItemPrefab, _verticalLayoutGroup.transform);

                Sprite targetSprite = meetQuotaData.PlantType switch
                {
                    PlantType.Tomato => _tomatoSprite,
                    PlantType.Eggplant => _eggplantSprite,
                    PlantType.Carrot => _carrotSprite,
                    PlantType.Pumpkin => _pumpkinSprite,
                    PlantType.Turnip => _turnipSprite,
                    PlantType.Corn => _cornSprite,
                    _ => throw new ArgumentOutOfRangeException()
                };

                sellerSeedDisplayItem.Initialize(_playerInventory, meetQuotaData.PlantType, targetSprite,
                    meetQuotaData.Quantity);
            }
        }
        
        private void CloseUI()
        {
            _closeButton.onClick.RemoveAllListeners();
            PlayFadeAnimation(StartFadeValue, () => OnClosed?.Invoke());
        }
        
        private void SetDefaultCanvasParams()
        {
            _canvasGroup.alpha = 0f;
        }
        
        private void PlayFadeAnimation(float targetValue, Action onComplete = null)
        {
            _canvasGroup.DOFade(targetValue, _timeToFadeInSeconds).OnComplete(() => onComplete?.Invoke());
        }

        private void KillFadeAnimation()
        {
            SetDefaultCanvasParams();
            _canvasGroup.DOKill();
        }
    }
}