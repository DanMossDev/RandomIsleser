using UnityEngine;
using UnityEngine.UI;

namespace RandomIsleser
{
    public class QuestScreen : Screen
    {
        [SerializeField] private float _scrollSpeed;
        [SerializeField] private ScrollRect _questLog;

        private float _scrollRate;
        
        protected override void OnEnable()
        {
            base.OnEnable();

            InputManager.CameraInput += ScrollQuestLog;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            InputManager.CameraInput -= ScrollQuestLog;
        }

        private void Update()
        {
            _questLog.verticalNormalizedPosition += Time.unscaledDeltaTime * _scrollRate;
        }

        private void ScrollQuestLog(Vector2 input)
        {
            _scrollRate = input.y * _scrollSpeed;
        }
    }
}
