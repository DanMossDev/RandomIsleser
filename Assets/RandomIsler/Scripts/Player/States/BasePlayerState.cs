namespace RandomIsleser
{
    public abstract class BasePlayerState
    {
        public virtual void OnEnterState(PlayerController context, BasePlayerState previousState) { }
        
        public virtual void OnUpdateState(PlayerController context) { }
        
        public virtual void OnExitState(PlayerController context, BasePlayerState nextState) { }

        public virtual bool TryAim(PlayerController context) { return false; }
        
        //Combat Methods
        public virtual void HammerAttack(PlayerController context) { }
        
        public virtual void UseItem(PlayerController context) { }
        
        //Movement Methods
        public virtual void Roll(PlayerController context) { }
        
        //Navigation Methods
        public virtual void Back(PlayerController context) { }
    }
}
