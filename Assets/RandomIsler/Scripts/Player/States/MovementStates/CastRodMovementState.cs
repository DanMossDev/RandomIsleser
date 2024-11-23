namespace RandomIsleser
{
    public class CastRodMovementState : BaseMovementState
    {
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            context.LocomotionAnimator.SetFloat(Animations.MovementSpeedHash, 0);
        }

        public override void OnExitState(PlayerController context, BasePlayerState nextState)
        {
            if (nextState is AimCombatState)
                return;
            
            context.EndAim();
        }
    }
}
