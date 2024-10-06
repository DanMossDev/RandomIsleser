namespace RandomIsleser
{
    public class CycloneCombatState : BaseCombatState
    {
        private CycloneJarController _controller;
        
        public override void OnEnterState(PlayerController context, BasePlayerState previousState)
        {
            context.SetStateSpeedMultiplier(((CycloneJarController)context.CurrentlyEquippedItem).MovementSpeedMultiplier);
            context.SetStateRotationMultiplier(((CycloneJarController)context.CurrentlyEquippedItem).RotationSpeedMultiplier);
            
            _controller = context.CurrentlyEquippedItem as CycloneJarController;
        }
        
        public override void OnUpdateState(PlayerController context)
        {
            context.Move();
            context.CycloneRotatePlayer();
            context.CurrentlyEquippedItem.UpdateEquippable();
        }
        
        public override void OnExitState(PlayerController context, BasePlayerState nextState)
        {
            context.SetStateSpeedMultiplier(1);
            context.SetStateRotationMultiplier(1);
        }

        public override void ReleaseItem(PlayerController context)
        {
            context.CurrentlyEquippedItem.ReleaseItem();
        }

        public override void Back(PlayerController context)
        {
            context.EquipItem(null);
        }
    }
}
