namespace RandomIsleser
{
    [System.Serializable]
    public class InventoryData
    {
        public Unlockables Unlockables;
        public int Currency;

        public void Initialise()
        {
            Unlockables = Unlockables.None;
            Currency = 0;
        }
        
        public void SetItemUnlocked(Unlockables flag, bool value)
        {
            if (value)
                Unlockables |= flag;
            else
                Unlockables &= ~flag;
        }

        public int AddCurrency(int amount)
        {
            Currency += amount;
            return Currency;
        }

        public bool ItemUnlocked(Unlockables flag)
        {
            return (Unlockables & flag) > 0;
        }
    }
}
