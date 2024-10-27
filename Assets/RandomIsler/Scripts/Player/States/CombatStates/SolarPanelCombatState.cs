using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class SolarPanelCombatState : BaseCombatState
    {
        public override void OnUpdateState(PlayerController context)
        {
            context.SetSolarPanelInput();
            context.CurrentlyEquippedItem.UpdateEquippable();
        }
        
        public override void UseItem(PlayerController context)
        {
            context.CurrentlyEquippedItem.UseItem();
        }

        public override void ReleaseItem(PlayerController context)
        {
            context.CurrentlyEquippedItem.ReleaseItem();
        }

        public override void Back(PlayerController context)
        {
            context.UnequipItem();
        }
    }
}
