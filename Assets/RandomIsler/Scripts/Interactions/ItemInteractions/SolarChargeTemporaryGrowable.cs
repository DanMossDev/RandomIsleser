using UnityEngine;

namespace RandomIsleser
{
    public class SolarChargeTemporaryGrowable : SolarChargeAbsorber
    {
        [SerializeField] private Transform _endPositionTransform;

        [SerializeField, Tooltip("Rate at which the growable 'un-grows'")] private float _dechargeRate = 1;
        [SerializeField, Tooltip("Delay after being charged before the plant begins to shrink")] private float _dechargeDelay = 1;
        
        private Vector3 _startPosition;
        private Vector3 _endPosition;

        private float _lastChargeTime;
        
        private void Start()
        {
            _startPosition = transform.position;
            _endPosition = _endPositionTransform.position;
        }

        private void FixedUpdate()
        {
            if (_chargeAmount > 0 && Time.time - _lastChargeTime > _dechargeDelay)
                _chargeAmount -= _dechargeRate * Time.fixedDeltaTime;
        }

        public override void ReceiveCharge(float amount)
        {
            _chargeAmount += amount * _chargeRate;

            if (_chargeAmount >= 1)
                _chargeAmount = 1;

            _lastChargeTime = Time.time;
        }
        
        protected override void OnChargeUpdated()
        {
            transform.position = Vector3.Lerp(_startPosition, _endPosition, _chargeAmount);
        }
    }
}
