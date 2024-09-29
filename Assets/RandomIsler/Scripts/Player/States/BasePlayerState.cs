namespace RandomIsleser
{
    public abstract class BasePlayerState
    {
        public virtual void OnEnterState(PlayerController context, BasePlayerState previousState) { }
        
        public abstract void OnUpdateState(PlayerController context);
        
        public virtual void OnLeaveState(PlayerController context, BasePlayerState nextState) { }
        
        //Combat Methods
        public virtual void HammerAttack(PlayerController context) { }
        
        public virtual void Shoot(PlayerController context) { }
        
        //Movement Methods
        public virtual void Roll(PlayerController context) { }
    }
}
