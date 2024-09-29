using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class AttackCombatState : BaseCombatState
    {
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            context.EquipItem(context.MainWeapon);
        }
        
        public override void HammerAttack(PlayerController context)
        {
            if (context.CanAttack)
                context.Attack();
        }
    }
}
