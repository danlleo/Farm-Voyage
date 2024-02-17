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
        public CameraState State { get; private set; } = CameraState.Main;
        
        private float _rotateDuration = .20f;
        private float _zoomDuration = .30f;
        private float _targetZoomInValue = 35f;
        
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
                
            SetPlayerFollowTarget();
            SetCameraBoundaries();
        }

        public void ZoomIn()
        {
            DOVirtual.Float(_initialZoomValue, _targetZoomInValue, _zoomDuration, UpdateFOVValue)
                .SetEase(Ease.Linear);
        }
        
        public void ZoomOut()
        {
            float currentZoomValue = _cinemachineVirtualCamera.m_Lens.FieldOfView;
            
            DOVirtual.Float(currentZoomValue, _initialZoomValue, _zoomDuration, UpdateFOVValue)
                .SetEase(Ease.Linear);
        }
        
        public void RotateCameraTowardsAngles(Vector2 targetRotation)
        {
            transform.DORotate(new Vector3(targetRotation.x, targetRotation.y, _initialRotation.z),
                _rotateDuration);
        }

        public void ResetCameraRotation()
        {
            transform.DORotate(_initialRotation.eulerAngles, _rotateDuration);
        }
        
        private void SetPlayerFollowTarget()
        {
            _cinemachineVirtualCamera.Follow = _player.transform;
        }

        private void SetCameraBoundaries()
        {
            GameObject cameraBoundaryObject = new GameObject("Camera Boundary");
            cameraBoundaryObject.transform.position = Vector3.zero;

            BoxCollider collider = cameraBoundaryObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.size = new Vector3(40f, 50f, 60f);
            
            CinemachineConfiner cinemachineConfiner = _cinemachineVirtualCamera.gameObject.AddComponent<CinemachineConfiner>();

            cinemachineConfiner.m_ConfineMode = CinemachineConfiner.Mode.Confine3D;
            cinemachineConfiner.m_BoundingVolume = collider;
        }
        
        private void UpdateFOVValue(float value)
            => _cinemachineVirtualCamera.m_Lens.FieldOfView = value;
    }
}
