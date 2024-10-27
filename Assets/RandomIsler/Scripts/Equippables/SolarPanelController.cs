using UnityEngine;

namespace RandomIsleser
{
    public class SolarPanelController : EquippableController
    {
        [SerializeField] private SolarPanelModel _model;
        [SerializeField] private Transform _beamOrigin;
        
        [SerializeField] private LineRenderer _lightBeam;

        [SerializeField] private LayerMask _lightCheckLayers;
        
        private Collider[] _lightCheck = new Collider[1];
        private Collider[] _beamHitCheck = new Collider[3];

        private float _chargeAmount;

        private Light _sun;
        
        public override int ItemIndex => _model.ItemIndex;
        
        protected override void Initialise()
        { }
        
        protected override void Cleanup()
        { }

        public override void UpdateEquippable()
        {
            if (Physics.OverlapSphereNonAlloc(transform.position, 1, _lightCheck, _lightCheckLayers) > 0 || CheckIfInSunlight())
                _chargeAmount += Time.deltaTime * _model.ChargeSpeed;

            if (PlayerController.Instance.FireHeld)
                FireLightBeam();
            else
                ClearLightBeam();
        }

        private bool CheckIfInSunlight()
        {
            if (!SceneTransitionManager.CurrentSceneIsOpenWorld())
                return false;
            
            var sunDir = -_sun.transform.forward;

            if (Physics.Raycast(transform.position, sunDir, out RaycastHit info, 200))
                return false;
            
            return true;
        }

        private void FireLightBeam()
        {
            if (_chargeAmount <= 0)
            {
                _lightBeam.positionCount = 0;
                return;
            }
            _lightBeam.positionCount = 2;
            _lightBeam.SetPosition(0, _beamOrigin.position);
            if (Physics.SphereCast(_beamOrigin.position, _model.RayWidth, _beamOrigin.forward, out RaycastHit info, _model.MaxDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                _lightBeam.SetPosition(1, info.point);

                var amount = Physics.OverlapSphereNonAlloc(info.point, 1, _beamHitCheck);
                for (int i = 0; i < amount; i++)
                {
                    if (_beamHitCheck[i].TryGetComponent(out SolarChargeAbsorber absorber))
                    {
                        absorber.ReceiveCharge(Time.deltaTime);
                    }
                }
            }
            else
                _lightBeam.SetPosition(1, _beamOrigin.position + _beamOrigin.forward * _model.MaxDistance);
            
            _chargeAmount -= Time.deltaTime * _model.FireSpeed;
            _chargeAmount = _chargeAmount < 0 ? 0 : _chargeAmount;
        }

        private void ClearLightBeam()
        {
            _lightBeam.positionCount = 0;
        }
        
        public override void UseItem()
        { }
        
        public override void ReleaseItem()
        { }
        
        public override void OnUnequip()
        {
            base.OnUnequip();
            
            _lightBeam.positionCount = 0;
            PlayerController.Instance.RecenterCamera();
            PlayerController.Instance.SetState(PlayerStates.DefaultMove);
            CameraManager.Instance.SetSolarPanelAimCamera(false);
        }

        public override void OnEquip()
        {
            base.OnEquip();
            
            if (SceneTransitionManager.CurrentSceneIsOpenWorld() && RenderSettings.sun != null)
                _sun = RenderSettings.sun;
            
            PlayerController.Instance.SetState(PlayerStates.SolarPanelCombat);
            CameraManager.Instance.SetSolarPanelAimCamera(true);
        }
    }
}
