namespace RandomIsleser
{
    public enum PlayerStates
    {
        None,
        DefaultMove,
        RollMove,
        DefaultCombat
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
                case PlayerStates.DefaultCombat:
                    return PlayerStateTypes.Combat;
                default:
                    return PlayerStateTypes.Movement;
            }
        }
    }
}
