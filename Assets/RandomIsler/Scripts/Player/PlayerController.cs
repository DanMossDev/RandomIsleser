using System;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace RandomIsleser
{
    public class PlayerController : Entity
    {
        [SerializeField] private PlayerModel _model;
        public PlayerModel PlayerModel => _model;
        
        //Variables
        private Vector3 _movementInput;
        private Vector2 _cameraInput;
        private Vector3 _cycloneCameraInput = new Vector3();

        private Vector3 _movement;

        private bool _targetHeld;

        private bool _canMove = true;
        private bool _canRotate = true;

        private bool _isGrounded = false;
        private float _heightRelativeToWater;

        private float _stateSpeedMultiplier = 1;
        private float _stateRotationMultiplier = 1;

        private Vector3 _grapplePoint;

        public bool SuctionHeld { get; private set; }
        public bool BlowHeld { get; private set; }
        public Vector3 LastMoveDirection { get; private set; }
        
        //Properties
        public bool CanAttack => true;
        public float HeightRelativeToWater => _heightRelativeToWater;
        public bool TargetHeld => _targetHeld;
        public bool CanMove => _canMove;
        public bool CanRotate => !_targetHeld && _canRotate;
        public bool CanAim => _isGrounded && CanAttack;
        public bool IsAttacking => CurrentState is AttackCombatState;

        public Vector3 AimDirection => _aimCamera.transform.forward;
        
        //States
        public BasePlayerState CurrentState { get; private set; }
        private readonly DefaultMovementState _defaultMovementState = new DefaultMovementState();
        private readonly RollMovementState _rollMovementState = new RollMovementState();
        private readonly SwimMovementState _swimMovementState = new SwimMovementState();
        
        private readonly AimCombatState _aimCombatState = new AimCombatState();
        private readonly AttackCombatState _attackCombatState = new AttackCombatState();
        
        private readonly CastRodMovementState _castRodMovementState = new CastRodMovementState();
        private readonly RodGrappleMovementState _rodGrappleMovementState = new RodGrappleMovementState();
        private readonly GrappleMovementState _grappleMovementState = new GrappleMovementState();

        private readonly CycloneCombatState _cycloneCombatState = new CycloneCombatState();
        
        
        //Weapons
        public EquippableController CurrentlyEquippedItem;
        public EquippableController Slot1Item;
        
        [SerializeField] private EquippableController _mainWeapon;
        [SerializeField] private FishingRodController _fishingRodController;
        [SerializeField] private CycloneJarController _cycloneJarController;

        public EquippableController MainWeapon => _mainWeapon;
        
        //Cameras
        [Header("Cameras")]
        [SerializeField] private CinemachineFreeLook _mainCamera;
        [SerializeField] private GameObject _aimCamera;
        [SerializeField] private GameObject _cycloneCamera;
        
        public GameObject CycloneCamera => _cycloneCamera;
        
        //Cached components
        [Header("Cached Components")]
        [SerializeField] private Transform _cameraTransform;

        public Transform MainCameraTransform => _cameraTransform;
        
        private CharacterController _characterController;
        private Animator _animator;

        public Animator Animator => _animator;
        
        
        public static PlayerController Instance { get; private set; }
        
        #region Debug

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 500, 20), $"Current State: {CurrentState}");
            GUI.Label(new Rect(10, 50, 500, 20), $"Current Speed: {_movement.y}");
        }
        #endregion
        
        #region Setters
        public void SetCanMove(bool value) => _canMove = value;
        public void SetCanRotate(bool value) => _canRotate = value;

        public void SetStateSpeedMultiplier(float value) => _stateSpeedMultiplier = value;
        public void SetStateRotationMultiplier(float value) => _stateRotationMultiplier = value;
        
        public void SetState(PlayerStates newState)
        {
            SetState(GetState(newState));
        }
        
        private void SetState(BasePlayerState newState)
        {
            CurrentState.OnExitState(this, newState);
            newState.OnEnterState(this, CurrentState);
            CurrentState = newState;
        }

        public void EquipItem(EquippableController equippable)
        {
            if (CurrentlyEquippedItem == equippable)
                return;
            
            if (CurrentlyEquippedItem != null)
                CurrentlyEquippedItem.OnUnequip();
            CurrentlyEquippedItem = equippable;
            if (CurrentlyEquippedItem != null)
            {
                CurrentlyEquippedItem.OnEquip();
                _animator.SetBool(Animations.WeaponEquippedHash, true);
                _animator.SetInteger(Animations.WeaponIndexHash, CurrentlyEquippedItem.ItemIndex);
            }
            else _animator.SetBool(Animations.WeaponEquippedHash, false);
        }

        public void SetGrapplePoint(Vector3 grapplePoint)
        {
            _grapplePoint = grapplePoint;
        }
        
        #endregion
        
        #region Getters
        private BasePlayerState GetState(PlayerStates state)
        {
            switch (state)
            {
                case PlayerStates.DefaultMove:
                    return _defaultMovementState;
                case PlayerStates.RollMove:
                    return _rollMovementState;
                case PlayerStates.SwimMove:
                    return _swimMovementState;
                case PlayerStates.GrappleMove:
                    return _grappleMovementState;
                case PlayerStates.AimCombat:
                    return _aimCombatState;
                case PlayerStates.AttackCombat:
                    return _attackCombatState;
                case PlayerStates.CastRodMovement:
                    return _castRodMovementState;
                case PlayerStates.RodGrappleMovement:
                    return _rodGrappleMovementState;
                case PlayerStates.CycloneCombat:
                    return _cycloneCombatState;
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
            InputManager.ItemSlot1Input += ItemSlot1Pressed;
            InputManager.BackInput += SetBackInput;
            InputManager.SuctionInput += SetSuctionInput;
            InputManager.BlowInput += SetBlowInput;

            CurrentState = new DefaultMovementState();
        }

        private void OnDestroy()
        {
            InputManager.MoveInput -= SetMoveInput;
            InputManager.RollInput -= SetRollInput;
            InputManager.CameraInput -= SetCameraInput;
            InputManager.TargetInput -= SetTargetInput;
            InputManager.HammerAttackInput -= HammerAttackPressed;
            InputManager.ItemSlot1Input -= ItemSlot1Pressed;
            InputManager.BackInput -= SetBackInput;
            InputManager.SuctionInput -= SetSuctionInput;
            InputManager.BlowInput -= SetBlowInput;
        }
        
        #endregion

        private void FixedUpdate()
        {
            if (CurrentState is not SwimMovementState && GetHeightRelativeToWater() < 0)
                SetState(PlayerStates.SwimMove);
            
            CurrentState.OnUpdateState(this);
            
            _mainCamera.m_RecenterToTargetHeading.m_enabled = _targetHeld || CurrentState is AimCombatState;
        }
        
        private void OnAnimatorMove()
        {
            transform.rotation = _animator.rootRotation;
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
                _movement.y += Physics.gravity.y * _model.GravityMultiplier * Time.deltaTime;
            else if (_movement.y < 0)
                _movement.y = -1;
            
            var movement = Vector3.zero;
            if (CanMove) 
                movement = RotateVectorToCamera(_movementInput);

            movement *= _model.MovementSpeed * _stateSpeedMultiplier;
            
            _movement.x = movement.x;
            _movement.z = movement.z;
            
            _characterController.Move(Time.deltaTime * _movement);
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
            else if (_movement.y > 0) 
                _movement.y = 0;
            
            _movement.y += Physics.gravity.y * _model.GravityMultiplier * Time.deltaTime;
            
            var movement = Vector3.zero;
            if (_canMove)
                movement = RotateVectorToCamera(_movementInput);

            movement *= _model.SwimSpeed;
            
            _movement.x = movement.x;
            _movement.z = movement.z;
            
            _characterController.Move(Time.deltaTime * _movement);
            _isGrounded = _characterController.isGrounded;
            GetHeightRelativeToWater();

            if (_isGrounded || _heightRelativeToWater > 1)
            {
                SetState(PlayerStates.DefaultMove);
            }
        }

        public void JumpSetHeight(float height)
        {
            _movement.y = Mathf.Sqrt(height * 2 * -(Physics.gravity.y * _model.GravityMultiplier));
        }

        public void Grapple()
        {
            var grappleDirection = _grapplePoint - transform.position;
            if (grappleDirection.sqrMagnitude < 0.5f)
            {
                SetState(PlayerStates.DefaultMove);
                return;
            }

            _characterController.Move(Time.deltaTime * _fishingRodController.Model.GrappleSpeed * grappleDirection.normalized);
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
        }

        public void Aim()
        {
            var camEulerAngles = _aimCamera.transform.eulerAngles;
            camEulerAngles.x -= _cameraInput.y * _model.AimSpeed * Time.deltaTime;
            _aimCamera.transform.eulerAngles = camEulerAngles;

            var transEulerAngles = transform.eulerAngles;
            transEulerAngles.y += _cameraInput.x * _model.AimSpeed * Time.deltaTime;
            transform.eulerAngles = transEulerAngles;
            
            CurrentlyEquippedItem.CheckAim(_aimCamera.transform.forward);
        }

        public void RotatePlayer()
        {
            if (CanRotate)
                RotateTowards(RotateVectorToCamera(_movementInput));
        }
        
        public void CycloneRotatePlayer()
        {
            if (CanRotate)
                RotateTowards(RotateVectorToCamera(_cycloneCameraInput));
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

            transform.forward = Vector3.RotateTowards(
                transform.forward, 
                direction, 
                Time.deltaTime * _model.RotationSpeed * _stateRotationMultiplier, 
                0);
        }

        public void Attack()
        {
            _animator.SetInteger(Animations.WeaponIndexHash, 0);
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
            _cycloneCameraInput.x = input.x;
            _cycloneCameraInput.z = input.y;
        }

        private void SetRollInput()
        {
            CurrentState.Roll(this);
        }

        private void SetTargetInput(bool isHeld)
        {
            //_targetHeld = isHeld;
        }

        private void SetSuctionInput(bool isHeld)
        {
            SuctionHeld = isHeld;
        }
        
        private void SetBlowInput(bool isHeld)
        {
            BlowHeld = isHeld;
        }

        private void HammerAttackPressed()
        {
            CurrentState.HammerAttack(this);
        }

        private void ItemSlot1Pressed(bool held)
        {
            EquipItem(Slot1Item);
            
            if (Slot1Item.IsAimable) //TODO - handle what happens if z targetting
            {
                if (CurrentState.TryAim(this))
                    return;
            }
            if (held)
                CurrentState.UseItem(this);
            else
                CurrentState.ReleaseItem(this);
        }

        private void SetBackInput()
        {
            CurrentState.Back(this);
        }
        
        #endregion
    }
}
