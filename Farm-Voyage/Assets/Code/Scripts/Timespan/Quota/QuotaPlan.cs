using System;
using System.Collections.Generic;
using Character.Player;
using Timespan.Quota.ConcreteQuotas;
using UnityEngine;

namespace Timespan.Quota
{
    public sealed class QuotaPlan
    {
        private readonly Quota _quota;

        public QuotaPlan(QuotaPlanSettingsSO quotaPlanSettings)
        {
            _quota = quotaPlanSettings.QuotaDifficulty switch
            {
                QuotaDifficulty.Easy => new EasyQuota(quotaPlanSettings.EasyQuotaData),
                QuotaDifficulty.Medium => new MediumQuota(quotaPlanSettings.MediumQuotaData),
                QuotaDifficulty.Hard => new HardQuota(quotaPlanSettings.HardQuotaData),
                QuotaDifficulty.Nightmare => new NightmareQuota(quotaPlanSettings.NightmareQuotaData),
                _ => throw new ArgumentOutOfRangeException()
            };

            foreach (var VARIABLE in ReadQuotaPlan())
            {
                Debug.Log($"{VARIABLE.PlantType}: {VARIABLE.Quantity}");
            }
        }

        public IEnumerable<MeetQuotaData> ReadQuotaPlan()
        {
            return _quota.MeetQuotaData;
        }
        
        public bool TryFinishQuotaPlan(PlayerInventory playerInventory)
        {
            List<MeetQuotaData> meetQuotaDataToSpend = new();
            
            foreach (MeetQuotaData meetQuotaData in _quota.MeetQuotaData)
            {
                if (!playerInventory.HasEnoughPlantQuantity(meetQuotaData.PlantType, meetQuotaData.Quantity))
                {
                    return false;
                }

                meetQuotaDataToSpend.Add(meetQuotaData);
            }

            foreach (MeetQuotaData meetQuotaData in meetQuotaDataToSpend)
            {
                playerInventory.RemovePlantQuantity(meetQuotaData.PlantType, meetQuotaData.Quantity);
            }

            _quota.OnMetQuota();
            
            return true;
        }
    }
}
