namespace RandomIsleser
{
    public class CastRodCombatState : BaseCombatState
    {
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            PlayerController.Instance.Animator.SetTrigger(Animations.FishingRodCastHash);
        }

        public override void OnExitState(PlayerController context, BasePlayerState nextState)
        {
            context.EndAim();
        }
    }
}
