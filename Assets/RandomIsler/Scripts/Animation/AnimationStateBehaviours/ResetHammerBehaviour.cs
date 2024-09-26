using UnityEngine;

namespace RandomIsleser
{
    public class ResetHammerBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger(Animations.HammerAttackHash);
        }
    }
}
