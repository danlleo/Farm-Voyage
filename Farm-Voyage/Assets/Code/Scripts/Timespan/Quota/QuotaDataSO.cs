using System.Collections.Generic;
using UnityEngine;

namespace Timespan.Quota
{
    [CreateAssetMenu(fileName = "QuotaData_", menuName = "Scriptable Objects/Farm/QuotaData")]
    public class QuotaDataSO : ScriptableObject
    {
        public IReadOnlyList<QuotaItem> QuotaItemsList => _quotaItemsList;

        [field: SerializeField, Min(1), Tooltip("Quantity can't be more than")]
        public int GenerateQuotaMeetDataItemsCount { get; private set; }  
        
        [SerializeField] private List<QuotaItem> _quotaItemsList;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_quotaItemsList.Count < GenerateQuotaMeetDataItemsCount)
                Debug.LogWarning("Quota items list count is less than Generate Quota Meet Data Items Count");
        }
#endif
    }
}