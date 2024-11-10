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
        CastRodMove,
        RodGrappleMove,
        LadderMove,
        
        //Combat
        AimCombat,
        AttackCombat,
        CycloneCombat,
        SolarPanelCombat,
        
        //Misc
        ItemGetState,
        FishingState,
        
        NullState
    }
}
