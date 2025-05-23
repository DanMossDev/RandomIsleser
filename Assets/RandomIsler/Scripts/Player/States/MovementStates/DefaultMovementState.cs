namespace RandomIsleser
{
    public class DefaultMovementState : BaseMovementState
    {
        public override void OnUpdateState(PlayerController context)
        {
            context.Move();
            context.RotatePlayer();
        }

        public override void OnExitState(PlayerController context, BasePlayerState nextState)
        {
            context.LocomotionAnimator.SetFloat(Animations.MovementSpeedHash, 0);
        }

        public override void Interact(PlayerController context)
        {
            if (!context.HasInteractable)
                return;

            context.Interact();
        }

        public override void Roll(PlayerController context)
        {
            context.SetState(PlayerStates.RollMove);
        }
        
        public override void HammerAttack(PlayerController context)
        {
            if (context.CanAttack)
            {
                context.Attack();
                context.SetState(PlayerStates.AttackCombat);
            }
        }

        public override void UseItem(PlayerController context)
        {
            if (context.CanAttack)
            {
                context.CurrentlyEquippedItem.UseItem();
            }
        }

        public override bool TryAim(PlayerController context)
        {
            context.SetState(PlayerStates.AimCombat);
            return true;
        }
    }
}
