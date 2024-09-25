using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

namespace RandomIsleser
{
    public class PlayerController : Entity
    {
        [SerializeField] private PlayerModel _model;
        public PlayerModel PlayerModel => _model;
        
        //Variables
        private Vector3 _movementInput;
        private Vector3 _movement;
        private bool _targetHeld;
        
        public Vector3 LastMoveDirection { get; private set; }
        
        //States
        public BasePlayerState CurrentState { get; private set; }
        public readonly DefaultPlayerState DefaultState = new DefaultPlayerState();
        public readonly RollPlayerState RollState = new RollPlayerState();
        
        //Cameras
        [SerializeField] private CinemachineFreeLook _mainCamera;
        
        //Cached components
        private CharacterController _characterController;
        private Animator _animator;
        [SerializeField] private Transform _cameraTransform;
        
        public Animator Animator => _animator;
        
        
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            
            InputManager.MoveInput += SetMoveInput;
            InputManager.RollInput += SetRollInput;
            InputManager.TargetInput += SetTargetInput;
            InputManager.HammerAttackInput += HammerAttackPressed;
            
            CurrentState = DefaultState;
        }

        private void OnDestroy()
        {
            InputManager.MoveInput -= SetMoveInput;
            InputManager.RollInput -= SetRollInput;
            InputManager.TargetInput -= SetTargetInput;
            InputManager.HammerAttackInput -= HammerAttackPressed;
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

            _mainCamera.m_RecenterToTargetHeading.m_enabled = _targetHeld;
        }

        private void SetMoveInput(Vector2 movement)
        {
            _movementInput.x = movement.x;
            _movementInput.z = movement.y;

            _movement.x = _movementInput.x;
            _movement.z = _movementInput.z;
            
            if (_movementInput != default)
                LastMoveDirection = RotateVectorToCamera(_movementInput);
        }

        private Vector3 RotateVectorToCamera(Vector3 input)
        {
            return Quaternion.AngleAxis(_cameraTransform.rotation.eulerAngles.y, Vector3.up) * input;
        }

        private void SetRollInput()
        {
            CurrentState.Roll(this);
        }

        private void SetTargetInput(bool isHeld)
        {
            _targetHeld = isHeld;
        }

        private void HammerAttackPressed()
        {
            CurrentState.HammerAttack(this);
        }

        public void Move()
        {
            if (!_characterController.isGrounded)
                _movement.y += Physics.gravity.y * Time.deltaTime;
            else
                _movement.y = -1;
            
            var movement = RotateVectorToCamera(_movementInput);
            _movement.x = movement.x;
            _movement.z = movement.z;
            
            _characterController.Move(_model.MovementSpeed * Time.deltaTime * _movement);
            SnapToInputDirection(movement);
        }

        public void SnapToInputDirection(Vector3 direction)
        {
            if (direction != default)
                transform.forward = direction.normalized;
        }

        public void Roll(Vector3 rollDirection)
        {
            if (!_characterController.isGrounded)
                rollDirection.y += Physics.gravity.y * Time.deltaTime;
            else
                rollDirection.y = -1;
            
            _characterController.Move(_model.RollSpeed * Time.deltaTime * rollDirection);
        }

        public void Attack()
        {
            _animator.ResetTrigger(Animations.HammerAttackHash);
            _animator.SetTrigger(Animations.HammerAttackHash);
        }
    }
}
