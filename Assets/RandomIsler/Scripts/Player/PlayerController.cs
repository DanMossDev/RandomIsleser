using Unity.VisualScripting;
using UnityEngine;

namespace RandomIsleser
{
    public class PlayerController : Entity
    {
        [SerializeField] private PlayerModel _model;
        
        //Variables
        private Vector3 _movementInput;
        private Vector3 _movement;
        
        //States
        public BasePlayerState CurrentState { get; private set; }
        public readonly DefaultPlayerState DefaultState = new DefaultPlayerState();
        
        //Cached components
        private CharacterController _characterController;
        [SerializeField] private Transform _cameraTransform;
        
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            
            InputManager.MoveInput += SetMoveInput;
            
            CurrentState = DefaultState;
        }

        public void SetState(BasePlayerState newState)
        {
            CurrentState.OnLeaveState(this, newState);
            newState.OnEnterState(this, CurrentState);
            CurrentState = newState;
        }

        private void FixedUpdate()
        {
            CurrentState.OnUpdateState(this);
        }

        private void SetMoveInput(Vector2 movement)
        {
            _movementInput.x = movement.x;
            _movementInput.z = movement.y;
            _movementInput = Quaternion.AngleAxis(_cameraTransform.rotation.eulerAngles.y, Vector3.up) * _movementInput;
            _movement.x = _movementInput.x;
            _movement.z = _movementInput.z;
        }

        public void Move()
        {
            if (!_characterController.isGrounded)
                _movement.y += Physics.gravity.y * Time.deltaTime;
            else
                _movement.y = -1;
            
            _characterController.Move(_model.MovementSpeed * Time.deltaTime * _movement);
            if (_movementInput != default)
                transform.forward = _movementInput.normalized;
        }
    }
}
