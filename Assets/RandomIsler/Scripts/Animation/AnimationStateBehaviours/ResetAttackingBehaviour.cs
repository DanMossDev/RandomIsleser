using UnityEngine;

namespace RandomIsleser
{
    public class ResetAttackingBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PlayerController.Instance.SetAttacking(false);
        }
    }
}
