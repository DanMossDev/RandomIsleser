using UnityEngine;

namespace RandomIsleser
{
    public class AimCombatState : BaseCombatState
    {
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            Services.Instance.UIManager.SetAimingUIVisible(true);
            
            if (previousState is CastRodMovementState)
                return;
            
            context.BeginAim();
        }
        
        public override void OnUpdateState(PlayerController context)
        {
            context.Aim();
            context.Move();
        }
        
        public override void OnExitState(PlayerController context, BasePlayerState nextState)
        {
            Services.Instance.UIManager.SetAimingUIVisible(false);
            
            if (nextState is CastRodMovementState)
                return;
            
            context.EndAim();
        }

        public override void UseItem(PlayerController context)
        {
            context.CurrentlyEquippedItem.UseItem();
        }

        public override void Back(PlayerController context)
        {
            context.SetState(PlayerStates.DefaultMove);
        }
    }
}
