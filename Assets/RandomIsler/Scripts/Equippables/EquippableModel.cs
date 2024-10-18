using MossUtils;
using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    public class EquippableModel : PickupModel
    {
        [Space, Header("Equippable Settings")] 
        public LocalizedString Name;
        public int ItemIndex; //TODO consider cha
        public Equippables EquippableType;
        public Unlockables UnlockableType;

        public bool Slottable = true;
        public bool Unlocked => Services.Instance.RuntimeSaveManager.LocalSaveData.InventoryData.ItemUnlocked(UnlockableType);
        public Sprite Sprite;

        public override void PickUp()
        {
            PlayerController.Instance.UnlockItem(UnlockableType);
        }
    }
}
