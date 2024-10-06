using UnityEngine;

namespace RandomIsleser
{
    public class RodGrappleMovementState : BaseMovementState
    {
        private float _timeEnteredState;
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            _timeEnteredState = Time.time;
            //context.Animator.SetBool(Animations.IsGrapplingHash, true);
        }

        public override void OnUpdateState(PlayerController context)
        {
            context.Grapple();
            
            if (Time.time - _timeEnteredState > 2f)
                context.SetState(PlayerStates.DefaultMove);
        }
        
        public override void OnExitState(PlayerController context, BasePlayerState previousState)
        {
            //context.Animator.SetBool(Animations.IsGrapplingHash, false);
        }
    }
}
