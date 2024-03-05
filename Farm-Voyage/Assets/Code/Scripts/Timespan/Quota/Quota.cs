using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Timespan.Quota
{
    public class Quota
    {
        private readonly QuotaDataSO _quotaDataSO;
        private readonly IEnumerable<MeetQuotaData> _meetQuotaDatas;
        
        protected Quota(QuotaDataSO quotaDataSO)
        {
            _quotaDataSO = quotaDataSO;
            _meetQuotaDatas = GenerateMeetQuotaData(quotaDataSO);
        }

        private IEnumerable<MeetQuotaData> GenerateMeetQuotaData(QuotaDataSO quotaData)
        {
            List<QuotaItem> deepCopyQuotaItems = new(quotaData.QuotaItemsList);
            
            int counter = 0;
            int generateQuotaMeetDataItemsCount = quotaData.GenerateQuotaMeetDataItemsCount;
            
            if (generateQuotaMeetDataItemsCount > deepCopyQuotaItems.Count)
                throw new ArgumentException("Number of items is bigger than list size itself.");
            
            while (counter < generateQuotaMeetDataItemsCount)
            {
                int randomIndex = Random.Range(0, deepCopyQuotaItems.Count);
                QuotaItem quotaItem = deepCopyQuotaItems[randomIndex];
                
                if (quotaItem.QuantityMinimal > quotaItem.QuantityMaximum)
                    throw new ArgumentException("Minimal quantity can't be bigger than maximum.");

                int randomQuantity = Random.Range(quotaItem.QuantityMinimal, quotaItem.QuantityMaximum + 1);

                yield return new MeetQuotaData(quotaItem.Seed, randomQuantity);
                
                deepCopyQuotaItems.RemoveAt(randomIndex);
                counter++;
            }
        }
    }
}