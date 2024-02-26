using Cameras;
using Cinemachine;
using UnityEngine;
using CameraState = Cameras.CameraState;

namespace Workbench
{
    [DisallowMultipleComponent]
    public class WorkbenchCamera : MonoBehaviour, IControllableCamera
    {
        public CameraMapping Mapping => new(CameraState.Workbench, GetComponent<CinemachineVirtualCamera>());
    }
}
