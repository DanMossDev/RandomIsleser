using System;
using UnityEngine;

namespace RandomIsleser
{
    public class Enemy : Spawnable, IDamageable
    {
        [Space, Header("Enemy")]
        [SerializeField] protected EnemyModel _enemyModel;

        protected int _hp;
        protected float _lastHitTime;
        protected Animator _animator;

        public event Action<Enemy> OnDeath;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        protected virtual void OnEnable()
        {
            Initialise();
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

        protected virtual void Die()
        {
            OnDeath?.Invoke(this);
            Services.Instance.ObjectPoolController.Return(gameObject, ObjectPoolKey);
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
}
