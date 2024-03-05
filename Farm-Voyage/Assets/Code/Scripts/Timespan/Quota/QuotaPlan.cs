using System;
using Timespan.Quota.ConcreteQuotas;

namespace Timespan.Quota
{
    public sealed class QuotaPlan
    {
        private readonly Quota _quota;

        private void Awake()
        {
            
        }

        public QuotaPlan(QuotaDifficulty quotaDifficulty)
        {
            _quota = quotaDifficulty switch
            {
                QuotaDifficulty.Easy => new EasyQuota(_easyQuotaData),
                QuotaDifficulty.Medium => new MediumQuota(_mediumQuotaData),
                QuotaDifficulty.Hard => new HardQuota(_hardQuotaData),
                QuotaDifficulty.Nightmare => new NightmareQuota(_nightmareQuotaData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
