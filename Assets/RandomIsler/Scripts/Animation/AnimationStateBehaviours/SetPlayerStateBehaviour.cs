using UnityEngine;

namespace RandomIsleser
{
    public class SetPlayerStateBehaviour : StateMachineBehaviour
    {
        [SerializeField] private PlayerStates _stateToSetOnStart;
        [SerializeField] private PlayerStates _stateToSetOnEnd;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_stateToSetOnStart != PlayerStates.None)
                PlayerController.Instance.SetState(_stateToSetOnStart);
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_stateToSetOnEnd != PlayerStates.None)
                PlayerController.Instance.SetState(_stateToSetOnEnd);
        }
    }
}
