using System;
using System.Collections;
using Misc;
using UnityEngine;

namespace Timespan
{
    public sealed class Day
    {
        public event Action<float, float> OnTimeChanged;
        public event Action OnReachedSunset;
        public event Action OnDayEnded;
        
        public float DayInitialTimeInSeconds { get; private set; }
        public float DayDurationInSeconds { get; private set; } = 180f;
        
        private float _currentTime;

        private AsyncProcessor _asyncProcessor;
        
        public Day(AsyncProcessor asyncProcessor)
        {
            _asyncProcessor = asyncProcessor;
        }

        public void StartDay()
        {
            _asyncProcessor.StartCoroutine(TrackDayCycleRoutine());
        }
        
        private IEnumerator TrackDayCycleRoutine()
        {
            float sunsetStartTime = DayDurationInSeconds * 0.8f;
            
            bool reachedSunset = false;
            
            while (_currentTime < DayDurationInSeconds)
            {
                _currentTime += Time.deltaTime;
                OnTimeChanged?.Invoke(_currentTime, DayDurationInSeconds);
                
                if (!reachedSunset && _currentTime >= sunsetStartTime)
                {
                    OnReachedSunset?.Invoke();
                    reachedSunset = true;
                }
                
                yield return null;
            }
            
            OnDayEnded?.Invoke();
        }
    }
}
