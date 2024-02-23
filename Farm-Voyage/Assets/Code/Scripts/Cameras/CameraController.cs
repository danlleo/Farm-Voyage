using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Cameras
{
    [DisallowMultipleComponent]
    public class CameraController : MonoBehaviour
    {
        private const int BasePriority = 10;
        private const int ActivePriority = 100;
        
        private readonly HashSet<CameraMapping> _camerasHashSet = new();
        private readonly Dictionary<CameraState, CinemachineVirtualCamera> _camerasDictionary = new();

        private void Awake()
        {
            FindAllControllableCameras();
            InitializeCamerasDictionary();
        }

        public void SwitchToCamera(CameraState cameraState)
        {
            if (!_camerasDictionary.TryGetValue(cameraState, out CinemachineVirtualCamera cameraToActivate))
                return;
            
            foreach (CinemachineVirtualCamera cinemachineVirtualCamera in _camerasDictionary.Values)
            {
                cinemachineVirtualCamera.Priority = BasePriority;
            }

            cameraToActivate.Priority = ActivePriority;
        }
        
        private void FindAllControllableCameras()
        {
            MonoBehaviour[] allMonoBehaviours = FindObjectsOfType<MonoBehaviour>();

            foreach (MonoBehaviour monoBehaviour in allMonoBehaviours)
            {
                if (monoBehaviour is IControllableCamera controllableCamera)
                {
                    _camerasHashSet.Add(controllableCamera.Mapping);
                }
            }
        }
        
        private void InitializeCamerasDictionary()
        {
            foreach (CameraMapping mapping in _camerasHashSet)
            {
                _camerasDictionary[mapping.State] = mapping.Camera;
            }
        }
    }
}