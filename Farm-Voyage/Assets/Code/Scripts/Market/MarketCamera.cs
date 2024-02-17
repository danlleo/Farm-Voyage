using Cameras;
using Cinemachine;
using UnityEngine;
using CameraState = Cameras.CameraState;

namespace Market
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    [DisallowMultipleComponent]
    public class MarketCamera : MonoBehaviour, IControllableCamera
    {
        public CameraMapping Mapping => new(CameraState.Market, GetComponent<CinemachineVirtualCamera>());
    }
}
