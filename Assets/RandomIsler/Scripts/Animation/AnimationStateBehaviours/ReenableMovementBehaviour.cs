using UnityEngine;

namespace RandomIsleser
{
    public class ReenableMovementBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PlayerController.Instance.SetCanMove(true);
        }
    }
}
