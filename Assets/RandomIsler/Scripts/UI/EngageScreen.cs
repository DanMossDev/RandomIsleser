using UnityEngine;

namespace RandomIsleser
{
    public class EngageScreen : Screen
    {
        [SerializeField] private MenuScreenManager _manager;

        protected override void Awake()
        {
        }

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
