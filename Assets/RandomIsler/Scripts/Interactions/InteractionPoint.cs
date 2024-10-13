using UnityEngine;

namespace RandomIsleser
{
    public class InteractionPoint : MonoBehaviour
    {
        private Interactable _interactable;

        private void Awake()
        {
            _interactable = GetComponentInParent<Interactable>();
        }

        public Interactable GetInteractable()
        {
            return _interactable;
        }
    }
}
