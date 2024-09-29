namespace RandomIsleser
{
    public class AimCombatState : BaseCombatState
    {
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            context.BeginAim();
            context.EquipAimable(context.FishingRodController);
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
            context.EquipAimable(null);
        }
        
        public override void CastRod(PlayerController context)
        {
            context.CastRodFromAim();
        }
    }
}
