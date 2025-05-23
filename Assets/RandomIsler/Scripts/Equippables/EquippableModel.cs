using MossUtils;
using UnityEngine;
using UnityEngine.Localization;

namespace RandomIsleser
{
    public class EquippableModel : PickupModel
    {
        [Space, Header("Equippable Settings")] 
        public int ItemIndex; //TODO consider cha
        public Equippables EquippableType;
        public Unlockables UnlockableType;

        public bool Slottable = true;
        public bool Unlocked => RuntimeSaveManager.Instance.CurrentSaveSlot.InventoryData.ItemUnlocked(UnlockableType);
        public Sprite Sprite;

        public override void PickUp()
        {
            PlayerController.Instance.UnlockItem(UnlockableType);
        }
    }
}
