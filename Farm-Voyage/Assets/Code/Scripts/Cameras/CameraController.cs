using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Cameras
{
    [DisallowMultipleComponent]
    public class CameraController : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private List<CameraMapping> _cameraMappingsList;

        private Dictionary<CameraState, CinemachineVirtualCamera> _camerasDictionary;

        private void Awake()
        {
            _camerasDictionary = new Dictionary<CameraState, CinemachineVirtualCamera>();

            foreach (CameraMapping mapping in _cameraMappingsList)
            {
                _camerasDictionary[mapping.State] = mapping.Camera;
            }
        }
    }
}