using Cinemachine;
using UnityEngine;

namespace Cameras
{
    [System.Serializable]
    public struct CameraMapping
    {
        [field:SerializeField] public CameraState State { get; private set; }
        [field:SerializeField] public CinemachineVirtualCamera Camera { get; private set; }
    }
}