using Cameras;
using UnityEngine;

namespace Market
{
    [DisallowMultipleComponent]
    public class MarketCamera : MonoBehaviour, IControllableCamera
    {
        public CameraState State { get; private set; } = CameraState.Shopping;
    }
}
