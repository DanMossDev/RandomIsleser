using UnityEngine;

namespace RandomIsleser
{
    public class CycloneJarController : EquippableController
    {
        [SerializeField] private CycloneJarModel _model;
        [SerializeField] private ParticleSystem _blowParticles;
        [SerializeField] private ParticleSystem _suckParticles;
        [SerializeField] private ParticleSystem _heldItemParticles;
        [SerializeField] private ParticleSystem _fireItemParticles;
        [SerializeField] private MeshCaster _suckCollider;
        [SerializeField] private Transform _holdPoint;

        private bool _isItemHeld = false;
        private CycloneMoveableController _heldItem;
        private float _timeLastFiredItem;

        public override int ItemIndex => _model.ItemIndex;
        public float MovementSpeedMultiplier => _model.MovementSpeedMultiplier;
        public float RotationSpeedMultiplier => _model.RotationSpeedMultiplier;


        protected override void Initialise()
        { }

        public override void UpdateEquippable()
        {
            bool inUse = false;

            if (PlayerController.Instance.BlowHeld)
            {
                inUse = true;
                Cyclone(false);
            }
            else if (PlayerController.Instance.SuctionHeld)
            {
                inUse = true;
                Cyclone(true);
            }

            PlayerController.Instance.Animator.SetBool(Animations.CycloneJarChargingHash, inUse);
            _suckCollider.gameObject.SetActive(inUse);
        }

        private void Cyclone(bool isSucking)
        {
            if (Time.time - _timeLastFiredItem < _model.FireCooldown
                || (isSucking && _isItemHeld))
                return;

            if (_isItemHeld)
            {
                _heldItem.FireItem(transform.forward * _model.FiringForce);
                _heldItemParticles.Stop();
                _fireItemParticles.Play();
                _isItemHeld = false;
                _heldItem = null;
                _timeLastFiredItem = Time.time;
                return;
            }
            else
            {
                var particles = isSucking ? _suckParticles : _blowParticles;
                particles.Play();
            }
            
            _suckCollider.gameObject.SetActive(true);
            var colliders = _suckCollider.GetColliders();

            foreach (var coll in colliders)
            {
                if (coll.TryGetComponent(out CycloneMoveableController cycloneMoveableController))
                {
                    Vector3 force = cycloneMoveableController.transform.position - transform.position;
                    float sqrMag = force.sqrMagnitude;
                    force /= isSucking ? -sqrMag : sqrMag;
                    
                    cycloneMoveableController.ApplyWindForce(force * _model.SuctionForce);

                    if (isSucking && sqrMag < _model.SuckDistance * _model.SuckDistance)
                    {
                        if (cycloneMoveableController.CanBeSuckedUp)
                        {
                            //TODO - Add some kind of "absorbed effect" enum to CycloneMoveableController, apply effect here
                            cycloneMoveableController.AbsorbItem(_holdPoint);
                        }
                        else if (cycloneMoveableController.CanBeHeld)
                        {
                            cycloneMoveableController.BeginHold(_holdPoint);
                            _heldItem = cycloneMoveableController;
                            _isItemHeld = true;
                            _heldItemParticles.Play();
                            break;
                        }
                    }
                }
            }
        }

        private void FireItem(Vector3 force)
        {
            _heldItem.FireItem(force);
            _isItemHeld = false;
            _heldItem = null;
        }

        public override void OnUnequip()
        {
            PlayerController.Instance.SetState(PlayerStates.DefaultMove);
            PlayerController.Instance.CycloneCamera.SetActive(false);

            if (_isItemHeld)
                FireItem(Vector3.zero);
        }

        public override void OnEquip()
        {
            PlayerController.Instance.SetState(PlayerStates.CycloneCombat);
            CycloneCameraTarget.Instance.SetRotation();
            PlayerController.Instance.CycloneCamera.SetActive(true);
        }
    }
}
