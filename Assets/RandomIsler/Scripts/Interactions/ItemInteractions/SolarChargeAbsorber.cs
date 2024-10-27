using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class SolarChargeAbsorber : MonoBehaviour
    {
        [SerializeField] protected float _chargeRate = 1;

        protected bool _fullyCharged;
        protected float _chargeAmount;

        public virtual void ReceiveCharge(float amount)
        {
            _chargeAmount += amount * _chargeRate;

            if (_chargeAmount >= 1)
                CompleteCharge();
        }

        public virtual void CompleteCharge()
        {
            if (_fullyCharged)
                return;

            _fullyCharged = true;
        }
    }
}
