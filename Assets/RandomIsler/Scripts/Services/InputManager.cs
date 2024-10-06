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
        public static event Action HammerAttackInput;
        public static event Action<bool> ItemSlot1Input;
        public static event Action BackInput;
        public static event Action<bool> SuctionInput;
        public static event Action<bool> BlowInput;

        
        //Input cache
        private Vector2 _moveInput;
        private Vector2 _cameraInput;
        private bool _targetHeld;

        private void OnEnable()
        {
            EnableInput();
            Subscribe();
        }

        private void OnDisable()
        {
            DisableInput();
            Unsubscribe();
        }

        public void EnableInput()
        {
            _inputActions.Enable();
        }

        public void DisableInput()
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

            _inputActions["HammerAttack"].performed += OnHammerAttack;

            _inputActions["Suck"].started += OnSuctionStart;
            _inputActions["Suck"].canceled += OnSuctionEnd;
            
            _inputActions["Blow"].started += OnBlowStart;
            _inputActions["Blow"].canceled += OnBlowEnd;

            _inputActions["ItemSlot1"].started += OnItemSlot1Pressed;
            _inputActions["ItemSlot1"].canceled += OnItemSlot1Released;

            _inputActions["Back"].performed += OnBack;
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
            
            _inputActions["HammerAttack"].performed -= OnHammerAttack;
            
            _inputActions["Suck"].started -= OnSuctionStart;
            _inputActions["Suck"].canceled -= OnSuctionEnd;
            
            _inputActions["Blow"].started -= OnBlowStart;
            _inputActions["Blow"].canceled -= OnBlowEnd;
            
            _inputActions["ItemSlot1"].started -= OnItemSlot1Pressed;
            _inputActions["ItemSlot1"].canceled -= OnItemSlot1Released;
            
            _inputActions["Back"].performed -= OnBack;
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

        private void OnHammerAttack(InputAction.CallbackContext context)
        {
            HammerAttackInput?.Invoke();
        }
        
        private void OnItemSlot1Pressed(InputAction.CallbackContext context)
        {
            ItemSlot1Input?.Invoke(true);
        }

        private void OnItemSlot1Released(InputAction.CallbackContext context)
        {
            ItemSlot1Input?.Invoke(false);
        }

        private void OnBack(InputAction.CallbackContext context)
        {
            BackInput?.Invoke();
        }
        
        private void OnSuctionStart(InputAction.CallbackContext context)
        {
            SuctionInput?.Invoke(true);
        }

        private void OnSuctionEnd(InputAction.CallbackContext context)
        {
            SuctionInput?.Invoke(false);
        }
        
        private void OnBlowStart(InputAction.CallbackContext context)
        {
            BlowInput?.Invoke(true);
        }

        private void OnBlowEnd(InputAction.CallbackContext context)
        {
            BlowInput?.Invoke(false);
        }
    }
}
