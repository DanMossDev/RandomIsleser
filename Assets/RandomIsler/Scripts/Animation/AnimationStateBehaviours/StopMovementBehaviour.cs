using UnityEngine;

namespace RandomIsleser
{
	public class StopMovementBehaviour : StateMachineBehaviour
	{
		[SerializeField] private bool _reenable = true;
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			PlayerController.Instance.SetCanMove(false);
		}
        
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (_reenable)
				PlayerController.Instance.SetCanMove(true);
		}
	}
}