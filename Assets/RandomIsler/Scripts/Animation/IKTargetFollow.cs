using UnityEngine;

namespace RandomIsleser
{
    public class IKTargetFollow : MonoBehaviour
    {
        private Transform _target;
        private bool _isFollowing = false;

        public void SetTarget(Transform target)
        {
            _target = target;
            _isFollowing = _target != null;
        }

        private void FixedUpdate()
        {
            if (!_isFollowing)
                return;
            
            transform.position = _target.position;
            transform.rotation = _target.rotation;
        }
    }
}
