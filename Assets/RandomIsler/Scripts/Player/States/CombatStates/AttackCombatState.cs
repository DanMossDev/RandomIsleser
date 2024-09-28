using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class AttackCombatState : BaseCombatState
    {
        public override void OnUpdateState(PlayerController context)
        { }
        
        public override void HammerAttack(PlayerController context)
        {
            if (context.CanAttack)
                context.Attack();
        }
    }
}
