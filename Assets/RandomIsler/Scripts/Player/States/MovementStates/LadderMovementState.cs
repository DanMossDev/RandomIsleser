namespace RandomIsleser
{
    public class LadderMovementState : BaseMovementState
    {
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            context.UnequipItem();
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
            context.LocomotionAnimator.SetBool(Animations.OnLadderHash, false);
        }
    }
}
