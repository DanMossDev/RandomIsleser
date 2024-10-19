using UnityEngine;

namespace RandomIsleser
{
    public class ApplyRootMotionBehaviour : StateMachineBehaviour
    {
        [SerializeField] private bool _disableCollision;
        [SerializeField] private bool _includeRotation;

        private Transform _playerTransform;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerTransform = PlayerController.Instance.transform;
            if (_disableCollision)
            {
                PlayerController.Instance.CharacterController.enabled = false;
            }
        }
        
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_disableCollision)
                _playerTransform.position += animator.deltaPosition;
            else
                PlayerController.Instance.MoveRaw(animator.deltaPosition);

            if (_includeRotation)
                _playerTransform.rotation = animator.rootRotation;
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_disableCollision)
                PlayerController.Instance.CharacterController.enabled = true;
        }
    }
}
