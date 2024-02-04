using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManagers
{
    [DisallowMultipleComponent]
    public class PlayerInput : MonoBehaviour
    {
        public Vector2 MovementInput { get; private set; }
        public float MoveAmount { get; private set; }

        private PlayerControls _playerControls;

        private float _verticalInput;
        private float _horizontalInput;
        
        private void OnEnable()
        {
            if (_playerControls == null)
            {
                _playerControls = new PlayerControls();
                _playerControls.PlayerMovement.KeyboardMovement.performed += ReadMovementValue;
            }
            
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }

        private void Update()
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
    }
}
