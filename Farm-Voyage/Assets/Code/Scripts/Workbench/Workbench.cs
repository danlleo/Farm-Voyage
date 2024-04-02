using System;
using Cameras;
using Character.Player;
using Character.Player.Events;
using UI.Icon;
using UnityEngine;
using Zenject;

namespace Workbench
{
    [SelectionBase]
    [RequireComponent(typeof(StartedUsingWorkbenchEvent))]
    [DisallowMultipleComponent]
    public class Workbench : MonoBehaviour, IDisplayIcon
    {
        public StartedUsingWorkbenchEvent StartedUsingWorkbenchEvent { get; private set; }
        public Guid ID { get; private set; } = Guid.NewGuid();
        
        [field:SerializeField] public IconSO Icon { get; private set; }

        private Player _player;
        private CameraController _cameraController;
        
        [Inject]
        private void Construct(Player player, CameraController cameraController)
        {
            _player = player;
            _cameraController = cameraController;
        }
        
        private void Awake()
        {
            StartedUsingWorkbenchEvent = GetComponent<StartedUsingWorkbenchEvent>();
        }

        private void OnEnable()
        {
            StartedUsingWorkbenchEvent.OnStartedUsingWorkbench += StartedUsingWorkbenchEvent_OnStartedUsingWorkbench;
        }

        private void OnDisable()
        {
            StartedUsingWorkbenchEvent.OnStartedUsingWorkbench -= StartedUsingWorkbenchEvent_OnStartedUsingWorkbench;
        }

        private void StartedUsingWorkbenchEvent_OnStartedUsingWorkbench(object sender, EventArgs e)
        {
            _player.Events.UsingWorkbenchEvent.Call(this, new PlayerUsingWorkbenchEventArgs(true));
            _cameraController.SwitchToCamera(CameraState.Workbench);
        }
    }
}
