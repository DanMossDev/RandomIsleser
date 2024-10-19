using MossUtils;
using UnityEngine;

namespace RandomIsleser
{
    public class BoatController : MonoSingleton<BoatController>, Interactable
    {
        [SerializeField] private Transform _playerSeatedPosition;
        
        private Vector3 _movementInput;
        private Vector3 _cameraInput;

        private bool _targetHeld;
        
        public float turnSpeed = 10;
        public float moveSpeed = 10;

        private PlayerController _playerController;
        private Rigidbody _rigidbody;
        private BuoyancyController _buoyancyController;
        
#region Initialisation
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _buoyancyController = GetComponent<BuoyancyController>();
            _playerController = PlayerController.Instance;
        }

        public void SubscribeControls()
        {
            InputManager.InteractInput += SetInteractInput;
            InputManager.MoveInput += SetMoveInput;
            InputManager.CameraInput += SetCameraInput;
            InputManager.TargetInput += SetTargetInput;
            InputManager.ItemSlot1Input += ItemSlot1Pressed;
            InputManager.BackInput += SetBackInput;
        }

        public void UnsubscribeControls()
        {
            InputManager.InteractInput -= SetInteractInput;
            InputManager.MoveInput -= SetMoveInput;
            InputManager.CameraInput -= SetCameraInput;
            InputManager.TargetInput -= SetTargetInput;
            InputManager.ItemSlot1Input -= ItemSlot1Pressed;
            InputManager.BackInput -= SetBackInput;

            ClearInputCache();
        }

        private void ClearInputCache()
        {
            _movementInput = Vector3.zero;
            _cameraInput = Vector3.zero;
            _targetHeld = false;
        }
#endregion
#region Input
        private void SetInteractInput()
        {
            //Interact();
        }
        
        private void SetMoveInput(Vector2 input)
        {
            _movementInput.x = input.x;
            _movementInput.z = input.y;
        }
        
        private void SetCameraInput(Vector2 input)
        {
            _cameraInput = input;
        }
        
        private void SetTargetInput(bool isHeld)
        {
            _targetHeld = isHeld;
        }

        private void ItemSlot1Pressed(bool held)
        {
            // if (held)
            //     UseItem();
            // else
            //     ReleaseItem();
        }
        
        private void SetBackInput()
        {
            StopDriving();
        }
#endregion

        private void FixedUpdate()
        {
            if (_movementInput == Vector3.zero)
                return;
            
            _rigidbody.AddForce(_movementInput.z * moveSpeed * transform.forward, ForceMode.Force);
            _rigidbody.AddTorque(_movementInput.x * turnSpeed * Vector3.up, ForceMode.Acceleration);
        }

        public void Interact()
        {
            _playerController.SetState(PlayerStates.OnShipMove);
        }

        private void StopDriving()
        {
            _playerController.SetState(PlayerStates.DefaultMove);
        }
    }
}
