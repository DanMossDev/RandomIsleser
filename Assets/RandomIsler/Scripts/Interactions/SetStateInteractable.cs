using UnityEngine;

namespace RandomIsleser
{
    public class SetStateInteractable : MonoBehaviour, Interactable
    {
        [SerializeField] private PlayerStates _state;
        public void Interact()
        {
            PlayerController.Instance.SetState(_state, transform);
        }
    }
}
