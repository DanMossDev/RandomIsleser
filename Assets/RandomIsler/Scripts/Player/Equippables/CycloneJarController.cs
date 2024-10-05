using DG.Tweening;
using UnityEngine;

namespace RandomIsleser
{
    public class CycloneJarController : EquippableController
    {
        [SerializeField] private CycloneJarModel _model;
        [SerializeField] private ParticleSystem _blowParticles;
        [SerializeField] private MeshCaster _suckCollider;

        private float _chargeRatio = 0;

        private float _emissionRate;
        
        private bool _isFull = false;
        private bool _isCharging = false;
        private bool _hasFired = false;
        public override int ItemIndex => _model.ItemIndex;
        public float MovementSpeedMultiplier => _model.MovementSpeedMultiplier;
        public float RotationSpeedMultiplier => _model.RotationSpeedMultiplier;

        private void Awake()
        {
            _emissionRate = _blowParticles.emission.rateOverTime.constant;
        }

        protected override void Initialise()
        {
            _isCharging = true;
            _hasFired = false;
            _chargeRatio = 0;
        }

        public override void UpdateEquippable()
        {
            if (_hasFired)
                return;

            if (_chargeRatio < 1)
                _chargeRatio += Time.deltaTime * _model.ChargeSpeed;
            else
                _chargeRatio = 1;

            if (!_isCharging)
            {
                Blow();
                return;
            }

            Suck();
        }
        
        public override void UseItem()
        {
            _isCharging = true;
            _chargeRatio = 0;
            _hasFired = false;
            BeginSuction();
        }

        public override void ReleaseItem()
        {
            _isCharging = false;
            Blow();
        }

        private void Suck()
        {
            var colliders = _suckCollider.GetColliders();

            foreach (var coll in colliders)
            {
                if (coll.TryGetComponent(out WindMoveableController windController))
                    windController.ApplyWindForce(transform.position, transform.forward * - _model.SuctionForce);
            }
        }

        private void Blow()
        {
            var player = PlayerController.Instance;
            player.SetStateRotationMultiplier(0);
            player.Animator.SetBool(Animations.CycloneJarChargingHash, false);
            _hasFired = true;
            
            var em = _blowParticles.emission;
            em.rateOverTime = _emissionRate * _chargeRatio;
            _blowParticles.Play();
            
            var seq = DOTween.Sequence();
            seq.AppendInterval(_model.CooldownTime);
            seq.OnComplete(() =>
            {
                player.SetState(PlayerStates.DefaultMove);
            });
        }

        private void BeginSuction()
        {
            PlayerController.Instance.SetState(PlayerStates.CycloneSuctionCombat);
        }
    }
}
