namespace MossUtils
{
    [System.Serializable]
    public class InventoryData
    {
        public Unlockables Unlockables;
        public int Currency;
        
        public void SetItemUnlocked(Unlockables flag, bool value)
        {
            if (value)
                Unlockables |= flag;
            else
                Unlockables &= ~flag;
        }

        public void AddCurrency(int amount)
        {
            Currency += amount;
        }

        public bool ItemUnlocked(Unlockables flag)
        {
            return (Unlockables & flag) > 0;
        }
    }
}
