using UnityEngine;

namespace RandomIsleser
{
	public class SetAttackingBehaviour : StateMachineBehaviour
	{
		[SerializeField] private bool _reenable = true;
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			PlayerController.Instance.SetState(PlayerStates.AttackCombat);
		}
        
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (_reenable)
				PlayerController.Instance.SetState(PlayerStates.DefaultMove);
		}
	}
}