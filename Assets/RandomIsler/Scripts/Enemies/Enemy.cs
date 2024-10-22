using System;
using UnityEngine;

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

        protected EnemyState _currentState;
        public event Action<Enemy> OnDeath;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        public override void OnSpawned(SpawnPoint spawnPoint)
        {
            _spawner = spawnPoint;
            Initialise();
            
            _startPosition = transform.position;
            _wanderPosition = _startPosition + UnityEngine.Random.insideUnitSphere * _enemyModel.WanderRadius;
            _wanderPosition.y = transform.position.y;
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
            }
            
            Vector3 direction = (_wanderPosition - transform.position).normalized;
            
            _rigidbody.AddForce(_enemyModel.MovementSpeed * Time.deltaTime * direction, ForceMode.Force);
            _animator.SetFloat(Animations.MovementSpeedHash, _rigidbody.velocity.magnitude);
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
