using System;
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
        private bool _attacking = false;

        private bool _canMove = true;
        private bool _canRotate = true;
        
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
        
        public static PlayerController Instance { get; private set; }

        private void OnEnable()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        private void OnDisable()
        {
            if (Instance == this)
                Instance = null;
        }

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
        
        public void SetCanMove(bool value) => _canMove = value;
        public void SetCanRotate(bool value) => _canRotate = value;
        public void SetAttacking(bool value) => _attacking = value;

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
            
            if (_movementInput != Vector3.zero)
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

            var movement = Vector3.zero;
            if (_canMove) 
                movement = RotateVectorToCamera(_movementInput);
            
            _movement.x = movement.x;
            _movement.z = movement.z;
            
            _characterController.Move(_model.MovementSpeed * Time.deltaTime * _movement);
        }

        public void RotateCamera()
        {
            if (!_targetHeld && _canRotate)
                RotateTowards(RotateVectorToCamera(_movementInput));
        }

        public void SnapToInputDirection(Vector3 direction)
        {
            if (direction != Vector3.zero)
                transform.forward = direction.normalized;
        }

        private void RotateTowards(Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;

            float rotationMulti = 1;
            if (_attacking)
                rotationMulti = _model.AttackingRotationMultiplier;

            transform.forward = Vector3.RotateTowards(
                transform.forward, 
                direction, 
                Time.deltaTime * _model.RotationSpeed * rotationMulti, 
                0);
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
