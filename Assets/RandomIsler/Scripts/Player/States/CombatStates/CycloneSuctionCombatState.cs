namespace RandomIsleser
{
    public class CycloneSuctionCombatState : BaseCombatState
    {
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            context.SetStateSpeedMultiplier(((CycloneJarController)context.CurrentlyEquippedItem).MovementSpeedMultiplier);
            context.SetStateRotationMultiplier(((CycloneJarController)context.CurrentlyEquippedItem).RotationSpeedMultiplier);
            context.Animator.SetBool(Animations.CycloneJarChargingHash, true);
        }
        
        public override void OnUpdateState(PlayerController context)
        {
            context.Move();
            context.RotatePlayer();
            context.CurrentlyEquippedItem.UpdateEquippable();
        }
        
        public override void OnExitState(PlayerController context, BasePlayerState nextState)
        {
            context.SetStateSpeedMultiplier(1);
            context.SetStateRotationMultiplier(1);
            context.Animator.SetBool(Animations.CycloneJarChargingHash, false);
        }

        public override void ReleaseItem(PlayerController context)
        {
            context.CurrentlyEquippedItem.ReleaseItem();
        }
    }
}
