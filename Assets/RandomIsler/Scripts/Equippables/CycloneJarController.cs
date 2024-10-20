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
        [SerializeField] private ParticleSystem _cycloneJumpParticles;
        [SerializeField] private MeshCaster _suckCollider;
        [SerializeField] private Transform _holdPoint;

        private bool _isItemHeld = false;
        private CycloneMoveableController _heldItem;
        private float _timeLastFiredItem;

        private bool _itemEquipped = false;
        private bool _chargingJump = false;
        private float _jumpChargeRatio = 0;
        
        public override int ItemIndex => _model.ItemIndex;
        public float MovementSpeedMultiplier => _model.MovementSpeedMultiplier;
        public float RotationSpeedMultiplier => _model.RotationSpeedMultiplier;


        protected override void Initialise()
        {
            base.Initialise();
            _itemEquipped = true;
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            _itemEquipped = false;
            _chargingJump = false;
            _jumpChargeRatio = 0;
        }

        public override void UseItem()
        {
            if (!_itemEquipped || !PlayerController.Instance.IsGrounded)
                return;

            _heldItemParticles.Play();
            _chargingJump = true;
            _jumpChargeRatio = 0;
            PlayerController.Instance.EquipmentAnimator.SetBool(Animations.CycloneJarChargingHash, false);
            PlayerController.Instance.EquipmentAnimator.SetBool(Animations.CycloneJarJumpChargeHash, true);
        }
        
        public override void ReleaseItem()
        {
            base.ReleaseItem();
            if (!_chargingJump)
                return;
            if (_jumpChargeRatio > 1)
                _jumpChargeRatio = 1;

            _heldItemParticles.Stop();
            _cycloneJumpParticles.Play();
            PlayerController.Instance.EquipmentAnimator.SetBool(Animations.CycloneJarJumpChargeHash, false);
            PlayerController.Instance.JumpSetHeight(_model.JumpHeight * _jumpChargeRatio);
            _chargingJump = false;
        }

        public override void UpdateEquippable()
        {
            bool inUse = false;

            if (_chargingJump)
            {
                _jumpChargeRatio += Time.deltaTime * _model.JumpChargeRate;

                return;
            }

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

            if (_isItemHeld && !inUse)
            {
                FireItem(Vector3.zero, false);
            }

            PlayerController.Instance.EquipmentAnimator.SetBool(Animations.CycloneJarChargingHash, inUse);
            _suckCollider.gameObject.SetActive(inUse);
        }

        private void Cyclone(bool isSucking)
        {
            if (Time.time - _timeLastFiredItem < _model.FireCooldown
                || (isSucking && _isItemHeld))
                return;

            if (_isItemHeld)
            {
                FireItem(transform.forward * _model.FiringForce);
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
                    Vector3 force = cycloneMoveableController.transform.position - _holdPoint.position;
                    force *= isSucking ? -1 : 1;
                    
                    cycloneMoveableController.ApplyWindForce(force.normalized * _model.SuctionForce);

                    if (isSucking && force.sqrMagnitude < _model.SuckDistance * _model.SuckDistance)
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

        private void FireItem(Vector3 force, bool playFireEffect = true)
        {
            _heldItem.FireItem(force);
            _heldItemParticles.Stop();
            if (playFireEffect)
                _fireItemParticles.Play();
            _isItemHeld = false;
            _heldItem = null;
            _timeLastFiredItem = Time.time;
        }

        public override void OnUnequip()
        {
            base.OnUnequip();
            PlayerController.Instance.SetState(PlayerStates.DefaultMove);
            CameraManager.Instance.SetCycloneCamera(false);

            if (_isItemHeld)
                FireItem(Vector3.zero, false);
        }

        public override void OnEquip()
        {
            base.OnEquip();
            PlayerController.Instance.SetState(PlayerStates.CycloneCombat);
            CycloneCameraTarget.Instance.SetRotation();
            CameraManager.Instance.SetCycloneCamera(true);
        }
    }
}
