namespace RandomIsleser
{
    [System.Flags]
    public enum Unlockables
    {
        None = 0,
        Hammer = (1<<1),
        FishingRod = (1<<2),
        CycloneJar  = (1<<3),
        SolarPanel = (1<<4),
    }
}
