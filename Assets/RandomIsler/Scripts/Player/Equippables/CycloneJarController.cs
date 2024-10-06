using UnityEngine;

namespace RandomIsleser
{
    public class CycloneJarController : EquippableController
    {
        [SerializeField] private CycloneJarModel _model;
        [SerializeField] private ParticleSystem _blowParticles;
        [SerializeField] private ParticleSystem _suckParticles;
        [SerializeField] private MeshCaster _suckCollider;

        private bool _isSucking = false;
        private bool _isBlowing = false;

        private float _suckStrength = 0;
        private float _blowStrength = 0;

        public override int ItemIndex => _model.ItemIndex;
        public float MovementSpeedMultiplier => _model.MovementSpeedMultiplier;
        public float RotationSpeedMultiplier => _model.RotationSpeedMultiplier;


        protected override void Initialise()
        {
            _isSucking = false;
            _isBlowing = false;
        }

        public override void UpdateEquippable()
        {
            bool inUse = false;

            if (PlayerController.Instance.BlowHeld)
            {
                _blowParticles.Play();
                inUse = true;
                Cyclone(false);
            }
            else if (PlayerController.Instance.SuctionHeld)
            {
                _suckParticles.Play();
                inUse = true;
                Cyclone(true);
            }

            PlayerController.Instance.Animator.SetBool(Animations.CycloneJarChargingHash, inUse);
            _suckCollider.gameObject.SetActive(inUse);
        }

        private void Cyclone(bool isSucking)
        {
            _suckCollider.gameObject.SetActive(true);
            var colliders = _suckCollider.GetColliders();

            foreach (var coll in colliders)
            {
                if (coll.TryGetComponent(out WindMoveableController windController))
                {
                    Vector3 force = windController.transform.position - transform.position;
                    float sqrMag = force.sqrMagnitude;
                    force /= isSucking ? -sqrMag : sqrMag;
                    
                    windController.ApplyWindForce(force * _model.SuctionForce);
                }
            }
        }

        public override void OnUnequip()
        {
            PlayerController.Instance.SetState(PlayerStates.DefaultMove);
            PlayerController.Instance.CycloneCamera.SetActive(false);
        }

        public override void OnEquip()
        {
            PlayerController.Instance.SetState(PlayerStates.CycloneCombat);
            CycloneCameraTarget.Instance.SetRotation();
            PlayerController.Instance.CycloneCamera.SetActive(true);
        }
    }
}
