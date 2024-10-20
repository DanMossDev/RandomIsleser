using UnityEngine;

namespace RandomIsleser
{
    public class SetBoolInteractable : MonoBehaviour, Interactable
    {
        [SerializeField] private SaveableBool _boolToSet;
        [SerializeField] private bool _valueToSet = true;

        public void Interact()
        {
            _boolToSet.Value = _valueToSet;
        }
    }
}
