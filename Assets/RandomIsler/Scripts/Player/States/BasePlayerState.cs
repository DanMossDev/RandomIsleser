namespace RandomIsleser
{
    public abstract class BasePlayerState
    {
        public virtual void OnEnterState(PlayerController context, BasePlayerState previousState) { }
        
        public abstract void OnUpdateState(PlayerController context);
        
        public virtual void OnLeaveState(PlayerController context, BasePlayerState nextState) { }
        
        public virtual void Roll(PlayerController context) { }
        
        public virtual void HammerAttack(PlayerController context) { }
    }
}
