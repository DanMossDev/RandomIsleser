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
        
        public override void HammerAttack(PlayerController context)
        {
            if (context.CanAttack)
            {
                context.Attack();
                context.SetState(PlayerStates.AttackCombat);
            }
        }

        public override bool TryAim(PlayerController context)
        {
            context.SetState(PlayerStates.AimCombat);
            return true;
        }
    }
}
