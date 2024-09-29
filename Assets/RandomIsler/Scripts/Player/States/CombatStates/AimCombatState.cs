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
        
        public override void OnLeaveState(PlayerController context, BasePlayerState nextState)
        {
            context.EndAim();
            context.EquipAimable(null);
        }
        
        public override void Shoot(PlayerController context)
        {
            //context.CurrentAimableWeapon.Shoot();
        }
    }
}
