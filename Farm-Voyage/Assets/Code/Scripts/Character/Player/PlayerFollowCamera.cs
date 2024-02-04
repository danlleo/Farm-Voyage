using Cinemachine;
using UnityEngine;
using Zenject;

namespace Character.Player
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class PlayerFollowCamera : MonoBehaviour
    {
        private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private Player _player;
        
        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }
        
        private void Awake()
        {
            _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            _cinemachineVirtualCamera.Follow = _player.transform;
        }
    }
}
