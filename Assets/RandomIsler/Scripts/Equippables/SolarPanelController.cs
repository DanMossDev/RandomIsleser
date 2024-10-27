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
        { }

        public override void CheckAim(Vector3 origin, Vector3 aimDirection)
        {
        }
        
        public override void UseItem()
        { }
        
        public override void ReleaseItem()
        { }
    }
}
