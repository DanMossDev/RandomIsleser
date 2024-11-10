using UnityEngine;
using DG.Tweening;

namespace RandomIsleser
{
    public class FishingRodController : EquippableController
    {
        [SerializeField] private FishingRodModel _model;
        [SerializeField] private FishingRodHookController _fishHookPrefab;
        [SerializeField] private Transform _fishHookStart;
        
        private Vector3 _grapplePoint;

        private float _distanceTravelled;
        private bool _grappleHit;
        
        private FishingRodHookController _fishHook;

        public override int ItemIndex => _model.ItemIndex;
        public FishingRodModel Model => _model;

        private void Awake()
        {
            _fishHook = Instantiate(_fishHookPrefab, _fishHookStart);
            GetComponentInChildren<FishingLineController>().SetTarget(_fishHook.transform);
        }

        protected override void Initialise()
        {
            _grapplePoint = Vector3.zero;
            _distanceTravelled = 0;
            _grappleHit = false;
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
            
            Vector3 targetPos = transform.position + aimDirection * _distanceTravelled;
            
            var seq = DOTween.Sequence();
            seq.AppendInterval(_model.WindUpTime);
            seq.OnComplete(() =>
            {
                _rodCast = true;
                _fishHook.CastRod(targetPos);
            });
        }

        public void ReelComplete()
        {
            if (!_grappleHit)
            {
                PlayerController.Instance.EquipmentAnimator.SetTrigger(Animations.FishingRodReturnHash);
                _fishHook.ReturnHook(_fishHookStart);
                _rodCast = false;
                return;
            }
            PlayerController.Instance.SetGrapplePoint(_grapplePoint);
            PlayerController.Instance.SetState(PlayerStates.RodGrappleMove);
        }

        private bool _rodCast = false;

        public void BackPressed()
        {
            if (_rodCast)
            {
                ReelComplete();
                PlayerController.Instance.SetState(PlayerStates.AimCombat);
            }
            else
                PlayerController.Instance.SetState(PlayerStates.DefaultMove);
        }
    }
}
