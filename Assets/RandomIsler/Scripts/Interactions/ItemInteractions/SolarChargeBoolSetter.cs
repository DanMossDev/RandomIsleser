using UnityEngine;

namespace RandomIsleser
{
    public class SolarChargeBoolSetter : SolarChargeAbsorber
    {
        [SerializeField] protected SaveableBool _boolToSet;

        public override void CompleteCharge()
        {
            if (_fullyCharged)
                return;

            _boolToSet.Value = true;
            
            base.CompleteCharge();
        }
    }
}
