namespace RandomIsleser
{
    public class RodGrappleMovementState : BaseMovementState
    {
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            //context.Animator.SetBool(Animations.IsGrapplingHash, true);
        }

        public override void OnUpdateState(PlayerController context)
        {
            context.Grapple();
        }
        
        public override void OnExitState(PlayerController context, BasePlayerState previousState)
        {
            //context.Animator.SetBool(Animations.IsGrapplingHash, false);
        }
    }
}
