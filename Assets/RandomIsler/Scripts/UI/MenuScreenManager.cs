using UnityEngine;

namespace RandomIsleser
{
    public class MenuScreenManager : MonoBehaviour
    {
        private Screen _currentScreen;

        [SerializeField] private Screen _engageScreen;
        [SerializeField] private Screen _menuScreen;
        [SerializeField] private Screen _settingsScreen;

        private void Awake()
        {
            _currentScreen = _engageScreen;
            _currentScreen.gameObject.SetActive(true);
        }

        public void GoToEngageScreen()
        {
            _currentScreen.gameObject.SetActive(false);
            _currentScreen = _engageScreen;
            _currentScreen.gameObject.SetActive(true);
        }

        public void GoToMainMenu()
        {
            _currentScreen.gameObject.SetActive(false);
            _currentScreen = _menuScreen;
            _currentScreen.gameObject.SetActive(true);
        }

        public void GoToSettingsScreen()
        {
            _currentScreen.gameObject.SetActive(false);
            _currentScreen = _settingsScreen;
            _currentScreen.gameObject.SetActive(true);
        }
    }
}
