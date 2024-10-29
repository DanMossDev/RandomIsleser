using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class SolarChargeAbsorber : MonoBehaviour
    {
        [SerializeField] protected float _chargeRate = 1;

        protected bool _fullyCharged;

        protected float _chargeField;

        protected float _chargeAmount
        {
            get => _chargeField;
            set
            {
                _chargeField = value;
                OnChargeUpdated();
            }
        }

        public virtual void ReceiveCharge(float amount)
        {
            if (_fullyCharged)
                return;
            
            _chargeAmount += amount * _chargeRate;

            if (_chargeAmount >= 1)
                CompleteCharge();
        }

        public virtual void CompleteCharge()
        {
            if (_fullyCharged)
                return;
            _chargeAmount = 1;
            _fullyCharged = true;
        }

        protected virtual void OnChargeUpdated()
        { }
        
        public virtual void Reflect(Vector3 point, Vector3 normal, LineRenderer lineRenderer)
        { }
    }
}
