using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace RandomIsleser
{
    public class Enemy : Spawnable, IDamageable
    {
        [Space, Header("Enemy")]
        [SerializeField] protected EnemyModel _enemyModel;

        [SerializeField] protected Transform[] _patrolPoints;

        protected int _hp;
        protected float _lastHitTime;
        protected Vector3 _startPosition;
        protected Vector3 _wanderPosition;

        protected float _lastExpensiveUpdateTime;
        
        protected SpawnPoint _spawner;
        protected Animator _animator;
        protected Rigidbody _rigidbody;
        protected NavMeshAgent _navMeshAgent;

        protected EnemyState _currentState;
        public event Action<Enemy> OnDeath;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public override void OnSpawned(SpawnPoint spawnPoint)
        {
            _spawner = spawnPoint;
            Initialise();
        }

        public override void OnDespawned()
        {
            _navMeshAgent.enabled = false;
        }

        protected virtual void Initialise()
        {
            _hp = _enemyModel.HP;
            ResetAllAnimParams();

            _lastExpensiveUpdateTime = UnityEngine.Random.Range(0, 20);
            _startPosition = transform.position;
            _wanderPosition = _startPosition + UnityEngine.Random.insideUnitSphere * _enemyModel.WanderRadius;
            _wanderPosition.y = transform.position.y;

            _navMeshAgent.enabled = true;
            _navMeshAgent.destination = _wanderPosition;
            _navMeshAgent.speed = _enemyModel.WanderSpeed;
            _navMeshAgent.angularSpeed = _enemyModel.TurnSpeed;
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

        protected void SetState(EnemyState state)
        {
            LeaveState(_currentState);
            _currentState = state;
            EnterState(_currentState);
        }

        protected virtual void LeaveState(EnemyState state)
        {
            switch (state)
            {
                case EnemyState.Idle:
                    break;
                case EnemyState.Aggro:
                    break;
                case EnemyState.Attack:
                    break;
                default:
                    break;
            }
        }
        
        protected virtual void EnterState(EnemyState state)
        {
            switch (state)
            {
                case EnemyState.Idle:
                    _navMeshAgent.speed = _enemyModel.WanderSpeed;
                    break;
                case EnemyState.Aggro:
                    _navMeshAgent.speed = _enemyModel.ChaseSpeed;
                    break;
                case EnemyState.Attack:
                    break;
                default:
                    break;
            }
        }

        protected virtual void FixedUpdate()
        {
            switch (_currentState)
            {
                case EnemyState.Idle:
                    Idle();
                    break;
                case EnemyState.Aggro:
                    Aggro();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
                default:
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
                case EnemyState.Idle:
                    ExpensiveIdle();
                    break;
                case EnemyState.Aggro:
                    ExpensiveAggro();
                    break;
                case EnemyState.Attack:
                    ExpensiveAttack();
                    break;
                default:
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

        protected virtual void Patrol()
        {
            
        }

        protected virtual void Wander()
        {
            if (Vector3.SqrMagnitude(transform.position - _wanderPosition) < 0.5f)
            {
                _wanderPosition = _startPosition + UnityEngine.Random.insideUnitSphere * _enemyModel.WanderRadius;
                _wanderPosition.y = transform.position.y;
                _navMeshAgent.destination = _wanderPosition;
            }

            _animator.SetFloat(Animations.MovementSpeedHash, _navMeshAgent.velocity.magnitude / _enemyModel.ChaseSpeed);
            CheckAggro();
        }

        protected virtual void CheckAggro()
        {
            if (Vector3.SqrMagnitude(transform.position - PlayerController.Instance.transform.position) < _enemyModel.AggroRange * _enemyModel.AggroRange)
                SetState(EnemyState.Aggro);
        }
        
        protected virtual void Aggro()
        {
            _animator.SetFloat(Animations.MovementSpeedHash, _navMeshAgent.velocity.magnitude / _enemyModel.ChaseSpeed);
        }

        protected virtual void ExpensiveAggro()
        {
            _navMeshAgent.destination = PlayerController.Instance.transform.position;
        }
        
        protected virtual void Attack()
        {
            
        }

        protected virtual void ExpensiveAttack()
        {
            
        }
        
        protected virtual void Die()
        {
            _animator.SetBool(Animations.IsDeadHash, true);
            OnDeath?.Invoke(this);
            SetState(EnemyState.Dead);
            
            var seq = DOTween.Sequence();
            seq.AppendInterval(_enemyModel.DeathTime);
            seq.OnComplete(() =>
            {
                _spawner.Despawn();
            });
        }

        public virtual void TakeDamage(int damage)
        {
            if (Time.time - _lastHitTime < _enemyModel.InvincibleTime)
                return;
            
            _hp -= damage;

            _lastHitTime = Time.time;

            if (_hp <= 0)
                Die();
        }
    }
    
    public enum EnemyState
    {
        Idle,
        Aggro,
        Attack,
        Stunned,
        Dead
    }
}
