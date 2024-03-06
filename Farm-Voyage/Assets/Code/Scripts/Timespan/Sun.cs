using UnityEngine;
using Zenject;

namespace Timespan
{
    [RequireComponent(typeof(Light))]
    [DisallowMultipleComponent]
    public class Sun : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _maxTemperature;
        [SerializeField] private float _minTemperature;
        
        private float _sunRotationSpeed;
        
        private Light _sunLightSource;
        private Day _day;
        
        [Inject]
        private void Construct(Day day)
        {
            _day = day;
        }

        private void Awake()
        {
            _sunLightSource = GetComponent<Light>();
            
            SetInitialSunTemperature();
            CalculateSunRotationSpeed();
        }

        private void OnEnable()
        {
            _day.OnTimeChanged += Day_OnTimeChanged;
        }

        private void OnDisable()
        {
            _day.OnTimeChanged -= Day_OnTimeChanged;
        }
        
        private void SetInitialSunTemperature()
        {
            _sunLightSource.colorTemperature = _maxTemperature;
        }
        
        private void CalculateSunRotationSpeed()
        {
            _sunRotationSpeed = 360f / _day.DayDurationInSeconds;
        }

        private void BlendSunTemperatureDependingOnCurrentTime(float currentTimeInSeconds)
        {
            float normalizedTime = Mathf.InverseLerp(_day.DayInitialTimeInSeconds, _day.DayDurationInSeconds,
                currentTimeInSeconds);
            float currentSunTemperature =
                Mathf.Lerp(_maxTemperature, _minTemperature, normalizedTime);

            _sunLightSource.colorTemperature = currentSunTemperature;
        }
        
        private void RotateSun()
        {
            float angleThisFrame = _sunRotationSpeed * Time.deltaTime / 2;
            transform.RotateAround(transform.position, Vector3.forward, angleThisFrame);
        }
        
        private void Day_OnTimeChanged(float currentTime, float dayDuration)
        {
            RotateSun();
            BlendSunTemperatureDependingOnCurrentTime(currentTime);
        }
    }
}