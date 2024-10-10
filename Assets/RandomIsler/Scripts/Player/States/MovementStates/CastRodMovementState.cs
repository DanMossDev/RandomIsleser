namespace RandomIsleser
{
    public class CastRodMovementState : BaseMovementState
    {
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            PlayerController.Instance.EquipmentAnimator.SetTrigger(Animations.FishingRodCastHash);
        }

        public override void OnExitState(PlayerController context, BasePlayerState nextState)
        {
            if (nextState is AimCombatState)
                return;
            
            context.EndAim();
        }
    }
}
