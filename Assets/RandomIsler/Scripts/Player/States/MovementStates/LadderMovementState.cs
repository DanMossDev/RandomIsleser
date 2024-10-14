namespace RandomIsleser
{
    public class LadderMovementState : BaseMovementState
    {
        public override async void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            context.UnequipItem();
            
            var heightAdjustedPos = context.StateChangeCause.position;
            heightAdjustedPos.y = context.transform.position.y;
            await context.MoveToTargetPosition(heightAdjustedPos + context.StateChangeCause.forward, 2);
            context.SnapToInputDirection(context.StateChangeCause.forward * -1);
            context.LocomotionAnimator.SetBool(Animations.OnLadderHash, true);
        }
        
        public override void OnUpdateState(PlayerController context)
        {
            context.LocomotionAnimator.ResetTrigger(Animations.AscendLadderHash);
            context.LocomotionAnimator.ResetTrigger(Animations.DescendLadderHash);
            
            context.LadderMove();
        }

        public override void Back(PlayerController context)
        {
            context.SetState(PlayerStates.DefaultMove);
        }
        
        public override void OnExitState(PlayerController context, BasePlayerState previousState)
        {
            context.LocomotionAnimator.ResetTrigger(Animations.ExitLadderHash);
            context.LocomotionAnimator.SetBool(Animations.OnLadderHash, false);
            context.StateChangeCause = null;
            context.ResetUp();
        }
    }
}
