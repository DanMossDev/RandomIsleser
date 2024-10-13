using UnityEngine;
using DG.Tweening;

namespace RandomIsleser
{
    public class FishingRodController : EquippableController
    {
        [SerializeField] private FishingRodModel _model;
        [SerializeField] private GameObject _fishHook;

        private Vector3 _grapplePoint;

        private float _distanceTravelled;
        private bool _grappleHit;

        private Vector3 _startingHookPosition;

        public override int ItemIndex => _model.ItemIndex;
        public FishingRodModel Model => _model;

        private void Awake()
        {
            _startingHookPosition = _fishHook.transform.localPosition;
        }

        protected override void Initialise()
        {
            _grapplePoint = Vector3.zero;
            _distanceTravelled = 0;
            _grappleHit = false;
            _fishHook.transform.localPosition = _startingHookPosition;
        }
        
        public override void CheckAim(Vector3 aimDirection)
        {
            bool showInteractReticle = false;
            if (Physics.SphereCast(
                    transform.position, 
                    _model.AimTolerance, 
                    aimDirection, 
                    out RaycastHit hit,
                    _model.Range, 
                    _model.HitLayers))
            {
                showInteractReticle = (_model.InteractLayer & 1 << hit.collider.gameObject.layer) != 0;
            }
            Services.Instance.UIManager.SetAimingReticle(showInteractReticle);
        }

        public override void UseItem()
        {
            PlayerController.Instance.SetState(PlayerStates.CastRodMove);
            var aimDirection = PlayerController.Instance.AimDirection;

            if (Physics.SphereCast(
                    transform.position,
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
            seq.Append(_fishHook.transform.DOMove(targetPos, _model.CastTime));
            seq.OnComplete(ReelComplete);
            //seq.Join(_fishHook.transform.DOLocalMoveY())
        }

        private void ReelComplete()
        {
            if (!_grappleHit)
            {
                PlayerController.Instance.EquipmentAnimator.SetTrigger(Animations.FishingRodReturnHash);
                return;
            }
            PlayerController.Instance.SetGrapplePoint(_grapplePoint);
            PlayerController.Instance.SetState(PlayerStates.RodGrappleMove);
            //TODO - Make a grapple state
        }
    }
}
