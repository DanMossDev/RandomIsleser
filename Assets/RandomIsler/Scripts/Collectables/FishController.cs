using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class FishController : AnimalController
    {
        private BuoyancyController _buoyancyController;

        private float _targetBuoyancy;

        public float BiteTolerance => _animalModel.EscapeTime;
        
        protected override void Awake()
        {
            _buoyancyController = GetComponent<BuoyancyController>();
            _animator = GetComponentInChildren<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        }
        
        public override void OnDespawned()
        {
        }
        
        protected override void Initialise()
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

            SetNewWanderPoint();

            _isNewItem = false;
            _timePickedUp = 0;
        }
        
        
#region States

        protected override void LeaveState(AnimalState state)
        {
            switch (state)
            {
                case AnimalState.Idle:
                    break;
                case AnimalState.Stunned:
                    _rigidbody.isKinematic = true;
                    _animator.SetBool(Animations.IsStunnedHash, false);
                    break;
                case AnimalState.Suction:
                    _rigidbody.isKinematic = true;
                    _animator.SetBool(Animations.IsStunnedHash, false);
                    break;
                case AnimalState.Captured:
                    break;
            }
        }
        
        protected override void EnterState(AnimalState state)
        {
            switch (state)
            {
                case AnimalState.Idle:
                    SetNewWanderPoint();
                    break;
                case AnimalState.Flee:
                    _buoyancyController.SetFloatingPower(0);
                    break;
                case AnimalState.Stunned:
                    break;
                case AnimalState.Suction:
                    break;
                case AnimalState.Captured:
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

        private void RotateTowards(Vector3 newForward)
        {
            transform.forward = Vector3.Slerp(transform.forward, newForward, Time.deltaTime * _animalModel.TurnSpeed);
        }

        protected override void Wander()
        {
            Vector3 dir = _wanderPosition - transform.position;
            if (dir.sqrMagnitude < 0.5f || Time.time - _timeStartedWandering > 10)
                SetNewWanderPoint();

            _rigidbody.AddForce(dir.normalized * _animalModel.WanderSpeed, ForceMode.Acceleration);
            _animator.SetFloat(Animations.MovementSpeedHash, _rigidbody.velocity.magnitude / _animalModel.FleeSpeed);
            RotateTowards(_rigidbody.velocity);
            _buoyancyController.SetFloatingPower(Mathf.Lerp(_buoyancyController.GetFloatingPower(), _targetBuoyancy, Time.deltaTime));
            CheckLured();
        }

        protected override void SetNewWanderPoint() //TODO - add chance to go to idle state
        {
            _timeStartedWandering = Time.time;
            _wanderPosition = _startPosition + UnityEngine.Random.insideUnitSphere * _animalModel.WanderRadius;
            _wanderPosition.y = transform.position.y;
            _targetBuoyancy = Random.Range(_animalModel.MinBuoyancy, _animalModel.MaxBuoyancy);
        }

        protected override void Flee()
        {
            RotateTowards(_rigidbody.velocity);
        }

        protected override void ExpensiveFlee()
        {
            
        }

        protected override void Lure()
        {
            Vector3 dir = _currentLure.transform.position - transform.position;

            if (dir.sqrMagnitude < 0.5f)
            {
                if (_currentLure.Interact(this))
                {
                    SetState(AnimalState.Flee);
                }
            }
            else
            {
                _rigidbody.AddForce(dir.normalized * _animalModel.WanderSpeed, ForceMode.Acceleration);
                _buoyancyController.SetFloatingPower(Mathf.Lerp(_buoyancyController.GetFloatingPower(), 20, Time.deltaTime));
                RotateTowards(_rigidbody.velocity);
            }

        }
        
        protected override void Lured(Lure lure)
        {
            base.Lured(lure);
            
            SetState(AnimalState.Lured);
        }
    }
}
