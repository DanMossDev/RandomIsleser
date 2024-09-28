using System.Collections.Generic;
using Cinemachine;
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

        private bool _isGrounded = false;
        
        public Vector3 LastMoveDirection { get; private set; }
        
        //Properties
        public bool CanAttack => CurrentMovementState == _defaultMovementState;
        
        //States
        public BaseMovementState CurrentMovementState { get; private set; }
        private readonly DefaultMovementState _defaultMovementState = new DefaultMovementState();
        private readonly RollMovementState _rollMovementState = new RollMovementState();
        
        public BaseCombatState CurrentCombatState { get; private set; }
        private readonly DefaultCombatState _defaultCombatState = new DefaultCombatState();
        
        //Cameras
        [SerializeField] private CinemachineFreeLook _mainCamera;
        
        //Cached components
        private CharacterController _characterController;
        private Animator _animator;
        [SerializeField] private Transform _cameraTransform;
        
        public Animator Animator => _animator;
        
        public static PlayerController Instance { get; private set; }
        
        #region Setters
        public void SetCanMove(bool value) => _canMove = value;
        public void SetCanRotate(bool value) => _canRotate = value;
        public void SetAttacking(bool value) => _attacking = value;
        
        public void SetState(PlayerStates newState)
        {
            switch (newState.GetStateType())
            {
                case PlayerStateTypes.Movement:
                    SetState(GetMovementState(newState));
                    return;
                case PlayerStateTypes.Combat:
                    SetState(GetCombatState(newState));
                    return;
            }
        }

        private void SetState(BaseMovementState newState)
        {
            CurrentMovementState.OnLeaveState(this, newState);
            newState.OnEnterState(this, CurrentMovementState);
            CurrentMovementState = newState;
        }
        
        private void SetState(BaseCombatState newState)
        {
            CurrentCombatState.OnLeaveState(this, newState);
            newState.OnEnterState(this, CurrentCombatState);
            CurrentCombatState = newState;
        }
        #endregion
        
        #region Getters
        private BaseMovementState GetMovementState(PlayerStates state)
        {
            switch (state)
            {
                case PlayerStates.DefaultMove:
                    return _defaultMovementState;
                case PlayerStates.RollMove:
                    return _rollMovementState;
            }

            return null;
        }
        
        private BaseCombatState GetCombatState(PlayerStates state)
        {
            switch (state)
            {
                case PlayerStates.DefaultCombat:
                    return _defaultCombatState;
            }

            return null;
        }
        
        #endregion

        #region Initialisation
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
            
            CurrentMovementState = _defaultMovementState;
            CurrentCombatState = _defaultCombatState;
        }

        private void OnDestroy()
        {
            InputManager.MoveInput -= SetMoveInput;
            InputManager.RollInput -= SetRollInput;
            InputManager.TargetInput -= SetTargetInput;
            InputManager.HammerAttackInput -= HammerAttackPressed;
        }
        
        #endregion

        private void FixedUpdate()
        {
            CurrentMovementState.OnUpdateState(this);
            
            _mainCamera.m_RecenterToTargetHeading.m_enabled = _targetHeld;
        }
        
        private Vector3 RotateVectorToCamera(Vector3 input)
        {
            return Quaternion.AngleAxis(_cameraTransform.rotation.eulerAngles.y, Vector3.up) * input;
        }

        public void Move()
        {
            if (!_isGrounded)
                _movement.y += Physics.gravity.y * Time.deltaTime;
            else
                _movement.y = -1;
            
            var movement = Vector3.zero;
            if (_canMove) 
                movement = RotateVectorToCamera(_movementInput);
            
            _movement.x = movement.x;
            _movement.z = movement.z;
            
            _characterController.Move(_model.MovementSpeed * Time.deltaTime * _movement);
            _isGrounded = _characterController.isGrounded;
        }

        public void RotatePlayer()
        {
            if (!_targetHeld && !_attacking && _canRotate)
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
            if (!_isGrounded)
                rollDirection.y += Physics.gravity.y * Time.deltaTime;
            else
                rollDirection.y = -1;
            
            _characterController.Move(_model.RollSpeed * Time.deltaTime * rollDirection);
            _isGrounded = _characterController.isGrounded;
        }

        public void Attack()
        {
            _animator.ResetTrigger(Animations.HammerAttackHash);
            _animator.SetTrigger(Animations.HammerAttackHash);
        }
        
        
        #region Input
        private void SetMoveInput(Vector2 movement)
        {
            _movementInput.x = movement.x;
            _movementInput.z = movement.y;

            _movement.x = _movementInput.x;
            _movement.z = _movementInput.z;
            
            if (_movementInput != Vector3.zero)
                LastMoveDirection = RotateVectorToCamera(_movementInput);
        }

        private void SetRollInput()
        {
            CurrentMovementState.Roll(this);
        }

        private void SetTargetInput(bool isHeld)
        {
            _targetHeld = isHeld;
        }

        private void HammerAttackPressed()
        {
            CurrentCombatState.HammerAttack(this);
        }
        
        #endregion
    }
}
