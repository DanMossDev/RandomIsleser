using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class ItemGetState : BasePlayerState
    {
        private BasePlayerState _previousState;
        
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            _previousState = previousState;
            CheckValidState(context);
            context.ShowItemGetUI();
            context.LocomotionAnimator.SetFloat(Animations.MovementSpeedHash, 0);
            //context.LocomotionAnimator.SetBool(Animations.ItemGetHash, true);
            CameraManager.Instance.SetItemGetCamera(true);
        }
        
        public override void OnExitState(PlayerController context, BasePlayerState nextState)
        {
            //context.LocomotionAnimator.SetBool(Animations.ItemGetHash, false);
            CameraManager.Instance.SetItemGetCamera(false);
        }

        public override void Back(PlayerController context)
        {
            if (context.CloseItemGetUI())
                context.SetState(_previousState);
        }

        public override void Interact(PlayerController context)
        {
            if (context.CloseItemGetUI())
                context.SetState(_previousState);
        }

        private void CheckValidState(PlayerController context)
        {
            if (_previousState is FishingState)
                _previousState = context.DefaultState;
        }
    }
}
