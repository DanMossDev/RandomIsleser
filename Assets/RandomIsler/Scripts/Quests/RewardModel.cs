using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "RewardModel", menuName = AssetMenuNames.Models + "RewardModel")]
    public class RewardModel : ScriptableObject
    {
        public Unlockables UnlockableReward;
        public int CurrencyReward;
        public KeyRewardTypes KeyRewardType;

        public void UnlockReward()
        {
            if (UnlockableReward != Unlockables.None)
                PlayerController.Instance.UnlockItem(UnlockableReward);
            else if (CurrencyReward > 0)
                PlayerController.Instance.AddCurrency(CurrencyReward);
            else if (KeyRewardType != KeyRewardTypes.None)
	            DungeonController.Instance.AddKey(KeyRewardType);
        }
    }

    public enum KeyRewardTypes
    {
	    None,
	    Normal,
	    Big
    }
}
