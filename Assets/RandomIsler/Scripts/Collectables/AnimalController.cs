using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RandomIsleser
{
    public class AnimalController : Spawnable
    {
        [Space, Header("Animal")]
        [SerializeField] protected AnimalModel _animalModel;
        [SerializeField] protected CollectableSettingsModel _collectableSettingsModel;

        protected SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] protected Material[] _graphicsVariants;

        [SerializeField] protected Transform[] _patrolPoints;
        
        protected Vector3 _startPosition;
        protected Vector3 _wanderPosition;

        protected float _timeStartedWandering;

        protected float _lastExpensiveUpdateTime;
        protected float _timeStunned;
        protected float _timeSuctioned;
        
        protected Collider[] _lureCollider = new Collider[1];

        private bool _useGravity;

        protected bool _isNewItem;
        protected float _timePickedUp;

        protected int _rarityLevel;
        
        protected Animator _animator;
        protected Rigidbody _rigidbody;
        private NavMeshAgent _navMeshAgent;

        protected Lure _currentLure;

        protected AnimalState _currentState;

        #region Initialisation
        protected virtual void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

            _useGravity = _rigidbody.useGravity;
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(ObjectPoolKey))
                return;
            
            var pool = Resources.Load<MossUtils.ObjectPoolModel>("Models/ObjectPoolModel");
            pool.PrefabLookup.TryAdd(ObjectPoolKey, gameObject);
        }
        #endif

        public override void OnSpawned(SpawnPoint spawnPoint)
        {
            Initialise();
        }

        public override void OnDespawned()
        {
            _navMeshAgent.enabled = false;
        }

        protected virtual void Initialise()
        {
            ResetAllAnimParams();

            _rarityLevel = _collectableSettingsModel.GetAnimalRarityLevel();
            for (int i = 0; i < _graphicsVariants.Length; i++)
            {
                if (i == _rarityLevel)
                    _skinnedMeshRenderer.material = _graphicsVariants[i];
            }

            _lastExpensiveUpdateTime = UnityEngine.Random.Range(0, 0.2f);
            _startPosition = transform.position;

            _navMeshAgent.enabled = true;
            _navMeshAgent.speed = _animalModel.WanderSpeed;
            _navMeshAgent.angularSpeed = _animalModel.TurnSpeed;
            SetNewWanderPoint();

            _isNewItem = false;
            _timePickedUp = 0;
        }

        protected virtual void ResetAllAnimParams()
        {
            foreach (var param in _animator.parameters)
            {
                switch (param.type)
                {
                    case AnimatorControllerParameterType.Bool:
                        _animator.SetBool(param.nameHash, param.defaultBool);
                        break;
                    case AnimatorControllerParameterType.Trigger:
                        _animator.ResetTrigger(param.nameHash);
                        break;
                    case AnimatorControllerParameterType.Float:
                        _animator.SetFloat(param.nameHash, param.defaultFloat);
                        break;
                    case AnimatorControllerParameterType.Int:
                        _animator.SetInteger(param.nameHash, param.defaultInt);
                        break;
                }
            }
        }
        #endregion

        #region States
        protected void SetState(AnimalState state)
        {
            LeaveState(_currentState);
            _currentState = state;
            EnterState(_currentState);
        }

        protected virtual void LeaveState(AnimalState state)
        {
            switch (state)
            {
                case AnimalState.Idle:
                    break;
                case AnimalState.Stunned:
                    _navMeshAgent.enabled = true;
                    _rigidbody.isKinematic = true;
                    _animator.SetBool(Animations.IsStunnedHash, false);
                    break;
                case AnimalState.Suction:
                    _navMeshAgent.enabled = true;
                    _rigidbody.isKinematic = true;
                    _animator.SetBool(Animations.IsStunnedHash, false);
                    break;
                case AnimalState.Captured:
                    break;
            }
        }
        
        protected virtual void EnterState(AnimalState state)
        {
            switch (state)
            {
                case AnimalState.Idle:
                    _navMeshAgent.speed = _animalModel.WanderSpeed;
                    SetNewWanderPoint();
                    break;
                case AnimalState.Flee:
                    _navMeshAgent.speed = _animalModel.FleeSpeed;
                    break;
                case AnimalState.Stunned:
                    _navMeshAgent.enabled = false;
                    _rigidbody.isKinematic = false;
                    _timeStunned = 0;
                    _animator.SetBool(Animations.IsStunnedHash, true);
                    break;
                case AnimalState.Suction:
                    _navMeshAgent.enabled = false;
                    _rigidbody.isKinematic = false;
                    _rigidbody.useGravity = false;
                    _timeStunned = 0;
                    _animator.SetBool(Animations.IsStunnedHash, true);
                    break;
                case AnimalState.Captured:
                    _navMeshAgent.enabled = false;
                    _rigidbody.isKinematic = true;
                    transform.parent = PlayerController.Instance.PickupHoldPoint;
                    transform.localPosition = Vector3.zero;
                    _animator.SetBool(Animations.IsCaughtHash, true);

                    if (RuntimeSaveManager.Instance.LocalSaveData.UnlockAnimal(_animalModel.Species, _rarityLevel))
                    {
                        _isNewItem = true;
                        PlayerController.Instance.NewItemGet(_animalModel);
                    }
                    
                    break;
            }
        }
        #endregion

        #region Update
        protected virtual void FixedUpdate()
        {
            switch (_currentState)
            {
                case AnimalState.Idle:
                    Idle();
                    break;
                case AnimalState.Flee:
                    Flee();
                    break;
                case AnimalState.Stunned:
                case AnimalState.Suction:
                    Stunned();
                    break;
                case AnimalState.Captured:
                    Captured();
                    break;
                case AnimalState.Lured:
                    Lure();
                    break;
            }
            
            if (Time.time - _lastExpensiveUpdateTime > 0.2f)
                ExpensiveUpdate();
        }

        protected virtual void ExpensiveUpdate()
        {
            _lastExpensiveUpdateTime = Time.time;
            switch (_currentState)
            {
                case AnimalState.Idle:
                    ExpensiveIdle();
                    break;
                case AnimalState.Flee:
                    ExpensiveFlee();
                    break;
                case AnimalState.Stunned:
                case AnimalState.Suction:
                    ExpensiveStunned();
                    break;
            }
        }

        protected virtual void Idle()
        {
            if (_patrolPoints.Length > 0)
                Patrol();
            else
                Wander();
        }
        
        protected virtual void ExpensiveIdle()
        {
        }

        protected virtual void Flee()
        {
            _animator.SetFloat(Animations.MovementSpeedHash, _navMeshAgent.velocity.magnitude / _animalModel.FleeSpeed);
            CheckStopFleeing();
        }
        
        protected virtual void ExpensiveFlee()
        {
            var dir = transform.position - PlayerController.Instance.transform.position;
            dir.y = transform.position.y;
            _navMeshAgent.destination = transform.position + dir;
        }

        protected virtual void Stunned()
        {
            _timeStunned += Time.deltaTime;
            _timeSuctioned += Time.deltaTime;
            if (_timeStunned > _animalModel.StunTime)
            {
                SetState(AnimalState.Idle);
            }
            _rigidbody.useGravity = _timeSuctioned >= Time.deltaTime * 2 && _useGravity;
        }
        
        protected virtual void ExpensiveStunned()
        {
            
        }

        protected virtual void Captured()
        {
            Vector3 dirOfCamera = PlayerController.Instance.MainCameraTransform.position - transform.position;
            dirOfCamera.y = 0;
            transform.forward = dirOfCamera;

            CheckDespawn();
        }

        protected virtual void Lure()
        {
            
        }

        protected virtual void Lured(Lure lure)
        {
            _currentLure = lure;
        }

        protected virtual void CheckLured()
        {
            if (Physics.OverlapSphereNonAlloc(transform.position, _animalModel.LureRadius, _lureCollider, 1<<ProjectLayers.LureLayer, QueryTriggerInteraction.Collide) == 0)
                return;

            if (!_lureCollider[0].TryGetComponent(out Lure lure))
                return;
            
            Lured(lure);
        }

        protected virtual void CheckDespawn()
        {
            if (!_isNewItem)
            {
                if (_timePickedUp > _animalModel.PickupShowTime)
                    Despawn();
                else
                    _timePickedUp += Time.deltaTime;

                return;
            }

            if (PlayerController.Instance.CurrentState is not ItemGetState)
                Despawn();
        }

        protected virtual void Despawn()
        {
            gameObject.SetActive(false);
            transform.parent = Services.Instance.ObjectPoolController.FloatingPoolParent;
        }

        protected virtual void Patrol()
        {
            
        }

        protected virtual void Wander()
        {
            if (Vector3.SqrMagnitude(transform.position - _wanderPosition) < 0.5f || !_navMeshAgent.hasPath || Time.time - _timeStartedWandering > 10)
                SetNewWanderPoint();

            _animator.SetFloat(Animations.MovementSpeedHash, _navMeshAgent.velocity.magnitude / _animalModel.FleeSpeed);
            CheckFlee();
        }

        protected virtual void SetNewWanderPoint()
        {
            _timeStartedWandering = Time.time;
            _wanderPosition = _startPosition + UnityEngine.Random.insideUnitSphere * _animalModel.WanderRadius;
            _wanderPosition.y = transform.position.y;
            _navMeshAgent.destination = _wanderPosition;
        }
        
        protected virtual void CheckFlee()
        {
            if (Vector3.SqrMagnitude(transform.position - PlayerController.Instance.transform.position) < _animalModel.FleeRange * _animalModel.FleeRange)
                SetState(AnimalState.Flee);
        }

        protected virtual void CheckStopFleeing()
        {
            if (Vector3.SqrMagnitude(transform.position - PlayerController.Instance.transform.position) > _animalModel.FleeRange * _animalModel.FleeRange * 2)
                SetState(AnimalState.Idle);
        }
        #endregion
        
        #region Interactions
        public virtual void PickUp()
        {
            SetState(AnimalState.Captured);
        }
        
        public virtual void ApplyWindForce(Vector3 force)
        {
            if (!_animalModel.CanBePulled)
                return;
            if (_currentState != AnimalState.Suction)
                SetState(AnimalState.Suction);
            else
                _timeStunned = 0;

            _timeSuctioned = 0;
            _rigidbody.AddForce(force, ForceMode.Force);
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (!_animalModel.CollectOnContact || other.gameObject.layer != ProjectLayers.PlayerLayer)
                return;
            
            PickUp();
        }

        #endregion
    }
    
    public enum AnimalState
    {
        Idle,
        Flee,
        Stunned,
        Suction,
        Captured,
        Lured
    }
}
