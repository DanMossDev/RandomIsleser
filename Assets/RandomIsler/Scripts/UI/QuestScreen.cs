using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RandomIsleser
{
    public class QuestScreen : Screen
    {
        [SerializeField] private float _scrollSpeed;
        [SerializeField] private ScrollRect _questLog;

        [SerializeField] private Transform _questContent;

        private float _scrollRate;

        private List<QuestLog> _questLogs = new List<QuestLog>();
        
        protected override void OnEnable()
        {
            base.OnEnable();

            InputManager.CameraInput += ScrollQuestLog;

            Populate();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            InputManager.CameraInput -= ScrollQuestLog;

            Depopulate();
        }

        private void Populate()
        {
            var data = Services.Instance.RuntimeSaveManager.LocalSaveData.QuestSaveData;

            foreach (var quest in data.InProgressQuestModels)
            {
                var questContent = Services.Instance.ObjectPoolController.Get("QuestLog");
                if (questContent.TryGetComponent(out QuestLog questLog))
                {
                    questLog.Populate(quest);
                    questLog.transform.SetParent(_questContent);
                    _questLogs.Add(questLog);
                }
                else
                    questContent.SetActive(false);
            }
        }

        private void Depopulate()
        {
            foreach (var log in _questLogs)
            {
                Services.Instance.ObjectPoolController.Return(log.gameObject, "QuestLog");
            }
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
