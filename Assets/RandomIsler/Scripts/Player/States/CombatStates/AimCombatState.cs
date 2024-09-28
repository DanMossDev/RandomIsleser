namespace RandomIsleser
{
    public class AimCombatState : BaseCombatState
    {
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            context.BeginAim();
        }
        
        public override void OnUpdateState(PlayerController context)
        {
            context.Aim();
            context.Move();
        }
        
        public override void OnLeaveState(PlayerController context, BasePlayerState nextState)
        {
            context.EndAim();
        }
        
        public override void Shoot(PlayerController context)
        {
            if (context.CanAttack)
                context.Attack();
        }
    }
}
