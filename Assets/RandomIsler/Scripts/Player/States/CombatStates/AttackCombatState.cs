using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class AttackCombatState : BaseCombatState
    {
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            context.SetStateRotationMultiplier(context.PlayerModel.AttackingRotationMultiplier);
            context.EquipItem(context.MainWeapon);
        }
        
        public override void OnExitState(PlayerController context, BasePlayerState nextState)
        {
            context.SetStateRotationMultiplier(1);
        }
        
        public override void HammerAttack(PlayerController context)
        {
            if (context.CanAttack)
                context.Attack();
        }
    }
}
