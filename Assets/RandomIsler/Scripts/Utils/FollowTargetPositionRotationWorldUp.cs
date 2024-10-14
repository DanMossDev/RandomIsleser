using UnityEngine;

namespace RandomIsleser
{
    public class FollowTargetPositionRotationWorldUp : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        private void FixedUpdate()
        {
            transform.position = _target.position;
            transform.rotation = _target.rotation;
            transform.up = Vector3.up;
        }
    }
}
