using System;
using Cameras;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Zenject;
using CameraState = Cameras.CameraState;

namespace Character.Player
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    [DisallowMultipleComponent]
    public class PlayerFollowCamera : MonoBehaviour, IControllableCamera
    {
        private const float RotateDuration = .20f;
        private const float ZoomDuration = .30f;
        private const float TargetZoomInValue = 35f;

        public CameraMapping Mapping => new(CameraState.Main, GetComponent<CinemachineVirtualCamera>());
        
        private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private Player _player;

        private Quaternion _initialRotation;
        private float _initialZoomValue;
        
        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }
        
        private void Awake()
        {
            _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            _initialZoomValue = _cinemachineVirtualCamera.m_Lens.FieldOfView;
            _initialRotation = transform.rotation; 
                
            SetCameraBoundaries();
        }

        private void OnEnable()
        {
            _player.Events.LeftHomeEvent.OnPlayerLeftHome += OnLeftHome;
        }

        private void OnDisable()
        {
            _player.Events.LeftHomeEvent.OnPlayerLeftHome -= OnLeftHome;
        }

        public void ZoomIn()
        {
            DOVirtual.Float(_initialZoomValue, TargetZoomInValue, ZoomDuration, UpdateFOVValue)
                .SetEase(Ease.Linear);
        }
        
        public void ZoomOut()
        {
            float currentZoomValue = _cinemachineVirtualCamera.m_Lens.FieldOfView;
            
            DOVirtual.Float(currentZoomValue, _initialZoomValue, ZoomDuration, UpdateFOVValue)
                .SetEase(Ease.Linear);
        }

        public void ZoomOutOfPlayer()
        {
            float currentZoomValue = _cinemachineVirtualCamera.m_Lens.FieldOfView;
            const float targetZoomValue = 65f;
            
            DOVirtual.Float(currentZoomValue, targetZoomValue, ZoomDuration, UpdateFOVValue)
                .SetEase(Ease.Linear);
        }
        
        public void RotateCameraTowardsAngles(Vector2 targetRotation)
        {
            transform.DORotate(new Vector3(targetRotation.x, targetRotation.y, _initialRotation.z),
                RotateDuration);
        }

        public void ResetCameraRotation()
        {
            transform.DORotate(_initialRotation.eulerAngles, RotateDuration);
        }

        public void LooseTarget()
        {
            _cinemachineVirtualCamera.Follow = null;
        }
        
        private void SetPlayerFollowTarget()
        {
            _cinemachineVirtualCamera.Follow = _player.transform;
        }

        private void SetCameraBoundaries()
        {
            GameObject cameraBoundaryObject = new("Camera Boundary")
            {
                transform =
                {
                    position = Vector3.zero
                }
            };

            BoxCollider boxCollider = cameraBoundaryObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            boxCollider.size = new Vector3(40f, 50f, 60f);
            
            CinemachineConfiner cinemachineConfiner = _cinemachineVirtualCamera.gameObject.AddComponent<CinemachineConfiner>();

            cinemachineConfiner.m_ConfineMode = CinemachineConfiner.Mode.Confine3D;
            cinemachineConfiner.m_BoundingVolume = boxCollider;
        }
        
        private void UpdateFOVValue(float value)
            => _cinemachineVirtualCamera.m_Lens.FieldOfView = value;
        
        private void OnLeftHome(object sender, EventArgs e)
        {
            SetPlayerFollowTarget();
        }
    }
}
