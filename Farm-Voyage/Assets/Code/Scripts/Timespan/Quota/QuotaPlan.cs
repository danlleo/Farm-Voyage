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

            foreach (MeetQuotaData meetQuotaData in _quota.MeetQuotaDatas)
            {
                Debug.Log($"Seed: {meetQuotaData.Seed}, with amount of: {meetQuotaData.Quantity}");
            }
        }

        public bool TryFinishQuotaPlan(PlayerInventory playerInventory)
        {
            List<MeetQuotaData> meetQuotaDatasToSpend = new();
            
            foreach (MeetQuotaData meetQuotaData in _quota.MeetQuotaDatas)
            {
                if (!playerInventory.HasEnoughSeedQuantity(meetQuotaData.Seed, meetQuotaData.Quantity))
                {
                    return false;
                }
                
                meetQuotaDatasToSpend.Add(meetQuotaData);
            }

            foreach (MeetQuotaData meetQuotaData in meetQuotaDatasToSpend)
            {
                playerInventory.RemoveSeedQuantity(meetQuotaData.Seed, meetQuotaData.Quantity);
            }

            _quota.OnMetQuota();
            
            return true;
        }
    }
}
