using UnityEngine;

namespace RandomIsleser
{
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] private Screen _defaultScreen;

        private Screen _currentScreen;
        private Screen _lastScreen;
        
        private void Awake()
        {
            _lastScreen = _defaultScreen;
        }

        private void OnEnable()
        {
            _currentScreen = _lastScreen;
            _currentScreen.gameObject.SetActive(true);
        }

        public void SetScreen(Screen screen)
        {
            _currentScreen.gameObject.SetActive(false);
            _currentScreen = screen;
            _currentScreen.gameObject.SetActive(true);
        }
    }
}
