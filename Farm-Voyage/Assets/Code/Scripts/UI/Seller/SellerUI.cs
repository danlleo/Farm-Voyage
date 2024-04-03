using System;
using System.Collections.Generic;
using Character.Player;
using Farm.Plants;
using Farm.Plants.Seeds;
using Timespan.Quota;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Seller
{
    [DisallowMultipleComponent]
    public class SellerUI : MonoBehaviour
    {
        [Header("External references")] 
        [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;
        [SerializeField] private SellerSeedDisplayItem _sellerSeedDisplayItemPrefab;

        [Space(10)] 
        [SerializeField] private Sprite _tomatoSprite;
        [SerializeField] private Sprite _turnipSprite;
        [SerializeField] private Sprite _cornSprite;
        [SerializeField] private Sprite _eggplantSprite;
        [SerializeField] private Sprite _carrotSprite;
        [SerializeField] private Sprite _pumpkinSprite;
        
        private QuotaPlan _quotaPlan;

        private IEnumerable<SellerSeedDisplayItem> _sellerSeedDisplayItems;

        private PlayerInventory _playerInventory;
        
        [Inject]
        private void Construct(QuotaPlan quotaPlan, PlayerInventory playerInventory)
        {
            _quotaPlan = quotaPlan;
            _playerInventory = playerInventory;
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
    }
}