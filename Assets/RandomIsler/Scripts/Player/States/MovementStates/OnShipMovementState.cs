namespace RandomIsleser
{
	public class OnShipMovementState : BaseMovementState
	{
		public override void OnEnterState(PlayerController context, BasePlayerState previousState)
		{
			context.UnsubscribeControls();
		}
		
		public override void OnExitState(PlayerController context, BasePlayerState previousState)
		{
			context.SubscribeControls();
		}
	}
}
