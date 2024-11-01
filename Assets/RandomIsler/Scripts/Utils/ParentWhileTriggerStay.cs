using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class ParentWhileTriggerStay : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        
        private Dictionary<Transform, Transform> _parentLookup = new Dictionary<Transform, Transform>();

        private void OnTriggerEnter(Collider other)
        {
            _parentLookup.Add(other.transform, other.transform.parent);
            other.transform.parent = _parent;
        }
        
        private void OnTriggerExit(Collider other)
        {
            other.transform.parent = _parentLookup[other.transform];
            _parentLookup.Remove(other.transform);
        }
    }
}
