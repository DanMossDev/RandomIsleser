using UnityEngine;

namespace RandomIsleser
{
	public class SetAttackingBehaviour : StateMachineBehaviour
	{
		[SerializeField] private bool _reenable = true;
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			PlayerController.Instance.SetAttacking(true);
		}
        
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (_reenable)
				PlayerController.Instance.SetAttacking(false);
		}
	}
}