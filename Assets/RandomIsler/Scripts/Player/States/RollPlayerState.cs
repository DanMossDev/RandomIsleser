using UnityEngine;

namespace RandomIsleser
{
    public class RollPlayerState : BasePlayerState
    {
        private Vector3 _rollDirection;
        private float _timeStarted;

        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            _timeStarted = Time.time;
            _rollDirection = context.LastMoveDirection.normalized;
            context.SnapToInputDirection(_rollDirection);
            context.Animator.SetTrigger(Animations.RollHash);
        }
        
        public override void OnUpdateState(PlayerController context)
        {
            context.Roll(_rollDirection);
            
            if (Time.time - _timeStarted > context.PlayerModel.RollDuration)
                context.SetState(context.DefaultState);
        }

        public override void OnLeaveState(PlayerController context, BasePlayerState nextState)
        {
            context.Animator.ResetTrigger(Animations.RollHash);
        }
    }
}
