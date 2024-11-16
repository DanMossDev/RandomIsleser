using UnityEngine;

namespace RandomIsleser
{
    public class EngageScreen : MenuScreen
    {
        protected override void OnEnable()
        {
            InputManager.AnyInput += GoToMainMenu;
        }

        protected override void OnDisable()
        {
            InputManager.AnyInput -= GoToMainMenu;
        }

        private void GoToMainMenu()
        {
            _manager.GoToMainMenu();
        }
    }
}
