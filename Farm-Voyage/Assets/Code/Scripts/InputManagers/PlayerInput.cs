using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManagers
{
    [DisallowMultipleComponent]
    public class PlayerInput : MonoBehaviour
    {
        private PlayerControls _playerControls;

        private Vector2 _movementInput;

        private float _verticalInput;
        private float _horizontalInput;
        private float _moveAmount;
        
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

        private void ReadMovementValue(InputAction.CallbackContext callbackContext)
        {
            _movementInput = callbackContext.ReadValue<Vector2>();
        }

        private void HandleMovementInput()
        {
            _verticalInput = _movementInput.y;
            _horizontalInput = _movementInput.x;
            _moveAmount = Mathf.Clamp01(Mathf.Abs(_verticalInput) + Mathf.Abs(_horizontalInput));

            switch (_moveAmount)
            {
                case <= 0.5f and > 0:
                    _moveAmount = 0.5f;
                    break;
                case > 0.5f and <= 1f:
                    _moveAmount = 1f;
                    break;
            }
        }
    }
}
