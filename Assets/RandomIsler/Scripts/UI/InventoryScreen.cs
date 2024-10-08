using UnityEngine.EventSystems;

namespace RandomIsleser
{
    public class InventoryScreen : Screen
    {
        protected override void OnEnable()
        {
            base.OnEnable();

            InputManager.ItemSlot1Input += TryEquipItemInSlot1;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            InputManager.ItemSlot1Input -= TryEquipItemInSlot1;
        }

        private void TryEquipItemInSlot1(bool isPress)
        {
            if (!isPress)
                return;

            if (EventSystem.current.currentSelectedGameObject.TryGetComponent(out EquippableInventoryItem equippableInventoryItem))
            {
                PlayerController.Instance.EquipItemInSlot1(equippableInventoryItem.Item.EquippableType);
            }
        }
    }
}
