using UnityEngine;

namespace RandomIsleser
{
    public class LadderMovementBehaviour : StateMachineBehaviour
    {
        [SerializeField] private bool _isAscending;
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PlayerController.Instance.LadderRootMovement(_isAscending);
        }
    }
}
