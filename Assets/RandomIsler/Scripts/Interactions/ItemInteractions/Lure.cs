using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class Lure : MonoBehaviour
    {
        [SerializeField] private float _nibbleForce = 5;
        [SerializeField] private float _biteForce = 10;

        [SerializeField] private float _interactCooldown = 1;

        private Rigidbody _rigidbody;

        private float _interactTime;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Interact()
        {
            if (Time.time - _interactTime < _interactCooldown)
                return;
            
            _interactTime = Time.time;

            float roll = Random.Range(0f, 1f);

            if (roll > 0.75f)
                Bite();
            else
                Nibble();
        }
        
        private void Nibble()
        {
            ApplyDownwardsForce(_nibbleForce);
        }

        private void Bite()
        {
            ApplyDownwardsForce(_biteForce);
        }

        private void ApplyDownwardsForce(float force)
        {
            _rigidbody.AddForce(Vector3.up * -force, ForceMode.Impulse);
        }
    }
}
