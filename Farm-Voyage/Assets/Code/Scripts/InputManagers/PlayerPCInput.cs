using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace InputManagers
{
    public class PlayerPCInput : IInitializable, IDisposable, ITickable, IPlayerInput
    {
        public event Action OnInteract;
        public Vector2 MovementInput { get; private set; }
        public float MoveAmount { get; private set; }

        private PlayerControls _playerControls;

        private float _verticalInput;
        private float _horizontalInput;
        
        public void Initialize()
        {
            if (_playerControls == null)
            {
                _playerControls = new PlayerControls();
                _playerControls.PlayerMovement.KeyboardMovement.performed += ReadMovementValue;
            }
            
            _playerControls.Enable();
            _playerControls.PlayerMovement.MouseClick.performed += MouseClick_OnPerformed;
        }

        public void Dispose()
        {
            _playerControls.Disable();
            _playerControls.PlayerMovement.MouseClick.performed -= MouseClick_OnPerformed;
        }
        
        public void Tick()
        {
            HandleMovementInput();
        }
        
        private void ReadMovementValue(InputAction.CallbackContext callbackContext)
        {
            MovementInput = callbackContext.ReadValue<Vector2>();
        }

        private void HandleMovementInput()
        {
            _verticalInput = MovementInput.y;
            _horizontalInput = MovementInput.x;
            MoveAmount = Mathf.Clamp01(Mathf.Abs(_verticalInput) + Mathf.Abs(_horizontalInput));

            switch (MoveAmount)
            {
                case <= 0.5f and > 0:
                    MoveAmount = 0.5f;
                    break;
                case > 0.5f and <= 1f:
                    MoveAmount = 1f;
                    break;
            }
        }
        
        private void MouseClick_OnPerformed(InputAction.CallbackContext obj)
        {
            OnInteract?.Invoke();
        }
    }
}
