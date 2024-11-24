using DG.Tweening;
using UnityEngine;

namespace RandomIsleser
{
    public class FishController : AnimalController
    {
        public Vector3 BiteOffset;
        public Vector3 BiteRotation;
        
        private BuoyancyController _buoyancyController;
        private FishingRodController _fishingRodController;

        private float _targetBuoyancy;
        private float _timeBit;

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
                    _rigidbody.isKinematic = false;
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
                    _rigidbody.isKinematic = true;
                    _animator.SetBool(Animations.IsStunnedHash, true);
                    break;
                case AnimalState.Suction:
                    break;
                case AnimalState.Captured:
                    transform.parent = PlayerController.Instance.PickupHoldPoint;
                    transform.localPosition = Vector3.zero;
                    _animator.SetBool(Animations.IsCaughtHash, true);
                    PlayerController.Instance.SetState(PlayerStates.DefaultMove);
                    if (RuntimeSaveManager.Instance.LocalSaveData.UnlockAnimal(_animalModel.Species, _rarityLevel))
                    {
                        _isNewItem = true;
                        PlayerController.Instance.NewItemGet(_animalModel);
                    }
                    break;
                case AnimalState.Null:
                    transform.parent = null;
                    _rigidbody.isKinematic = true;
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
        }

        protected override void SetNewWanderPoint() //TODO - add chance to go to idle state
        {
            _timeStartedWandering = Time.time;
            _wanderPosition = _startPosition + UnityEngine.Random.insideUnitSphere * _animalModel.WanderRadius;
            _wanderPosition.y = transform.position.y;
            _targetBuoyancy = Random.Range(_animalModel.MinBuoyancy, _animalModel.MaxBuoyancy);
        }

        public void BeginReel()
        {
            SetState(AnimalState.Stunned);
            transform.localPosition = BiteOffset;
            transform.localEulerAngles = BiteRotation;
        }

        protected override void Stunned()
        {
            _timeBit += Time.deltaTime;
            
            if (_timeBit >= _animalModel.EscapeTime && !_fishingRodController.IsReeling)
            {
                Escape();
                return;
            }
        }

        protected override void Flee()
        {
            _timeBit += Time.deltaTime;
            if (_timeBit >= _animalModel.EscapeTime)
            {
                Escape();
                return;
            }
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
                    _timeBit = 0;
                    SetState(AnimalState.Flee);
                    _currentLure.RemoveFollower();
                }
            }
            else
            {
                _rigidbody.AddForce(dir.normalized * _animalModel.WanderSpeed, ForceMode.Acceleration);
                _buoyancyController.SetFloatingPower(Mathf.Lerp(_buoyancyController.GetFloatingPower(), 20, Time.deltaTime));
                RotateTowards(_rigidbody.velocity);
            }
            if (dir.sqrMagnitude > (_animalModel.LureRadius * _animalModel.LureRadius) + 2)
                SetState(AnimalState.Idle);
        }
        
        protected override void Lured(Lure lure)
        {
            base.Lured(lure);
            _fishingRodController = lure.FishingRodController;
            SetState(AnimalState.Lured);
        }

        private void Escape()
        {
            SetState(AnimalState.Null);
            _currentLure.FishEscaped();
            var direction = transform.position - _fishingRodController.transform.position;
            if (direction.y > 0)
                direction.y = -5;
            
            var seq = DOTween.Sequence();
            seq.Append(transform.DOMove(transform.position + (direction.normalized * 10), 2).OnUpdate(() => RotateTowards(direction)));
            seq.OnComplete(Despawn);
        }

        public void Capture()
        {
            SetState(AnimalState.Captured);
        }
    }
}
