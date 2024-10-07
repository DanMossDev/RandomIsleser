using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class GameManager : MonoBehaviour
    {
        private bool _isPaused = false;
        
        public bool IsPaused => _isPaused;
        private void Start()
        {
            InputManager.PauseInput += PausePressed;
            InputManager.BackInput += BackPressed;
        }

        private void OnDestroy()
        {
            InputManager.PauseInput -= PausePressed;
            InputManager.BackInput -= BackPressed;
        }

        private void BackPressed()
        {
            if (_isPaused)
                Continue();
        }

        private void PausePressed()
        {
            if (_isPaused)
                Continue();
            else
                Pause();
        }

        private void Continue()
        {
            PlayerController.Instance.SubscribeControls();
            Time.timeScale = 1;
            _isPaused = false;
        }

        private void Pause()
        {
            PlayerController.Instance.UnsubscribeControls();
            Time.timeScale = 0;
            _isPaused = true;
        }
    }
}
