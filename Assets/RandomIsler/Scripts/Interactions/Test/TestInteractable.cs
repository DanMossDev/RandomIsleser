using UnityEngine;

namespace RandomIsleser
{
    public class TestInteractable : MonoBehaviour, Interactable
    {
        public void Interact()
        {
            Debug.Log("Interacting!");
        }
    }
}
