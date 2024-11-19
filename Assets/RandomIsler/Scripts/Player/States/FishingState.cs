namespace RandomIsleser
{
    public class FishingState : BasePlayerState
    {
        private FishingRodController _fishingRodController;
        
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            _fishingRodController = context.CurrentlyEquippedItem as FishingRodController;
            context.LocomotionAnimator.SetFloat(Animations.MovementSpeedHash, 0);
        }

        public override void OnUpdateState(PlayerController context)
        {
            
        }

        public override void OnExitState(PlayerController context, BasePlayerState nextState)
        {
            
        }

        public override void Interact(PlayerController context)
        {
            
        }

        public override void UseItem(PlayerController context)
        {
            _fishingRodController.BeginReel();
        }

        public override void ReleaseItem(PlayerController context)
        {
            _fishingRodController.EndReel();
        }

        public override void Back(PlayerController context)
        {
            _fishingRodController.BackPressed();
        }
    }
}
