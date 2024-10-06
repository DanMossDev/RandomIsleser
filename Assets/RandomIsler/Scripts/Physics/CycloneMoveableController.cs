using DG.Tweening;
using UnityEngine;

namespace RandomIsleser
{
    [RequireComponent(typeof(Rigidbody))]
    public class CycloneMoveableController : MonoBehaviour
    {
        [SerializeField] private bool _canBeSuckedUp = true;
        [SerializeField] private bool _canBeHeld = false;
        private Rigidbody _rigidbody;

        private Transform _originalParent;
        
        public bool CanBeSuckedUp => _canBeSuckedUp;
        public bool CanBeHeld => _canBeHeld;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _originalParent = transform.parent;
        }
        
        public void ApplyWindForce(Vector3 force)
        {
            _rigidbody.AddForce(force, ForceMode.Force); //TODO - fix this
        }

        public void FireItem(Vector3 force)
        {
            _rigidbody.isKinematic = false;
            transform.parent = _originalParent;
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }

        public void BeginHold(Transform holdPoint)
        {
            _rigidbody.isKinematic = true;
            transform.parent = holdPoint;
            transform.DOLocalMove(Vector3.zero, 0.1f);
            transform.DOLocalRotate(Vector3.zero, 0.3f);
        }

        public void AbsorbItem(Transform holdPoint)
        {
            _rigidbody.isKinematic = true;
            transform.parent = holdPoint;
            transform.DOLocalMove(Vector3.zero, 0.3f);
            transform.DOScale(Vector3.zero, 0.3f);
        }
    }
}
