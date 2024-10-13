namespace RandomIsleser
{
    public enum PlayerStates
    {
        None,
        
        //Movement
        DefaultMove,
        OnShipMove,
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
