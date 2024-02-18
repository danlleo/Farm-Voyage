using Cameras;
using Cinemachine;
using UnityEngine;
using CameraState = Cameras.CameraState;

namespace Seller
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class SellerCamera : MonoBehaviour, IControllableCamera
    {
        public CameraMapping Mapping => new(CameraState.Seller, GetComponent<CinemachineVirtualCamera>());
    }
}