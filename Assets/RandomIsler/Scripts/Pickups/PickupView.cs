using UnityEngine;

namespace RandomIsleser
{
    [RequireComponent(typeof(SphereCollider))]
    public class PickupView : MonoBehaviour
    {
        [SerializeField] private PickupModel _model;
        protected void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerController player))
                return;

            _model.PickUp();
            gameObject.SetActive(false);
        }
    }
}
