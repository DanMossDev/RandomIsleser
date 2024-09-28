using UnityEngine;

namespace RandomIsleser
{
    public class ResetHammerVariablesOnExitStateBehaviour : StateMachineBehaviour
    {
        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            PlayerController.Instance.SetAttacking(false);
            PlayerController.Instance.SetCanMove(true);
            animator.ResetTrigger(Animations.HammerAttackHash);
        }
    }
}
