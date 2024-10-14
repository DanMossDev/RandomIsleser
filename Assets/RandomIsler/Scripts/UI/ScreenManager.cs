using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] private List<Screen> _screens;

        private int _currentScreenIndex;
        private int _lastScreenIndex;
        
        private void Awake()
        {
            _lastScreenIndex = 0;
        }

        private void OnEnable()
        {
            _currentScreenIndex = _lastScreenIndex;
            _screens[_currentScreenIndex].gameObject.SetActive(true);

            InputManager.TabInput += NextScreen;
        }

        private void OnDisable()
        {
            _lastScreenIndex = _currentScreenIndex;
            _screens[_currentScreenIndex].gameObject.SetActive(false);
            InputManager.TabInput -= NextScreen;
        }


        private void NextScreen(bool isRight)
        {
            _screens[_currentScreenIndex].gameObject.SetActive(false);
            
            if (isRight)
            {
                _currentScreenIndex++;
                if (_currentScreenIndex == _screens.Count)
                    _currentScreenIndex = 0;
            }
            else
            {
                _currentScreenIndex--;
                if (_currentScreenIndex == -1)
                    _currentScreenIndex = _screens.Count - 1;
            }
            
            _screens[_currentScreenIndex].gameObject.SetActive(true);
        }
        
        public void SetScreen(Screen screen)
        {
            _screens[_currentScreenIndex].gameObject.SetActive(false);
            _currentScreenIndex = _screens.IndexOf(screen);
            _screens[_currentScreenIndex].gameObject.SetActive(true);
        }
    }
}
