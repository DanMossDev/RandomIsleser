using UnityEngine;

namespace RandomIsleser
{
    [RequireComponent(typeof(Rigidbody))]
    public class WindMoveableController : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public void ApplyWindForce(Vector3 origin, Vector3 force)
        {
            force /= Vector3.SqrMagnitude(transform.position - origin);
            _rigidbody.AddForce(force, ForceMode.Force); //TODO - fix this
        }

        public void ApplyWindImpulse(Vector3 force)
        {
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }
    }
}
