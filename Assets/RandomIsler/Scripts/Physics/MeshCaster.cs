using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    [RequireComponent(typeof(MeshCollider))]
    public class MeshCaster : MonoBehaviour
    {
        private Dictionary<int, Collider> _colliders = new Dictionary<int, Collider>();
        
        private void OnTriggerEnter(Collider other)
        {
            int instanceID = other.GetInstanceID();
            _colliders.TryAdd(instanceID, other);
        }

        private void OnTriggerExit(Collider other)
        {
            int instanceID = other.GetInstanceID();
            if (_colliders.ContainsKey(instanceID))
                _colliders.Remove(instanceID);
        }

        public List<Collider> GetColliders()
        {
            return new List<Collider>(_colliders.Values);
        }
    }
}
