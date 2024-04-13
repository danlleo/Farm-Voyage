using System;
using Cameras;
using Character.Player;
using UI.Icon;
using UnityEngine;
using Zenject;

namespace Workbench
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public class Workbench : MonoBehaviour, IDisplayIcon
    {
        public readonly UsingWorkbenchStateChangedEvent UsingWorkbenchStateChangedEvent = new();
        public Guid ID { get; } = Guid.NewGuid();
        
        [field:SerializeField] public IconSO Icon { get; private set; }

        private Player _player;
        private CameraController _cameraController;
        
        [Inject]
        private void Construct(Player player, CameraController cameraController)
        {
            _player = player;
            _cameraController = cameraController;
        }
        
        private void OnEnable()
        {
            UsingWorkbenchStateChangedEvent.OnUsingWorkbenchStateChanged += Workbench_OnUsingWorkbenchStateChanged;
        }

        private void OnDisable()
        {
            UsingWorkbenchStateChangedEvent.OnUsingWorkbenchStateChanged -= Workbench_OnUsingWorkbenchStateChanged;
        }

        private void Workbench_OnUsingWorkbenchStateChanged(bool isUsingWorkbench)
        {
            if (isUsingWorkbench)
            {
                _player.Events.UsingWorkbenchStateChangedEvent.Call(true);
                _cameraController.SwitchToCamera(CameraState.Workbench);
                return;
            }

            _player.Events.UsingWorkbenchStateChangedEvent.Call(false);
            _cameraController.SwitchToCamera(CameraState.Main);
        }
    }
}
