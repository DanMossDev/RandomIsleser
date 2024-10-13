using UnityEngine;

namespace RandomIsleser
{
    public class InteractionFinder : MonoBehaviour
    {
        private PlayerController _controller;
        private void Start()
        {
            _controller = PlayerController.Instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out InteractionPoint interactable))
                return;
            
            _controller.SetInteractable(interactable.GetInteractable());
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out InteractionPoint interactable))
                return;
            
            _controller.UnsetInteractable(interactable.GetInteractable());
        }
    }
}
