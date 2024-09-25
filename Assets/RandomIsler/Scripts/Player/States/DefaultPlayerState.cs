namespace RandomIsleser
{
    public class DefaultPlayerState : BasePlayerState
    {
        public override void OnUpdateState(PlayerController context)
        {
            context.Move();
        }

        public override void Roll(PlayerController context)
        {
            context.SetState(context.RollState);
        }

        public override void HammerAttack(PlayerController context)
        {
            context.Attack();
        }
    }
}
