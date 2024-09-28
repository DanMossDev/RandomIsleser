namespace RandomIsleser
{
    public enum PlayerStates
    {
        None,
        
        //Movement
        DefaultMove,
        RollMove,
        SwimMove,
        
        //Combat
        IdleCombat,
        AimCombat
    }

    public enum PlayerStateTypes
    {
        None,
        Movement,
        Combat
    }

    public static class PlayerStatesExtensions
    {
        public static PlayerStateTypes GetStateType(this PlayerStates state)
        {
            switch (state)
            {
                case PlayerStates.None:
                    return PlayerStateTypes.None;
                case PlayerStates.IdleCombat:
                case PlayerStates.AimCombat:
                    return PlayerStateTypes.Combat;
                default:
                    return PlayerStateTypes.Movement;
            }
        }
    }
}
