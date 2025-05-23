using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace RandomIsleser
{
    public class PlayerController : Entity
    {
        [SerializeField] private PlayerModel _model;
        public PlayerModel PlayerModel => _model;
        
        //Variables
        private Interactable _currentInteractable;
        private bool _hasInteractable;
        
        public Collider IncomingCameraBounds;
        public bool HasInteractable => _hasInteractable;

        private PickupModel _lastPickup;
        
        //Cached Input
        private Vector3 _movementInput;
        private Vector2 _cameraInput;
        private Vector3 _cycloneCameraInput = new Vector3();
        private bool _targetHeld;
        
        public Vector3 MovementInput => _movementInput;
        public Vector3 LastInputDirection { get; private set; }

        //Movement
        private Vector3 _movement;
        
        private bool _canMove = true;
        private bool _canRotate = true;
        private bool _isGrounded = false;

        private bool _isInWater;
        private float _heightRelativeToWater;

        private float _stateSpeedMultiplier = 1;
        private float _stateRotationMultiplier = 1;

        private Vector3 _grapplePoint;

        public bool IsGrounded => _isGrounded;
        public bool ChargeHeld { get; private set; }
        public bool FireHeld { get; private set; }
        public Vector3 LastMoveDirection { get; private set; }
        
        //Properties
        public bool CanAttack => true;
        public float HeightRelativeToWater => _heightRelativeToWater;
        public bool TargetHeld => _targetHeld;
        public bool CanMove => _canMove;
        public bool CanRotate => !_targetHeld && _canRotate;
        public bool CanAim => _isGrounded && CanAttack;
        public bool IsAttacking => CurrentState is AttackCombatState;
        public bool CanUseItem => CurrentState is DefaultMovementState or AimCombatState or CycloneCombatState or FishingState;
        public bool StrafeMovement => _targetHeld || CurrentState is AimCombatState;

        public Vector3 AimPosition => _aimCamera.position;
        public Vector3 AimDirection => _aimCamera.forward;

        [NonSerialized] public Transform StateChangeCause;
        
        //States
        public BasePlayerState CurrentState { get; private set; }
        public BasePlayerState DefaultState => _defaultMovementState;
        
        private readonly DefaultMovementState _defaultMovementState = new DefaultMovementState();
        private readonly RollMovementState _rollMovementState = new RollMovementState();
        private readonly SwimMovementState _swimMovementState = new SwimMovementState();
        private readonly OnShipMovementState _onShipMovementState = new OnShipMovementState();
        private readonly LadderMovementState _ladderMovementState = new LadderMovementState();
            
        private readonly AimCombatState _aimCombatState = new AimCombatState();
        private readonly AttackCombatState _attackCombatState = new AttackCombatState();
        
        private readonly CastRodMovementState _castRodMovementState = new CastRodMovementState();
        private readonly RodGrappleMovementState _rodGrappleMovementState = new RodGrappleMovementState();
        private readonly GrappleMovementState _grappleMovementState = new GrappleMovementState();
        private readonly CycloneCombatState _cycloneCombatState = new CycloneCombatState();
        private readonly SolarPanelCombatState _solarPanelCombatState = new SolarPanelCombatState();

        private readonly ItemGetState _itemGetState = new ItemGetState();
        private readonly FishingState _fishingState = new FishingState();
        private readonly NullState _nullState = new NullState();
        
        //Weapons
        public EquippableController CurrentlyEquippedItem;
        public EquippableController Slot1Item;
        
        [SerializeField] private EquippableController _mainWeapon;
        [SerializeField] private FishingRodController _fishingRodController;
        [SerializeField] private CycloneJarController _cycloneJarController;
        [SerializeField] private SolarPanelController _solarPanelController;

        private Dictionary<Equippables, EquippableController> _equippableLookup;

        public EquippableController MainWeapon => _mainWeapon;
        
        //Cameras
        [Header("Cameras")]
        private Camera _mainCamera;
        private CinemachineFreeLook _followCamera;
        private Transform _aimCamera;

        private bool _cameraBeingRecentered;
        
        //Cached components
        [Header("Cached Components")]
        private Transform _cameraTransform;
        private CharacterController _characterController;
        private Rigidbody _rigidbody;
        private CapsuleCollider _capsuleCollider;

        public Transform MainCameraTransform => _cameraTransform;
        public CharacterController CharacterController => _characterController;

        [SerializeField] private Transform _cameraFollowTarget;
        public Transform CameraFollowTarget => _cameraFollowTarget;

        [SerializeField] private Transform _pickupHoldPoint;

        public Transform PickupHoldPoint => _pickupHoldPoint;
        
        //IK and Animations
        [SerializeField] private Animator _equipmentAnimator;
        [SerializeField] private Animator _locomotionAnimator;

        [SerializeField] private TwoBoneIKConstraint _leftHand;
        [SerializeField] private TwoBoneIKConstraint _rightHand;

        [SerializeField] private IKTargetFollow _leftHandFollowIK;
        [SerializeField] private IKTargetFollow _rightHandFollowIK;

        public Animator EquipmentAnimator => _equipmentAnimator;
        public Animator LocomotionAnimator => _locomotionAnimator;
        
        
        public static PlayerController Instance { get; private set; }
        
        #region Debug

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 500, 20), $"Current State: {CurrentState}");
            GUI.Label(new Rect(10, 50, 500, 20), $"Current Speed: {_movement.y}");
            
            if (GUI.Button(new Rect(10, 100, 200, 25), "Load Test Chunk"))
                SceneTransitionManager.LoadChunks(new List<string> {"TestChunk"});
            
            if (GUI.Button(new Rect(10, 150, 200, 25), "Save Game"))
                RuntimeSaveManager.Instance.SaveGame();

            if (GUI.Button(new Rect(10, 200, 200, 25), "Unlock all"))
            {
                foreach (Unlockables unlockable in Enum.GetValues(typeof(Unlockables)))
                {
                    UnlockItem(unlockable);
                }
            }
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
        
        public void SetState(PlayerStates newState, Transform cause)
        {
            StateChangeCause = cause;
            SetState(GetState(newState));
        }
        
        public void SetState(BasePlayerState newState)
        {
            if (newState == CurrentState)
                return;
            
            CurrentState.OnExitState(this, newState);
            newState.OnEnterState(this, CurrentState);
            CurrentState = newState;
        }

        public void AddCurrency(int value)
        {
            int startAmount = RuntimeSaveManager.Instance.CurrentSaveSlot.InventoryData.Currency;
            int newAmount = RuntimeSaveManager.Instance.CurrentSaveSlot.InventoryData.AddCurrency(value);
            Services.Instance.UIManager.UpdateCurrency(newAmount - startAmount);
        }

        public void UnlockItem(Unlockables unlockable)
        {
            RuntimeSaveManager.Instance.CurrentSaveSlot.InventoryData.SetItemUnlocked(unlockable, true);
        }

        public void EquipItem(EquippableController equippable)
        {
            if (CurrentlyEquippedItem == equippable)
                return;
            
            if (CurrentlyEquippedItem != null)
                CurrentlyEquippedItem.OnUnequip();
            CurrentlyEquippedItem = equippable;
            CurrentlyEquippedItem.OnEquip();
            _equipmentAnimator.SetBool(Animations.WeaponEquippedHash, true);
            _equipmentAnimator.SetInteger(Animations.WeaponIndexHash, CurrentlyEquippedItem.ItemIndex);
        }

        public void UnequipItem()
        {
            if (CurrentlyEquippedItem == null)
                return;
            
            CurrentlyEquippedItem.OnUnequip();
            CurrentlyEquippedItem = null;
            _equipmentAnimator.SetBool(Animations.WeaponEquippedHash, false);
            _locomotionAnimator.SetBool(Animations.WeaponEquippedHash, false);
        }

        public void SetGrapplePoint(Vector3 grapplePoint)
        {
            _grapplePoint = grapplePoint;
        }

        public void SetSolarPanelInput()
        {
            _equipmentAnimator.SetFloat(Animations.SolarPanelHorizontalHash, _cameraInput.x); //consider squaring the values if it's too sensitive
            _equipmentAnimator.SetFloat(Animations.SolarPanelVerticalHash, _cameraInput.y);
        }

        public void SetLeftHandIK(bool enable, Transform target = null)
        {
            _leftHandFollowIK.SetTarget(target);
            if (!enable)
            {
                _leftHand.weight = 0;
                return;
            }
            _leftHand.weight = 1;
        }
        
        public void SetRightHandIK(bool enable, Transform target = null)
        {
            _rightHandFollowIK.SetTarget(target);
            if (!enable)
            {
                _rightHand.weight = 0;
                return;
            }
            _rightHand.weight = 1;
        }

        public void SetRigidbodyMovement(bool enable)
        {
            _characterController.enabled = !enable;
            _capsuleCollider.enabled = enable;
            _rigidbody.isKinematic = !enable;
            _rigidbody.useGravity = enable;
        }

        public void SetControllerAndRigidbodyVelocitiesEqual(bool useRigidbody)
        {
            if (useRigidbody)
                _movement = _rigidbody.velocity;
            else
                _rigidbody.velocity = _movement;
        }

        public void ExitLadder()
        {
            if (CurrentState is LadderMovementState)
                _locomotionAnimator.SetTrigger(Animations.ExitLadderHash);
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
                case PlayerStates.OnShipMove:
                    return _onShipMovementState;
                case PlayerStates.GrappleMove:
                    return _grappleMovementState;
                case PlayerStates.LadderMove:
                    return _ladderMovementState;
                case PlayerStates.AimCombat:
                    return _aimCombatState;
                case PlayerStates.AttackCombat:
                    return _attackCombatState;
                case PlayerStates.CastRodMove:
                    return _castRodMovementState;
                case PlayerStates.RodGrappleMove:
                    return _rodGrappleMovementState;
                case PlayerStates.CycloneCombat:
                    return _cycloneCombatState;
                case PlayerStates.SolarPanelCombat:
                    return _solarPanelCombatState;
                case PlayerStates.ItemGetState:
                    return _itemGetState;
                case PlayerStates.FishingState:
                    return _fishingState;
                case PlayerStates.NullState:
                    return _nullState;
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
            _rigidbody = GetComponent<Rigidbody>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            
            SubscribeControls();

            _mainCamera = CameraManager.Instance.MainCamera;
            _followCamera = CameraManager.Instance.FollowCamera;
            _cameraTransform = CameraManager.Instance.MainCamera.transform;
            _aimCamera = CameraManager.Instance.AimCamera.transform;

            CurrentState = new DefaultMovementState();
            
            _equippableLookup = new Dictionary<Equippables, EquippableController>()
            {
                {Equippables.FishingRod, _fishingRodController},
                {Equippables.CycloneJar, _cycloneJarController},
                {Equippables.SolarPanel, _solarPanelController}
            };
        }

        private void OnDestroy()
        {
            UnsubscribeControls();
        }

        public void SubscribeControls()
        {
            InputManager.InteractInput += SetInteractInput;
            InputManager.MoveInput += SetMoveInput;
            InputManager.RollInput += SetRollInput;
            InputManager.CameraInput += SetCameraInput;
            InputManager.TargetInput += SetTargetInput;
            InputManager.HammerAttackInput += HammerAttackPressed;
            InputManager.ItemSlot1Input += ItemSlot1Pressed;
            InputManager.BackInput += SetBackInput;
            InputManager.ChargeInput += SetChargeInput;
            InputManager.FireInput += SetFireInput;
        }

        public void UnsubscribeControls()
        {
            InputManager.InteractInput -= SetInteractInput;
            InputManager.MoveInput -= SetMoveInput;
            InputManager.RollInput -= SetRollInput;
            InputManager.CameraInput -= SetCameraInput;
            InputManager.TargetInput -= SetTargetInput;
            InputManager.HammerAttackInput -= HammerAttackPressed;
            InputManager.ItemSlot1Input -= ItemSlot1Pressed;
            InputManager.BackInput -= SetBackInput;
            InputManager.ChargeInput -= SetChargeInput;
            InputManager.FireInput -= SetFireInput;

            ClearInputCache();
        }

        private void ClearInputCache()
        {
            _movementInput = Vector3.zero;
            _cameraInput = Vector3.zero;
            _cycloneCameraInput = Vector3.zero;
            _targetHeld = false;
        }
        
        #endregion

        private void Update()
        {
            if (CurrentState is not SwimMovementState and not OnShipMovementState and not LadderMovementState && GetHeightRelativeToWater() < 0)
                SetState(PlayerStates.SwimMove);
            
            CurrentState.OnUpdateState(this);
            
            if (!_cameraBeingRecentered)
                _followCamera.m_RecenterToTargetHeading.m_enabled = _targetHeld || CurrentState is AimCombatState;
        }
        
        private void OnAnimatorMove()
        {
            if (CurrentState is AttackCombatState)
                transform.rotation = _equipmentAnimator.rootRotation;
        }
        
        #region Utils
        public Vector3 RotateVectorToCamera(Vector3 input)
        {
            return Quaternion.AngleAxis(_cameraTransform.rotation.eulerAngles.y, Vector3.up) * input;
        }

        private float GetHeightRelativeToWater()
        {
            _heightRelativeToWater = transform.position.y + _characterController.center.y - OceanController.Instance.GetHeightAtPosition(transform.position);

            return _heightRelativeToWater;
        }
        
        #endregion

        #region Interactions
        public void SetInteractable(Interactable interactable)
        {
            if (_currentInteractable == interactable) 
                return;

            _hasInteractable = interactable != null;
            _currentInteractable = interactable;
        }

        public void UnsetInteractable(Interactable interactable)
        {
            if (_currentInteractable != interactable) 
                return;

            _hasInteractable = false;
            _currentInteractable = null;
        }
        
        public void Interact()
        {
            _currentInteractable.Interact();
        }
        
        public async Task MoveToTargetPosition(Vector3 target, float giveUpTime = 10, float speedMultiplier = 1)
        {
            float timeBegan = Time.time;
            UnsubscribeControls();
            _locomotionAnimator.SetFloat(Animations.MovementSpeedHash, 0.5f);

            while (Vector3.SqrMagnitude(transform.position - target) > 0.01f)
            {
                var direction = target - transform.position;
                _characterController.Move(Time.deltaTime * speedMultiplier * direction.normalized);
                RotatePlayer(direction);
                await Task.Yield();

                if (giveUpTime > 0 && Time.time - timeBegan > giveUpTime)
                    break;
            }
            _locomotionAnimator.SetFloat(Animations.MovementSpeedHash, 0);
            SubscribeControls();
        }

        public async Task MoveThroughDoorToTargetPosition(Vector3 target)
        {
            SetState(PlayerStates.NullState);
            CameraManager.Instance.SetDoorCamera(true);
            await MoveToTargetPosition(target, 5, 2.5f);
            CameraManager.Instance.SetBounds(IncomingCameraBounds);
            IncomingCameraBounds = null;
            CameraManager.Instance.SetDoorCamera(false);
            SetState(PlayerStates.DefaultMove);
        }

        public async Task OpenChest(ChestController controller, ChestModel model)
        {
	        await MoveToTargetPosition(controller.ChestOpenPoint.position, 5, 2f);
	        //Play chest music
	        //Play chest animation
	        model.Reward.UnlockReward();
            model.HasBeenOpened = true;
        }
        
        public void BoardShip()
        {
	        UnsubscribeControls();
	        BoatController.Instance.SubscribeControls();
	        transform.parent = BoatController.Instance.transform;
	        CameraManager.Instance.SetBoatCamera(true);
        }

        public void DisembarkShip()
        {
	        SubscribeControls();
	        BoatController.Instance.UnsubscribeControls();
	        transform.parent = null;
	        CameraManager.Instance.SetBoatCamera(false);
        }

        public void NewItemGet(PickupModel pickup)
        {
            _lastPickup = pickup;
            SetState(PlayerStates.ItemGetState);
        }

        public void ShowItemGetUI()
        {
            Services.Instance.UIManager.ShowItemGetUI(_lastPickup);
        }

        public bool CloseItemGetUI()
        {
            return Services.Instance.UIManager.CloseItemGetUI();
        }
        
        #endregion
        
		#region Movement
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
            
            _locomotionAnimator.SetFloat(Animations.MovementSpeedHash, movement.magnitude / _model.MovementSpeed);
            if (StrafeMovement)
                _locomotionAnimator.SetFloat(Animations.HorizontalMovementSpeedHash, (_movementInput.x + 1) / 2);
            else
                _locomotionAnimator.SetFloat(Animations.HorizontalMovementSpeedHash, 0.5f);
            
            _movement.x = movement.x;
            _movement.z = movement.z;
            
            _characterController.Move(Time.deltaTime * _movement);
            _isGrounded = _characterController.isGrounded;
        }

        public void MoveRaw(Vector3 deltaPos)
        {
            _characterController.Move(deltaPos);
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
            {
                _rigidbody.drag = _model.SwimDrag;
                _rigidbody.AddForceAtPosition(_model.BuoyancyForce * Mathf.Abs(_heightRelativeToWater) * Vector3.up, transform.position, ForceMode.Force);
            }
            else
                _rigidbody.drag = 0;
            
            var movement = Vector3.zero;
            if (_canMove)
                movement = RotateVectorToCamera(_movementInput);

            movement *= _model.SwimSpeed;
            _locomotionAnimator.SetFloat(Animations.MovementSpeedHash, movement.magnitude / _model.SwimSpeed);
            
            _movement.x = movement.x;
            _movement.z = movement.z;
            _movement.y = _rigidbody.velocity.y;

            _rigidbody.velocity = _movement;

            GetHeightRelativeToWater();
            if (_heightRelativeToWater > 0)
            {
                _movement = Vector3.zero;
                SetState(PlayerStates.DefaultMove);
            }
        }

        public void LadderMove()
        {
            if (Mathf.Abs(_movementInput.z) < 0.3f)
                return;
            
            _locomotionAnimator.SetTrigger(_movementInput.z > 0 ? Animations.AscendLadderHash : Animations.DescendLadderHash);
        }

        [SerializeField] private LayerMask _ladderLayers;

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
        
        public void RotatePlayer()
        {
	        if (CanRotate)
		        RotateTowards(RotateVectorToCamera(_movementInput));
        }

        public void RotatePlayer(Vector3 direction)
        {
	        if (CanRotate)
		        RotateTowards(direction);
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

        public void ResetUp()
        {
	        transform.up = Vector3.up;
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
        #endregion

        #region Combat
        private void TryAim()
        {
            if (CanAim)
                SetState(PlayerStates.AimCombat);
        }

        public void RecenterCamera()
        {
            _cameraBeingRecentered = true;
            _followCamera.m_YAxisRecentering.m_enabled = true;
            _followCamera.m_RecenterToTargetHeading.m_enabled = true;
            var prevYTime = _followCamera.m_YAxisRecentering.m_RecenteringTime;
            _followCamera.m_YAxisRecentering.m_RecenteringTime = 0;
            var prexXTime = _followCamera.m_RecenterToTargetHeading.m_RecenteringTime;
            _followCamera.m_RecenterToTargetHeading.m_RecenteringTime = 0;
            
            var seq = DOTween.Sequence();
            seq.AppendInterval(0.1f);
            seq.OnComplete(() =>
            {
                _followCamera.m_YAxisRecentering.m_RecenteringTime = prevYTime;
                _followCamera.m_RecenterToTargetHeading.m_RecenteringTime = prexXTime;
                _followCamera.m_YAxisRecentering.m_enabled = false;
                _followCamera.m_RecenterToTargetHeading.m_enabled = false;
                _cameraBeingRecentered = false;
            });
        }

        public void BeginAim()
        {
            var camForward = _mainCamera.transform.forward;
            camForward.y = 0;
            SnapToInputDirection(camForward);
            _followCamera.m_YAxisRecentering.m_enabled = true;
            _aimCamera.transform.localRotation = Quaternion.identity;
            CameraManager.Instance.SetAimCamera(true);
            _equipmentAnimator.SetBool(Animations.IsAimingHash, true);
        }
        
        public void EndAim()
        {
            _followCamera.m_YAxisRecentering.m_enabled = false;
            CameraManager.Instance.SetAimCamera(false);
            _equipmentAnimator.SetBool(Animations.IsAimingHash, false);
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
            
            var camEulerX = camEulerAngles.x;
            if (camEulerX > 180) camEulerX -= 360;

            if (Mathf.Abs(camEulerX) > 85)
                camEulerAngles.x = Mathf.Clamp(camEulerX, -85, 85);
            
            _aimCamera.transform.eulerAngles = camEulerAngles;

            var transEulerAngles = transform.eulerAngles;
            transEulerAngles.y += _cameraInput.x * _model.AimSpeed * Time.deltaTime;
            transform.eulerAngles = transEulerAngles;
            
            CurrentlyEquippedItem.CheckAim(_aimCamera.position, _aimCamera.forward);
        }

        public void Attack()
        {
            _equipmentAnimator.SetInteger(Animations.WeaponIndexHash, 0);
            _equipmentAnimator.ResetTrigger(Animations.HammerAttackHash);
            _locomotionAnimator.ResetTrigger(Animations.HammerAttackHash);
            _equipmentAnimator.SetTrigger(Animations.HammerAttackHash);
            _locomotionAnimator.SetTrigger(Animations.HammerAttackHash);
        }
        #endregion
        
		#region Input
        private void SetInteractInput()
        {
            CurrentState.Interact(this);
        }
        
        private void SetMoveInput(Vector2 movement)
        {
            _movementInput.x = movement.x;
            _movementInput.z = movement.y;

            _movement.x = _movementInput.x;
            _movement.z = _movementInput.z;

            if (_movementInput != Vector3.zero)
            {
                LastInputDirection = _movementInput;
                LastMoveDirection = RotateVectorToCamera(_movementInput);
            }
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
            _targetHeld = isHeld;
        }

        private void SetChargeInput(bool isHeld)
        {
            ChargeHeld = isHeld;
        }
        
        private void SetFireInput(bool isHeld)
        {
            FireHeld = isHeld;
        }

        private void HammerAttackPressed()
        {
            CurrentState.HammerAttack(this);
        }

        private void ItemSlot1Pressed(bool held)
        {
            if (!CanUseItem)
                return;
            
            EquipItem(Slot1Item);
            
            if (held && Slot1Item.IsAimable) //TODO - handle what happens if z targetting
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
        
        #region EquipmentSlots

        public void EquipItemInSlot1(Equippables item)
        {
            if (_equippableLookup.TryGetValue(item, out var controller))
            {
                Slot1Item = controller;
            }
        }
        #endregion
    }
}
