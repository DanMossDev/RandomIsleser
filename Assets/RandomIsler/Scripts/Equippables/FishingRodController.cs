using System;
using UnityEngine;
using DG.Tweening;

namespace RandomIsleser
{
    public class FishingRodController : EquippableController
    {
        [SerializeField] private FishingRodModel _model;
        [SerializeField] private FishingRodHookController _fishHookPrefab;
        [SerializeField] private Transform _fishHookStart;
        
        [SerializeField] private Vector3 _hookOffset;
        
        private Vector3 _grapplePoint;

        private float _distanceTravelled;
        private bool _grappleHit;
        private bool _rodCast = false;
        private bool _isReeling;
        
        private FishingRodHookController _fishHook;
        private Lure _lure;
        private Animator _animator;

        public bool IsReeling => _isReeling;
        public override int ItemIndex => _model.ItemIndex;
        public FishingRodModel Model => _model;

        private void Awake()
        {
            _fishHook = Instantiate(_fishHookPrefab, _fishHookStart);
            _fishHook.transform.localPosition = _hookOffset;
            _lure = _fishHook.GetComponent<Lure>();
            _lure.Init(this);
            GetComponentInChildren<FishingLineController>().SetTarget(_fishHook.transform);
            _animator = PlayerController.Instance.LocomotionAnimator;
        }

        private void FixedUpdate()
        {
            if (_isReeling)
            {
                var dir = transform.position - _lure.transform.position;
                _lure.ApplyForce((dir.normalized + Vector3.up) * _model.ReelForce);

                if (dir.sqrMagnitude < 4)
                {
                    OnReelReturned();
                    _fishHook.ReturnHook(_fishHookStart);
                    _rodCast = false;
                    _isReeling = false;
                    _lure.ReelReturned();
                }
            }
        }

        protected override void Initialise()
        {
            _grapplePoint = Vector3.zero;
            _distanceTravelled = 0;
            _grappleHit = false;
            _isReeling = false;
            _fishHook.transform.localPosition = Vector3.zero;
            _fishHook.Init(this);
        }
        
        public override void CheckAim(Vector3 origin, Vector3 aimDirection)
        {
            bool showInteractReticle = Physics.SphereCast(
                origin,
                _model.AimTolerance,
                aimDirection,
                out _,
                _model.Range,
                _model.InteractLayer);
            
            Services.Instance.UIManager.SetAimingReticle(showInteractReticle);
        }

        public override void UseItem()
        {
            PlayerController.Instance.SetState(PlayerStates.CastRodMove);
            var aimPos = PlayerController.Instance.AimPosition;
            var aimDirection = PlayerController.Instance.AimDirection;
            _lure.OnCast();
            _animator.SetBool(Animations.FishingRodCastHash, true);

            if (Physics.SphereCast(
                    aimPos,
                    _model.AimTolerance,
                    aimDirection,
                    out RaycastHit hit,
                    _model.Range,
                    _model.HitLayers))
            {
                _distanceTravelled = hit.distance;
                _grappleHit = (_model.InteractLayer & 1 << hit.collider.gameObject.layer) != 0;
                if (_grappleHit)
                    _grapplePoint = hit.point;
            }
            else
            {
                _distanceTravelled = _model.Range;
                _grappleHit = false;
            }
            
            Vector3 targetPos = aimPos + aimDirection * _distanceTravelled;
            
            var seq = DOTween.Sequence();
            seq.AppendInterval(_model.WindUpTime);
            seq.OnComplete(() =>
            {
                _rodCast = true;
                _fishHook.CastRod(targetPos, _model.CastSpeed / _distanceTravelled);
            });
        }

        public void ReelComplete()
        {
            if (!_grappleHit)
            {
                OnReelReturned();
                _rodCast = false;
                _fishHook.ReturnHook(_fishHookStart);
                return;
            }
            PlayerController.Instance.SetGrapplePoint(_grapplePoint);
            PlayerController.Instance.SetState(PlayerStates.RodGrappleMove);
        }

        public void BackPressed()
        {
            if (_rodCast)
            {
                ReelComplete();
                OnReelReturned();
            }
            else
                PlayerController.Instance.SetState(PlayerStates.DefaultMove);
        }

        public void ReturnHook()
        {
            _fishHook.ReturnHook(_fishHookStart);
            EndReel();
            OnReelReturned();
        }

        public void BeginReel()
        {
            _animator.SetBool(Animations.FishingRodReelHash, true);
            _isReeling = true;

            if (_lure.HasBite())
                _lure.BeginReel();
        }

        public void EndReel()
        {
            _animator.SetBool(Animations.FishingRodReelHash, false);
            _isReeling = false;
        }

        private void OnReelReturned()
        {
            PlayerController.Instance.SetState(PlayerStates.AimCombat);
            _animator.SetBool(Animations.FishingRodReelHash, false);
            _animator.SetBool(Animations.FishingRodCastHash, false);
        }
    }
}
