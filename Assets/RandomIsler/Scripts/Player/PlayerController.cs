using System;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace RandomIsleser
{
    public class PlayerController : Entity
    {
        [SerializeField] private PlayerModel _model;
        public PlayerModel PlayerModel => _model;
        
        //Variables
        private Vector3 _movementInput;
        private Vector2 _cameraInput;
        private Vector3 _movement;
        private bool _targetHeld;
        private bool _attacking = false;

        private bool _canMove = true;
        private bool _canRotate = true;

        private bool _isGrounded = false;
        private float _heightRelativeToWater;
        
        public Vector3 LastMoveDirection { get; private set; }
        
        //Properties
        public bool CanAttack => CurrentMovementState == _defaultMovementState;
        public float HeightRelativeToWater => _heightRelativeToWater;
        public bool TargetHeld => _targetHeld;
        public bool CanMove => _canMove && !_attacking && CurrentCombatState is IdleCombatState or AimCombatState;
        public bool CanRotate => !_targetHeld && _canRotate && !_attacking && CurrentCombatState is IdleCombatState;
        public bool CanAim => _isGrounded && CanAttack;
        
        //States
        public BaseMovementState CurrentMovementState { get; private set; }
        private readonly DefaultMovementState _defaultMovementState = new DefaultMovementState();
        private readonly RollMovementState _rollMovementState = new RollMovementState();
        private readonly SwimMovementState _swimMovementState = new SwimMovementState();
        
        public BaseCombatState CurrentCombatState { get; private set; }
        private readonly IdleCombatState _idleCombatState = new IdleCombatState();
        private readonly AimCombatState _aimCombatState = new AimCombatState();
        
        //Weapons
        //public Weapon CurrentWeapon
        
        
        //Cameras
        [Header("Cameras")]
        [SerializeField] private CinemachineFreeLook _mainCamera;
        [SerializeField] private GameObject _aimCamera;
        
        //Cached components
        [Header("Cached Components")]
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
                case PlayerStates.SwimMove:
                    return _swimMovementState;
            }

            return null;
        }
        
        private BaseCombatState GetCombatState(PlayerStates state)
        {
            switch (state)
            {
                case PlayerStates.IdleCombat:
                    return _idleCombatState;
                case PlayerStates.AimCombat:
                    return _aimCombatState;
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
            InputManager.CameraInput += SetCameraInput;
            InputManager.TargetInput += SetTargetInput;
            InputManager.HammerAttackInput += HammerAttackPressed;
            InputManager.AimInput += SetAimInput;
            
            CurrentMovementState = _defaultMovementState;
            CurrentCombatState = _idleCombatState;
        }

        private void OnDestroy()
        {
            InputManager.MoveInput -= SetMoveInput;
            InputManager.RollInput -= SetRollInput;
            InputManager.CameraInput -= SetCameraInput;
            InputManager.TargetInput -= SetTargetInput;
            InputManager.HammerAttackInput -= HammerAttackPressed;
        }
        
        #endregion

        private void FixedUpdate()
        {
            if (CurrentMovementState is not SwimMovementState && GetHeightRelativeToWater() < 0)
                SetState(PlayerStates.SwimMove);
            
            CurrentMovementState.OnUpdateState(this);
            CurrentCombatState.OnUpdateState(this);
            
            _mainCamera.m_RecenterToTargetHeading.m_enabled = _targetHeld || CurrentCombatState is AimCombatState;
        }
        
        //UTILS
        private Vector3 RotateVectorToCamera(Vector3 input)
        {
            return Quaternion.AngleAxis(_cameraTransform.rotation.eulerAngles.y, Vector3.up) * input;
        }

        private float GetHeightRelativeToWater()
        {
            _heightRelativeToWater = transform.position.y + 1 - OceanController.Instance.GetHeightAtPosition(transform.position);

            return _heightRelativeToWater;
        }

        //MOVEMENT
        public void Move()
        {
            if (!_isGrounded)
                _movement.y += Physics.gravity.y * Time.deltaTime;
            else
                _movement.y = -1;
            
            var movement = Vector3.zero;
            if (CanMove) 
                movement = RotateVectorToCamera(_movementInput);
            
            _movement.x = movement.x;
            _movement.z = movement.z;
            
            _characterController.Move(_model.MovementSpeed * Time.deltaTime * _movement);
            _isGrounded = _characterController.isGrounded;
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
        
        public void Swim()
        {
            if (_heightRelativeToWater < 0)
                _movement.y += _model.BuoyancyForce * Time.deltaTime * Mathf.Abs(_heightRelativeToWater);
            else
            {
                if (_movement.y > 0)
                    _movement.y = 0;
                _movement.y += Physics.gravity.y * Time.deltaTime;
            }
            
            var movement = Vector3.zero;
            if (_canMove) 
                movement = RotateVectorToCamera(_movementInput);
            
            _movement.x = movement.x;
            _movement.z = movement.z;
            
            _characterController.Move(_model.SwimSpeed * Time.deltaTime * _movement);
            _isGrounded = _characterController.isGrounded;
            GetHeightRelativeToWater();

            if (_isGrounded || _heightRelativeToWater > 1)
            {
                SetState(PlayerStates.DefaultMove);
            }
        }

        private void TryAim()
        {
            if (CanAim)
                SetState(PlayerStates.AimCombat);
        }

        public void BeginAim()
        {
            _mainCamera.m_YAxisRecentering.m_enabled = true;
            _aimCamera.transform.localRotation = Quaternion.identity;
            _aimCamera.SetActive(true);
            _animator.SetBool(Animations.IsAimingHash, true);
            //TODO - Activate aim UI
        }
        
        public void EndAim()
        {
            _mainCamera.m_YAxisRecentering.m_enabled = false;
            _aimCamera.SetActive(false);
            _animator.SetBool(Animations.IsAimingHash, false);
            SetCanRotate(false);
            var seq = DOTween.Sequence();
            seq.AppendInterval(0.5f);
            seq.OnComplete(() =>
            {
                SetCanRotate(true);
            });
            //TODO - Disable aim UI
        }

        public void Aim()
        {
            var camEulerAngles = _aimCamera.transform.eulerAngles;
            camEulerAngles.x -= _cameraInput.y * _model.AimSpeed * Time.deltaTime;
            _aimCamera.transform.eulerAngles = camEulerAngles;

            var transEulerAngles = transform.eulerAngles;
            transEulerAngles.y += _cameraInput.x * _model.AimSpeed * Time.deltaTime;
            transform.eulerAngles = transEulerAngles;
        }

        public void RotatePlayer()
        {
            if (CanRotate)
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

        private void SetCameraInput(Vector2 input)
        {
            _cameraInput = input;
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

        private void SetAimInput(bool isHeld)
        {
            //_aimHeld = isHeld; 

            if (isHeld)
                TryAim();
            else if (CurrentCombatState is AimCombatState)
                SetState(PlayerStates.IdleCombat);
        }
        
        #endregion
    }
}
