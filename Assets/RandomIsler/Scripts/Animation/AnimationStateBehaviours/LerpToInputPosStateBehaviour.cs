using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class LerpToInputPosStateBehaviour : StateMachineBehaviour
    {
        private Vector3 _startPosition;
        private Vector3 _inputPosition;
        private float _stateLength;
        private float _lerpRatio;

        private bool _shouldLerp = false;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _shouldLerp = !PlayerController.Instance.TargetHeld;
            if (!_shouldLerp)
                return;
            
            _lerpRatio = 0;
            _startPosition = animator.transform.forward;
            _inputPosition = PlayerController.Instance.LastMoveDirection;
            _stateLength = stateInfo.length;
        }
        
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!_shouldLerp)
                return;
            
            _lerpRatio += Time.deltaTime * 1.5f / _stateLength;
            Vector3 newRotation = Vector3.Lerp(_startPosition, _inputPosition, _lerpRatio);
            Quaternion newRotationQuaternion = Quaternion.LookRotation(newRotation);
            animator.rootRotation = newRotationQuaternion;
        }
    }
}
