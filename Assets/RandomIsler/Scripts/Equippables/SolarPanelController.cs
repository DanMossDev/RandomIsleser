using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class SolarPanelController : EquippableController
    {
        [SerializeField] private SolarPanelModel _model;
        [SerializeField] private Transform _beamOrigin;
        
        public override int ItemIndex => _model.ItemIndex;
        
        protected override void Initialise()
        { }
        
        protected override void Cleanup()
        { }

        public override void UpdateEquippable()
        {
            if (PlayerController.Instance.FireHeld)
                FireLightBeam();
        }

        private void FireLightBeam()
        {
            Debug.DrawRay(_beamOrigin.position, _beamOrigin.forward * 100f, Color.red);
        }
        
        public override void UseItem()
        { }
        
        public override void ReleaseItem()
        { }
        
        public override void OnUnequip()
        {
            base.OnUnequip();
            
            PlayerController.Instance.RecenterCamera();
            PlayerController.Instance.SetState(PlayerStates.DefaultMove);
            CameraManager.Instance.SetSolarPanelAimCamera(false);
        }

        public override void OnEquip()
        {
            base.OnEquip();
            
            PlayerController.Instance.SetState(PlayerStates.SolarPanelCombat);
            CameraManager.Instance.SetSolarPanelAimCamera(true);
        }
    }
}
