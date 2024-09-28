namespace RandomIsleser
{
    public class IdleCombatState : BaseCombatState
    {
        public override void OnUpdateState(PlayerController context)
        { }
        
        public override void HammerAttack(PlayerController context)
        {
            if (context.CanAttack)
                context.Attack();
        }
    }
}
