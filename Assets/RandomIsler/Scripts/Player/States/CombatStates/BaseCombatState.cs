namespace RandomIsleser
{
    public abstract class BaseCombatState : BasePlayerState
    {
        public virtual void HammerAttack(PlayerController context) { }
        
        public virtual void Shoot(PlayerController context) { }
    }
}
