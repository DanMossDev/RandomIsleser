using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RandomIsleser
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _inputActions;

        public static event Action AnyInput;
        public static event Action InteractInput;
        public static event Action<Vector2> MoveInput;
        public static event Action<Vector2> CameraInput;
        public static event Action RollInput;
        public static event Action<bool> TargetInput;
        public static event Action HammerAttackInput;
        public static event Action<bool> ItemSlot1Input;
        public static event Action<bool> ChargeInput;
        public static event Action<bool> FireInput;
        
        public static event Action PauseInput;
        public static event Action AcceptInput;
        public static event Action BackInput;

        public static event Action<bool> TabInput;

        
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
            _inputActions["Any"].performed += OnAnyInput;
            
            _inputActions["Interact"].performed += OnInteract;
            
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

            _inputActions["Charge"].started += OnChargeStart;
            _inputActions["Charge"].canceled += OnChargeEnd;
            
            _inputActions["Fire"].started += OnFireStart;
            _inputActions["Fire"].canceled += OnFireEnd;

            _inputActions["ItemSlot1"].started += OnItemSlot1Pressed;
            _inputActions["ItemSlot1"].canceled += OnItemSlot1Released;
            
            _inputActions["Accept"].performed += OnAccept;

            _inputActions["Back"].performed += OnBack;
            
            _inputActions["Pause"].performed += OnPause;
            
            _inputActions["LeftTab"].performed += OnLeftTab;
            _inputActions["RightTab"].performed += OnRightTab;
        }

        private void Unsubscribe()
        {
            _inputActions["Any"].performed -= OnAnyInput;
            
            _inputActions["Interact"].performed -= OnInteract;
            
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
            
            _inputActions["Charge"].started -= OnChargeStart;
            _inputActions["Charge"].canceled -= OnChargeEnd;
            
            _inputActions["Fire"].started -= OnFireStart;
            _inputActions["Fire"].canceled -= OnFireEnd;
            
            _inputActions["ItemSlot1"].started -= OnItemSlot1Pressed;
            _inputActions["ItemSlot1"].canceled -= OnItemSlot1Released;

            _inputActions["Accept"].performed -= OnAccept;
            
            _inputActions["Back"].performed -= OnBack;
            
            _inputActions["Pause"].performed -= OnPause;
            
            _inputActions["LeftTab"].performed -= OnLeftTab;
            _inputActions["RightTab"].performed -= OnRightTab;
        }

        private void OnAnyInput(InputAction.CallbackContext context)
        {
            AnyInput?.Invoke();
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            InteractInput?.Invoke();
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
        
        private void OnChargeStart(InputAction.CallbackContext context)
        {
            ChargeInput?.Invoke(true);
        }

        private void OnChargeEnd(InputAction.CallbackContext context)
        {
            ChargeInput?.Invoke(false);
        }
        
        private void OnFireStart(InputAction.CallbackContext context)
        {
            FireInput?.Invoke(true);
        }

        private void OnFireEnd(InputAction.CallbackContext context)
        {
            FireInput?.Invoke(false);
        }
        
        //UI
        private void OnAccept(InputAction.CallbackContext context)
        {
            AcceptInput?.Invoke();
        }
        
        private void OnBack(InputAction.CallbackContext context)
        {
            BackInput?.Invoke();
        }
        
        private void OnPause(InputAction.CallbackContext context)
        {
            PauseInput?.Invoke();
        }

        private void OnLeftTab(InputAction.CallbackContext context)
        {
            TabInput?.Invoke(false);
        }
        
        private void OnRightTab(InputAction.CallbackContext context)
        {
            TabInput?.Invoke(true);
        }
    }
}
