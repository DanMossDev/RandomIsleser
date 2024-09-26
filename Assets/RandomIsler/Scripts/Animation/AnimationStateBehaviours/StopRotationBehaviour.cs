using UnityEngine;

namespace RandomIsleser
{
    public class StopRotationBehaviour : StateMachineBehaviour
    {
        [SerializeField] private bool _reenable = true;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PlayerController.Instance.SetCanRotate(false);
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_reenable)
                PlayerController.Instance.SetCanRotate(true);
        }
    }
}
