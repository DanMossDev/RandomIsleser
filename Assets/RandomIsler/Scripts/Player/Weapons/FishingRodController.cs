using UnityEngine;
using DG.Tweening;

namespace RandomIsleser
{
    public class FishingRodController : AimableController
    {
        [SerializeField] private AimableModel _model;
        [SerializeField] private GameObject _fishHook;

        private GrapplePoint _grapplePoint;

        private float _distanceTravelled;
        private bool _grappleHit;

        private Vector3 _startingHookPosition;

        private void Awake()
        {
            _startingHookPosition = _fishHook.transform.localPosition;
        }

        private void OnEnable()
        {
            Initialise();
        }

        private void Initialise()
        {
            _grapplePoint = null;
            _distanceTravelled = 0;
            _grappleHit = false;
            _fishHook.transform.localPosition = _startingHookPosition;
        }
        
        public override void CheckAim(Vector3 aimDirection)
        {
            Debug.DrawLine(transform.position, transform.position + (aimDirection * _model.Range), Color.red);
            if (Physics.SphereCast(
                    transform.position, 
                    _model.AimTolerance, 
                    aimDirection, 
                    out RaycastHit hit,
                    _model.Range, 
                    _model.HitLayers))
            {
                if ((_model.InteractLayer & 1 << hit.collider.gameObject.layer) != 0)
                {
                    //TODO - show "interact" icon instead of aim icon
                    Debug.Log("You can interact with this!");
                }
            }
        }

        public override void Shoot(Vector3 aimDirection)
        {
            PlayerController.Instance.SetState(PlayerStates.CastRodCombat);

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
                {
                    _grappleHit = hit.collider.gameObject.TryGetComponent(out _grapplePoint);
                }
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
                //TODO - make a "rod return" state
                PlayerController.Instance.SetState(PlayerStates.AimCombat);
                return;
            }
            
            PlayerController.Instance.SetGrapplePoint(_grapplePoint.transform);
            //TODO - Make a grapple state
        }
    }
}
