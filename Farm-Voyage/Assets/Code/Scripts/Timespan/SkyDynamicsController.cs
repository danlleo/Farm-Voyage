using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Timespan
{
    [DisallowMultipleComponent]
    public class SkyDynamicsController : MonoBehaviour
    {
        private static readonly int s_blend = Shader.PropertyToID("_Blend");
        
        [Header("External references")]
        [SerializeField] private Light _sun;
        [SerializeField] private Light _moon;
        [SerializeField] private Material _skyboxMaterial;
        
        [Header("Settings")]
        [SerializeField] private AnimationCurve _lightIntensityCurve;
        [SerializeField] private float _maxSunIntensity = 1f;
        [SerializeField] private float _maxMoonIntensity = .5f;

        [Space(10)] 
        [SerializeField] private Color _dayAmbientLight;
        [SerializeField] private Color _nightAmbientLight;

        [Space(10)]
        [SerializeField] private Volume _volume;

        private ColorAdjustments _colorAdjustments;
        private TimeManager _timeManager;

        [Inject]
        private void Construct(TimeManager timeManager)
        {
            _timeManager = timeManager;
        }

        private void OnEnable()
        {
            _timeManager.Service.OnCurrentTimeChanged += TimeService_OnCurrentTimeChanged;
        }

        private void OnDisable()
        {
            _timeManager.Service.OnCurrentTimeChanged -= TimeService_OnCurrentTimeChanged;
        }

        private void Start()
        {
            _volume.profile.TryGet(out _colorAdjustments);
        }

        private void UpdateLightSettings()
        {
            float dotProduct = Vector3.Dot(_sun.transform.forward, Vector3.down);
            _sun.intensity = Mathf.Lerp(0, _maxSunIntensity, _lightIntensityCurve.Evaluate(dotProduct));
            _moon.intensity = Mathf.Lerp(_maxMoonIntensity, 0, _lightIntensityCurve.Evaluate(dotProduct));

            if (_colorAdjustments == null) return;

            _colorAdjustments.colorFilter.value = Color.Lerp(_nightAmbientLight, _dayAmbientLight,
                _lightIntensityCurve.Evaluate(dotProduct));
        }
        
        private void RotateSun()
        {
            float rotation = _timeManager.Service.CalculateSunAngle();
            _sun.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.right);
        }

        private void UpdateSkyBlend()
        {
            float dotProduct = Vector3.Dot(_sun.transform.forward, Vector3.up);
            float blend = Mathf.Lerp(0f, 1f, _lightIntensityCurve.Evaluate(dotProduct));
            _skyboxMaterial.SetFloat(s_blend, blend);
        }
        
        private void TimeService_OnCurrentTimeChanged()
        {
            RotateSun();
            UpdateLightSettings();
            UpdateSkyBlend();
        }
    }
}