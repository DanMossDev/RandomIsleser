using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RandomIsleser
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _inputActions;
        
        public static event Action<Vector2> MoveInput;
        public static event Action<Vector2> CameraInput;
        public static event Action RollInput;

        public static event Action<bool> TargetInput;
        
        //Input cache
        private Vector2 _moveInput;
        private Vector2 _cameraInput;
        private bool _targetHeld;

        private void OnEnable()
        {
            Enable();
            Subscribe();
        }

        public void Enable()
        {
            _inputActions.Enable();
        }

        public void Disable()
        {
            _inputActions.Disable();
        }

        private void Subscribe()
        {
            _inputActions["Move"].started += OnMove;
            _inputActions["Move"].performed += OnMove;
            _inputActions["Move"].canceled += OnMove;
            
            _inputActions["Look"].started += OnLook;
            _inputActions["Look"].performed += OnLook;
            _inputActions["Look"].canceled += OnLook;
            
            _inputActions["Roll"].performed += OnRoll;

            _inputActions["Target"].started += OnTargetStart;
            _inputActions["Target"].canceled += OnTargetEnd;
        }

        private void Unsubscribe()
        {
            _inputActions["Move"].started -= OnMove;
            _inputActions["Move"].performed -= OnMove;
            _inputActions["Move"].canceled -= OnMove;
            
            _inputActions["Look"].started -= OnLook;
            _inputActions["Look"].performed -= OnLook;
            _inputActions["Look"].canceled -= OnLook;
            
            _inputActions["Roll"].performed -= OnRoll;
            
            _inputActions["Target"].started -= OnTargetStart;
            _inputActions["Target"].canceled -= OnTargetEnd;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            MoveInput?.Invoke(_moveInput);
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            _cameraInput = context.ReadValue<Vector2>();
            CameraInput?.Invoke(_cameraInput);
        }

        private void OnRoll(InputAction.CallbackContext context)
        {
            RollInput?.Invoke();
        }

        private void OnTargetStart(InputAction.CallbackContext context)
        {
            TargetInput?.Invoke(true);
        }

        private void OnTargetEnd(InputAction.CallbackContext context)
        {
            TargetInput?.Invoke(false);
        }
    }
}
