using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "RewardModel", menuName = "RandomIsler/Quests/RewardModel")]
    public class RewardModel : ScriptableObject
    {
        public Unlockables UnlockableReward;
        public int CurrencyReward;

        public void UnlockReward()
        {
            if (UnlockableReward != Unlockables.None)
                PlayerController.Instance.UnlockItem(UnlockableReward);
            else if (CurrencyReward > 0)
                PlayerController.Instance.AddCurrency(CurrencyReward);
        }
    }
}
