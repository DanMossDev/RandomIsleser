namespace RandomIsleser
{
    public class DefaultMovementState : BaseMovementState
    {
        public override void OnUpdateState(PlayerController context)
        {
            context.Move();
            context.RotatePlayer();
        }

        public override void Roll(PlayerController context)
        {
            context.SetState(PlayerStates.RollMove);
        }
    }
}
