namespace RandomIsleser
{
    public class SwimMovementState : BaseMovementState
    {
        public override void OnUpdateState(PlayerController context)
        {
            context.Swim();
            context.RotatePlayer();
        }
    }
}
