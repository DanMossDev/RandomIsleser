namespace RandomIsleser
{
    public class DefaultPlayerState : BasePlayerState
    {
        public override void OnUpdateState(PlayerController context)
        {
            context.Move();
        }
    }
}
