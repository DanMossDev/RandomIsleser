using UnityEngine;

namespace RandomIsleser
{
    public class RollMovementState : BaseMovementState
    {
        private Vector3 _rollDirection;
        private float _timeStarted;

        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            _timeStarted = Time.time;
            _rollDirection = context.LastMoveDirection.normalized;
            context.EquipmentAnimator.SetTrigger(Animations.RollHash);
            context.LocomotionAnimator.SetTrigger(Animations.RollHash);
        }
        
        public override void OnUpdateState(PlayerController context)
        {
            context.Roll(_rollDirection);
            
            if (Time.time - _timeStarted > context.PlayerModel.RollDuration)
                context.SetState(PlayerStates.DefaultMove);
        }

        public override void OnExitState(PlayerController context, BasePlayerState nextState)
        {
            context.EquipmentAnimator.ResetTrigger(Animations.RollHash);
            context.LocomotionAnimator.ResetTrigger(Animations.RollHash);
        }
    }
}
