using UnityEngine;

namespace RandomIsleser
{
    public class SolarChargeGrowable : SolarChargeBoolSetter
    {
        [SerializeField] private Transform _endPositionTransform;
        
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        
        private void Start()
        {
            _startPosition = transform.position;
            _endPosition = _endPositionTransform.position;
            
            if (_boolToSet.Value)
                ReceiveCharge(10);
        }

        protected override void OnChargeUpdated()
        {
            transform.position = Vector3.Lerp(_startPosition, _endPosition, _chargeAmount);
        }
    }
}
