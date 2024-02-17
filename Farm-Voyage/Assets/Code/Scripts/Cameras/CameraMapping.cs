using Cinemachine;

namespace Cameras
{
    public struct CameraMapping
    {
        public readonly CameraState State;
        public readonly CinemachineVirtualCamera Camera;

        public CameraMapping(CameraState state, CinemachineVirtualCamera camera)
        {
            State = state;
            Camera = camera;
        }
    }
}