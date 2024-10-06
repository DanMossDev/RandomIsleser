namespace RandomIsleser
{
    public enum PlayerStates
    {
        None,
        
        //Movement
        DefaultMove,
        RollMove,
        SwimMove,
        GrappleMove,
        CastRodMovement,
        RodGrappleMovement,
        
        //Combat
        AimCombat,
        AttackCombat,
        CycloneCombat
    }
}
