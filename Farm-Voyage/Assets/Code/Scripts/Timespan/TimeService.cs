using System;
using Misc;
using UnityEngine;

namespace Timespan
{
    public sealed class TimeService
    {
        public event Action OnSunrise = delegate { };
        public event Action OnSunset = delegate { };
        public event Action OnHourChange = delegate { };
        public event Action OnCurrentTimeChanged = delegate { };
        
        private readonly TimeSettingsSO _timeSettings;
        private readonly TimeSpan _sunriseTime;
        private readonly TimeSpan _sunsetTime;
        private DateTime _currentTime;

        private readonly Observable<bool> _isDayTime;
        private readonly Observable<int> _currentHour;
        
        public TimeService(TimeSettingsSO timeSettings)
        {
            _timeSettings = timeSettings;
            _sunriseTime = TimeSpan.FromHours(timeSettings.SunriseHour);
            _sunsetTime = TimeSpan.FromHours(timeSettings.SunsetHour);
            _currentTime = DateTime.Now.Date + TimeSpan.FromHours(timeSettings.StartHour);

            _isDayTime = new Observable<bool>(IsDayTime());
            _currentHour = new Observable<int>(_currentTime.Hour);

            _isDayTime.OnValueChanged += day => (day ? OnSunrise : OnSunset)?.Invoke();
            _currentHour.OnValueChanged += _ => OnHourChange?.Invoke();
        }

        public void UpdateTime(float deltaTime)
        {
            _currentTime = _currentTime.AddSeconds(deltaTime * _timeSettings.TimeMultiplier);
            _isDayTime.Value = IsDayTime();
            _currentHour.Value = _currentTime.Hour;

            OnCurrentTimeChanged?.Invoke();
        }
        
        public float CalculateSunAngle()
        {
            bool isDay = IsDayTime();
            float startDegree = isDay ? 0 : 180;
            
            TimeSpan start = isDay ? _sunriseTime : _sunsetTime;
            TimeSpan end = isDay ? _sunsetTime : _sunriseTime;

            TimeSpan totalTime = CalculateDifference(start, end);
            TimeSpan elapsedTime = CalculateDifference(start, _currentTime.TimeOfDay);

            double percentage = elapsedTime.TotalMinutes / totalTime.TotalMinutes;

            return Mathf.Lerp(startDegree, startDegree + 180, (float)percentage);
        }
        
        public DateTime CurrentTime => _currentTime;
        
        private bool IsDayTime() => _currentTime.TimeOfDay > _sunriseTime && _currentTime.TimeOfDay < _sunsetTime;

        private TimeSpan CalculateDifference(TimeSpan from, TimeSpan to)
        {
            TimeSpan difference = to - from;
            return difference.TotalHours < 0 ? difference + TimeSpan.FromHours(24) : difference;
        }
    }
}
