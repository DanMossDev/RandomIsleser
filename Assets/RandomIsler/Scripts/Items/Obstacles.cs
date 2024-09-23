namespace RandomIsleser
{
    [System.Flags]
    public enum Obstacles
    {
        None = 0,
        MetalPlate = 1<<1,
        HorizontalGap = 1<<2,
        VerticalGap = 1<<3,
        Ice = 1<<4,
        Surfing = 1<<5,
        Photograph = 1<<6,
    }
}
