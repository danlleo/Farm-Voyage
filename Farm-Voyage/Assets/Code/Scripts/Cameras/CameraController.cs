using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Cameras
{
    [DisallowMultipleComponent]
    public class CameraController : MonoBehaviour
    {
        private HashSet<CameraMapping> _camerasHashSet;
        private Dictionary<CameraState, CinemachineVirtualCamera> _camerasDictionary;

        private void Awake()
        {
            FindAllControllableCameras();
            InitializeCamerasDictionary();
        }

        public void SwitchToCamera(CameraState cameraState)
        {
            if (!_camerasDictionary.TryGetValue(cameraState, out CinemachineVirtualCamera cameraToActivate)) return;
            
            foreach (CinemachineVirtualCamera camera in _camerasDictionary.Values)
            {
                camera.gameObject.SetActive(false);
            }
            
            cameraToActivate.gameObject.SetActive(true);
        }
        
        private void FindAllControllableCameras()
        {
            CinemachineVirtualCamera[] allCameras = FindObjectsOfType<CinemachineVirtualCamera>();

            foreach (CinemachineVirtualCamera cinemachineVirtualCamera in allCameras)
            {
                if (cinemachineVirtualCamera is IControllableCamera controllableCamera)
                {
                    _camerasHashSet.Add(new CameraMapping(controllableCamera.State, cinemachineVirtualCamera));
                }
            }
        }
        
        private void InitializeCamerasDictionary()
        {
            _camerasDictionary = new Dictionary<CameraState, CinemachineVirtualCamera>();

            foreach (CameraMapping mapping in _camerasHashSet)
            {
                _camerasDictionary[mapping.State] = mapping.Camera;
            }
        }
    }
}