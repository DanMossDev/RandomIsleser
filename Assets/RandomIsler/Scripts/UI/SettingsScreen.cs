using UnityEngine;

namespace RandomIsleser
{
    public class SettingsScreen : MenuScreen
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            InputManager.BackInput += Back;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            InputManager.BackInput -= Back;
        }
        
        private void Back()
        {
            _manager.GoToMainMenu();
        }
    }
}
