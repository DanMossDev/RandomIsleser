using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace RandomIsleser
{
    public class MenuScreenManager : MonoBehaviour
    {
        private MenuScreen _currentScreen;

        [SerializeField] private MenuScreen _engageScreen;
        [SerializeField] private MenuScreen _menuScreen;
        [SerializeField] private MenuScreen _settingsScreen;

        private void Start()
        {
            _currentScreen = _engageScreen;
            _currentScreen.CanvasGroup.DOFade(1, 1);
            _currentScreen.gameObject.SetActive(true);
        }

        public void GoToEngageScreen()
        {
            SetScreen(_engageScreen);
        }

        public void GoToMainMenu()
        {
            SetScreen(_menuScreen);
        }

        public void StartGame()
        {
            SceneTransitionManager.LoadScene("TestOpenWorld");
        }

        public void GoToSettingsScreen()
        {
            SetScreen(_settingsScreen);
        }

        private void SetScreen(MenuScreen screen)
        {
            _currentScreen.CanvasGroup.DOFade(0,1).OnComplete(()=>
            {
                _currentScreen.gameObject.SetActive(false);
                _currentScreen = screen;
                _currentScreen.CanvasGroup.DOFade(1,1).OnStart(()=>_currentScreen.gameObject.SetActive(true));
            });
        }

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            return;
#endif
            Application.Quit();
        }
    }
}
