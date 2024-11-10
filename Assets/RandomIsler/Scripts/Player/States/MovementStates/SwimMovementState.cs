namespace RandomIsleser
{
    public class SwimMovementState : BaseMovementState
    {
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            context.SetRigidbodyMovement(true);
            context.SetControllerAndRigidbodyVelocitiesEqual(false);
        }
        
        public override void OnUpdateState(PlayerController context)
        {
            context.Swim();
            context.RotatePlayer();
        }
        
        public override void OnExitState(PlayerController context, BasePlayerState nextState)
        {
            context.SetRigidbodyMovement(false);
            context.SetControllerAndRigidbodyVelocitiesEqual(true);
        }
    }
}
