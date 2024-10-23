using System;
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
            
            _startPosition = transform.position;
            _wanderPosition = _startPosition + UnityEngine.Random.insideUnitSphere * _enemyModel.WanderRadius;
            _wanderPosition.y = transform.position.y;

            _navMeshAgent.enabled = true;
            _navMeshAgent.destination = _wanderPosition;
        }

        public override void OnDespawned()
        {
            _navMeshAgent.enabled = false;
        }

        protected virtual void Initialise()
        {
            _hp = _enemyModel.HP;
            ResetAllAnimParams();
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
        }

        protected virtual void Idle()
        {
            if (_patrolPoints.Length > 0)
                Patrol();
            else
                Wander();
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
            
            _animator.SetFloat(Animations.MovementSpeedHash, _navMeshAgent.velocity.magnitude);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_wanderPosition, 0.1f);
        }
        
        protected virtual void Aggro()
        {
            
        }
        
        protected virtual void Attack()
        {
            
        }
        
        protected virtual void Die()
        {
            OnDeath?.Invoke(this);
            _spawner.Despawn();
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
        Stunned
    }
}
