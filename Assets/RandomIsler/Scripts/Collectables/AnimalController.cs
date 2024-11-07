using UnityEngine;
using UnityEngine.AI;

namespace RandomIsleser
{
    public class AnimalController : Spawnable
    {
        [Space, Header("Animal")]
        [SerializeField] protected AnimalModel _animalModel;

        [SerializeField] protected Transform[] _patrolPoints;
        
        protected Vector3 _startPosition;
        protected Vector3 _wanderPosition;

        protected float _lastExpensiveUpdateTime;
        protected float _timeStunned;
        protected float _timeSuctioned;

        protected bool _useGravity;

        protected bool _isNewItem;
        protected float _timePickedUp;
        
        protected Animator _animator;
        protected Rigidbody _rigidbody;
        protected NavMeshAgent _navMeshAgent;

        protected AnimalState _currentState;

        protected virtual void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _navMeshAgent = GetComponent<NavMeshAgent>();

            _useGravity = _rigidbody.useGravity;
        }

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

            _lastExpensiveUpdateTime = UnityEngine.Random.Range(0, 0.2f);
            _startPosition = transform.position;

            _navMeshAgent.enabled = true;
            _navMeshAgent.speed = _animalModel.WanderSpeed;
            _navMeshAgent.angularSpeed = _animalModel.TurnSpeed;
            SetNewWanderPoint();

            _isNewItem = false;
            _timePickedUp = 0;
        }

        public virtual void PickUp()
        {
            SetState(AnimalState.Captured);
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

                    if (Services.Instance.RuntimeSaveManager.LocalSaveData.UnlockAnimal(_animalModel.Species))
                    {
                        _isNewItem = true;
                        PlayerController.Instance.NewItemGet(_animalModel);
                    }
                    
                    break;
            }
        }

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
            _rigidbody.useGravity = _timeStunned >= Time.deltaTime * 2 && _useGravity;
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
            if (Vector3.SqrMagnitude(transform.position - _wanderPosition) < 0.5f || !_navMeshAgent.hasPath)
                SetNewWanderPoint();

            _animator.SetFloat(Animations.MovementSpeedHash, _navMeshAgent.velocity.magnitude / _animalModel.FleeSpeed);
            CheckFlee();
        }

        protected virtual void SetNewWanderPoint()
        {
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

        public void ApplyWindForce(Vector3 force)
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
    }
    
    public enum AnimalState
    {
        Idle,
        Flee,
        Stunned,
        Suction,
        Captured
    }
}
