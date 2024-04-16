using UnityEngine;

namespace Timespan
{
    [CreateAssetMenu(fileName = "TimeSettings_", menuName = "Scriptable Objects/Time/TimeSettings")]
    public class TimeSettingsSO : ScriptableObject
    {
        [field: SerializeField] public float TimeMultiplier { get; private set; } = 2000;
        [field: SerializeField] public float StartHour { get; private set; } = 12;
        [field: SerializeField] public float SunriseHour { get; private set; } = 6;
        [field: SerializeField] public float SunsetHour { get; private set; } = 18;
    }
}
