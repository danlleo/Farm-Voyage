using UnityEngine;

namespace Timespan.Quota
{
    [CreateAssetMenu(fileName = "QuotaPlanSettings_", menuName = "Scriptable Objects/Farm/QuotaPlanSettings")]
    public class QuotaPlanSettingsSO : ScriptableObject
    {
        [field: SerializeField] public QuotaDifficulty QuotaDifficulty { get; private set; }
        [field: SerializeField] public QuotaDataSO EasyQuotaData { get; private set; }
        [field: SerializeField] public QuotaDataSO MediumQuotaData { get; private set; }
        [field: SerializeField] public QuotaDataSO HardQuotaData { get; private set; }
        [field: SerializeField] public QuotaDataSO NightmareQuotaData { get; private set; }
    }
}