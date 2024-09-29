namespace RandomIsleser
{
    public class AimCombatState : BaseCombatState
    {
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            if (previousState is CastRodCombatState)
                return;
            
            context.BeginAim();
        }
        
        public override void OnUpdateState(PlayerController context)
        {
            context.Aim();
            context.Move();
        }
        
        public override void OnExitState(PlayerController context, BasePlayerState nextState)
        {
            if (nextState is CastRodCombatState)
                return;
            
            context.EndAim();
        }

        public override void UseItem(PlayerController context)
        {
            context.CurrentlyEquippedItem.UseItem();
        }
    }
}
